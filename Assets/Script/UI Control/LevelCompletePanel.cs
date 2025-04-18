using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using DG.Tweening;

public class LevelCompletePanel : BasePanelController
{    
    [Header("Main Components")]
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private GameObject[] Stars;
    [SerializeField] private TextMeshProUGUI RewardText;


    public async void ShowPanel(int star, int moverCount)
    {
        Time.timeScale = 0;
        //Tắt hết các sao
        for (int i = 0; i < Stars.Length; i++)
        {
            Stars[i].SetActive(false);
        }

        Popup(AnimationTimeIn);
        ShowRewardAnimated(moverCount);
        await Task.Delay((int)(AnimationTimeIn * 900));

        LevelID levelID = GameManager.Instance.CurrentLevel;
        LevelText.text = "Level " + levelID.Stage + "-" + levelID.Index;

        //Hiện reward text
       
        for (int i = 0; i < Stars.Length; i++)
        {
            Stars[i].SetActive(i < star);
            await Task.Delay(500);
        }
    }

    public override void Hide()
    {
        PopOut(AnimationTimeOut);
    }

    private void ShowRewardAnimated(int moveCount){
        int finalReward = moveCount * 100;
        int current = 0;

        DOTween.To(() => current, x => {
            current = x;
            RewardText.text = "+" + current;

        }, finalReward, 1f).SetEase(Ease.Linear).SetUpdate(true);
    }
}
