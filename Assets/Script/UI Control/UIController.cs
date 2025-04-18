using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    [Header("Main Panels")]
    [SerializeField] private GameObject BlurPanel;
    [SerializeField] private GameObject HomePanel;
    [SerializeField] private PauseSettingPanel SettingPanel;
    [SerializeField] private LevelCompletePanel levelCompletePanel;
    [SerializeField] private LevelFailedPanel levelFailedPanel;
    [SerializeField] private PauseSettingPanel PausePanel;
    
    [Header("In Level Panel")]
    [SerializeField] private InPlayLevelUI InLevelUiPanel;
    [SerializeField] private GuidePanel GuidePanel;
    


    [Header("Level Select Panel")]
    [SerializeField] private LevelSelectUI LevelSelectPanel;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    //===Xử lý hiển thị ========
    public void ShowInPlayLevelUI()
    {
        Close();
        InLevelUiPanel.Show();
    }

    public void SetMoveCountUI(int moveCount, int moveCountLimit, int[] moveToGetStar)
    {
        InLevelUiPanel.SetMoveCount(moveCount, moveCountLimit);
        InLevelUiPanel.SetStarMarks(moveToGetStar, moveCountLimit);
    }

    public void DisplayGuide(List<GuideDisplayInfo> guideTypes)
    {
        GuidePanel.ShowGuide(guideTypes);
    }
    
   
    //===========================================
    public void ShowGameOverPanel()
    {
        Time.timeScale = 0;
        levelFailedPanel.Show();
        BlurPanel.SetActive(true);
    }


    //====== Xử lý hiển thị win panel========
    public void ShowLevelCompletePanel(int stars, int moveCount)
    {
        Time.timeScale = 0;
        BlurPanel.SetActive(true);
        levelCompletePanel.ShowPanel(stars, moveCount);
    }


    //====== Xử lý hiển thị pause panel========
    public void ShowPausePanel()
    {
        Time.timeScale = 0;
        PausePanel.Show();
        BlurPanel.SetActive(true);
    }



    //====== Xử lý các button============
    public void OnClickResume()
    {
        Time.timeScale = 1;
        PausePanel.Hide();
        SettingPanel.Hide();
        BlurPanel.SetActive(false);
    }

    public void OnClickSetting()
    {
        Time.timeScale = 0;
        SettingPanel.Show();
        BlurPanel.SetActive(true);
    }

    public void OnClickHome()
    {
        Time.timeScale = 1;
        Close();
        HomePanel.SetActive(true);
    }

    public void OnClickPlay()
    {
        Close();
        LevelSelectPanel.Show();
    }
   
    public void OnClickBackToLevelSelect()
    {
        GameManager.Instance.UnloadLevel();
        Close();
        LevelSelectPanel.Show();
    }

    public async void OnClickRestart()
    {
        Close();
        await Task.Delay(100);
        GameManager.Instance.ReloadLevel();
    }

    public void OnClickNextLevel()
    {
        Close();
        GameManager.Instance.LoadNextLevel();
    }


    private void Close()
    {
        Time.timeScale = 1;

        levelCompletePanel.Hide();
        levelFailedPanel.Hide();
        InLevelUiPanel.Hide();
        PausePanel.Hide();
        SettingPanel.Hide();
        LevelSelectPanel.Hide();

        BlurPanel.SetActive(false); 
        HomePanel.SetActive(false);
    }
}
