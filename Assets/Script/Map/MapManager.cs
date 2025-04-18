using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private int moveLimit;

    [SerializeField] private int MoveToGetStar1;
    [SerializeField] private int MoveToGetStar2;
    [SerializeField] private int MoveToGetStar3;

    [SerializeField] private List<GuideDisplayInfo> guideNeedToDisplayList = new();
    

    void Start()
    {
        GameManager.Instance.SetUpMap(moveLimit, new int[] { MoveToGetStar1, MoveToGetStar2, MoveToGetStar3 });

        if(guideNeedToDisplayList.Count > 0)
        {
            StartCoroutine(DisplayGuide());
        }
    }
    
    private IEnumerator DisplayGuide()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(0.5f);
        UIController.Instance.DisplayGuide(guideNeedToDisplayList);
    }
}


