using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GuidePanel : BasePanelController
{
    [Header("Main Component")]
    [SerializeField] private CanvasGroup GuideFocus;
    [SerializeField] private GameObject Hand;
    [SerializeField] private TextMeshProUGUI GuideNameText;
    [SerializeField] private TextMeshProUGUI GuideDescriptionText;
    [SerializeField] private TextMeshProUGUI ButtonText;

    private Dictionary<GuideType, GuideSO> guideSO = new();
    private List<GuideDisplayInfo> guideNeedToDisplayList = new();


    private void Awake()
    {
        foreach (GuideSO guide in Resources.LoadAll<GuideSO>("GuideData"))
        {
            guideSO.Add(guide.GuideType, guide);
        }

        gameObject.SetActive(false);
    }

    public void ShowGuide(List<GuideDisplayInfo> guideTypes)
    {
        guideNeedToDisplayList = guideTypes;
        PanelSetUp(guideNeedToDisplayList[0]);
        
    }

    public void PanelSetUp(GuideDisplayInfo guideType)
    {
        if(!guideSO.ContainsKey(guideType.GuideType)) return;

        Vector2 screenPos = Camera.main.WorldToScreenPoint(guideType.transform.position);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent.GetComponent<RectTransform>(),
            screenPos,
            Camera.main,
            out Vector2 localPoint
        );
        
        RectTransform rectTransform = GetComponent<RectTransform>();

        if(localPoint.x >0) {

            rectTransform.pivot = new Vector2(1.1f, 1.15f);
            Hand.transform.localScale = new Vector3(-1, 1, 1);
            Hand.GetComponent<RectTransform>().anchoredPosition = new Vector3(300, 200, 0);
            
        } else {

            rectTransform.pivot = new Vector2(-0.1f, 1.15f);
            Hand.transform.localScale = new Vector3(1, 1, 1);
            Hand.GetComponent<RectTransform>().anchoredPosition = new Vector3(-300, 200, 0);
            
        }

        ButtonText.text = guideNeedToDisplayList.Count > 1 ? "NEXT" : "CLOSE";

        GuideNameText.text = guideSO[guideType.GuideType].GuideName;
        GuideDescriptionText.text = guideSO[guideType.GuideType].GuideDescription;

        rectTransform.anchoredPosition = localPoint;
        guideNeedToDisplayList.Remove(guideType);

        Popup(AnimationTimeIn);

        GuideFocus.transform.position = guideType.transform.position + guideType.OffsetPos;
        GuideFocus.gameObject.SetActive(true);
        DisplayFocus();

    }

    public void NextGuide()
    {
        if(guideNeedToDisplayList.Count > 0){
            PanelSetUp(guideNeedToDisplayList[0]);
        } else {
            PopOut(AnimationTimeOut);
            HideFocus();
            Time.timeScale = 1;
        }
    }

    private void DisplayFocus(){

        GuideFocus.alpha = 0;
        GuideFocus.transform.localScale = Vector3.one * 3;
        Sequence seq = DOTween.Sequence();
        seq.SetUpdate(true); 
        seq.Append(GuideFocus.DOFade(1, AnimationTimeIn*0.75f).SetUpdate(true));
        seq.Join(GuideFocus.transform.DOScale(1, AnimationTimeIn*1.25f).SetEase(Ease.OutBack));
    }

    public void HideFocus()
    {
        Sequence seq = DOTween.Sequence();
        seq.SetUpdate(true); 
        seq.Append(GuideFocus.DOFade(0, AnimationTimeOut));
        seq.Join(GuideFocus.transform.DOScale(0, AnimationTimeOut).SetEase(Ease.InBack));
        seq.OnComplete(() => GuideFocus.gameObject.SetActive(false));
    }
}



[System.Serializable]
public class GuideDisplayInfo
{
    public GuideType GuideType;
    public Transform transform;
    public Vector3 OffsetPos;
}
