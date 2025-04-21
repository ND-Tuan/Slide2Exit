using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int NumberOfLevelsAvailable;

    private PlayerData playerData = new();
    public PlayerData PlayerData => playerData;

    private MapManager currentMapManager;
    private Dictionary<(int, int), int> levelStatusValue = new();
    private Dictionary<(int, int), LevelSelectBox> levelSelectBoxes = new();
    public LevelID CurrentLevel { get; private set; }
    private AsyncOperationHandle<SceneInstance>? currentSceneHandle;

    private string CurrentScenePath => $"Assets/Scenes/Stage_{CurrentLevel.Stage}/{CurrentLevel.Stage}_{CurrentLevel.Index}.unity";

    public int MoveCount { get; private set; }
    public int MoveCountLimit { get; private set; }
    private int[] moveToGetStar = new int[3];

    //====Singleton================
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadData();
    }

    private void LoadData()
    {
        //Load player data
        playerData = SaveSystem.LoadPlayerData();
        
        if (playerData == null)
        {
            playerData = new PlayerData
            {
                Coin = 100,
                SkinOwned = new List<int> { 0 },
                SkinEquipped = 0,
                MusicVolume = 1f,
                SFXVolume = 1f,
                GuideUnlocked = new List<int>()
            };
        }

        //Load level data
        levelStatusValue = SaveSystem.LoadLevelData()?.Data;
        if (levelStatusValue == null)
        {
            levelStatusValue = new Dictionary<(int, int), int>
            {
                { (1, 1), 0 }
            };
        }
    }

    //====Level Manager================
    public void RegisterLevelSelectBox(int stage, int levelIndex, LevelSelectBox levelSelectBox)
    {
        levelSelectBoxes[(stage, levelIndex)] = levelSelectBox;
    }

    //====Load level================
    public async void LoadLevel(int stage, int level)
{
        CurrentLevel = new LevelID { Stage = stage, Index = level };

        var handle = Addressables.LoadSceneAsync(CurrentScenePath, LoadSceneMode.Additive);

        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded){
            currentSceneHandle = handle;
        } else {
            Debug.LogError($"Failed to load scene at address: {CurrentScenePath}");
        }
    }

    public void ReloadLevel()
    {
        currentMapManager.ResetMap();
    }

    public void LoadNextLevel()
    {
        var nextLevelKey = (CurrentLevel.Stage, CurrentLevel.Index + 1);

        if (!levelStatusValue.ContainsKey(nextLevelKey))
        {
            UIController.Instance.OnClickBackToLevelSelect();
            return;
        }
        
        UnloadLevel();
        LoadLevel(nextLevelKey.Stage, nextLevelKey.Item2);
    }

    public async void UnloadLevel()
    {
        if (currentSceneHandle.HasValue)
        {
            await Addressables.UnloadSceneAsync(currentSceneHandle.Value).Task;
            currentSceneHandle = null;
        }
    }

    public int GetLevelStatusValue(int stage, int levelIndex)
    {
        return levelStatusValue.TryGetValue((stage, levelIndex), out int value) ? value : -1;
    }

    //===============================================

    //====Khởi tạo thông số cho level================
    public void SetUpMap(MapManager mapManager,int moveCountLimit, int[] moveToGetStar)
    {
        currentMapManager = mapManager;
        MoveCount = moveCountLimit;
        MoveCountLimit = moveCountLimit;
        this.moveToGetStar = moveToGetStar;

        //Khởi tạo UI
        UIController.Instance.ShowInPlayLevelUI();
        UIController.Instance.SetMoveCountUI(MoveCount, MoveCountLimit, moveToGetStar);
    }

    //=========Tính toán số lần di chuyển================
    public void MinusMoveCount()
    {
        MoveCount--;
        UIController.Instance.SetMoveCountUI(MoveCount, MoveCountLimit, moveToGetStar);

        if (MoveCount <= 0)
            GameOver();
    }

    //==================================================

    //====Xử lý trạng thái game=========================
    public void GameWin()
    {
        int star = moveToGetStar.Count(threshold => MoveCount >= threshold);

        UIController.Instance.ShowLevelCompletePanel(star, MoveCount);

        UpdateLevelStatus(CurrentLevel.Stage, CurrentLevel.Index, star);

        //Cập nhật trạng thái mở khóa level tiếp theo
        var nextLevelKey = (CurrentLevel.Stage, CurrentLevel.Index + 1);
        
        if  (   levelSelectBoxes.ContainsKey(nextLevelKey) &&
                !levelStatusValue.ContainsKey(nextLevelKey) &&
                levelStatusValue.Count() < NumberOfLevelsAvailable
            )
        {
            
            levelStatusValue.Add(nextLevelKey, 0);
        }

        //cập nhật dữ liệu vào file
        SaveSystem.SaveLevelData(new LevelData(levelStatusValue));
    }

    private void GameOver()
    {
        UIController.Instance.ShowGameOverPanel();
    }

    //Cập nhật lại trạng thái level
    private void UpdateLevelStatus(int stage, int index, int star)
    {
        var key = (stage, index);
        if (levelStatusValue.ContainsKey(key)){
            levelStatusValue[key] = Mathf.Max(levelStatusValue[key], star);
            
        } else {
            levelStatusValue.Add(key, star);
        }
    }

    //==========================================

    //====Lưu dữ liệu===========================
    public void SaveSettingsData(float musicVolume, float sfxVolume)
    {
        playerData.MusicVolume = musicVolume;
        playerData.SFXVolume = sfxVolume;

        SaveSystem.SavePlayerData(playerData);
    }
}

public struct LevelID
{
    public int Stage;
    public int Index;
} 
