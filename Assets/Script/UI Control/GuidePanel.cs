using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.UIElements;

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

        //Set position
        AdjustPanelPosition(guideType.transform);

        //set nội dung hiện thị
        //còn hướng dẫn thì hiển thí nút next
        ButtonText.text = guideNeedToDisplayList.Count > 1 ? "NEXT" : "CLOSE";

        //tên và mô tả
        GuideNameText.text = guideSO[guideType.GuideType].GuideName;
        GuideDescriptionText.text = guideSO[guideType.GuideType].GuideDescription;

        //loại bỏ hướng dẫn đã hiện thị
        guideNeedToDisplayList.Remove(guideType);

        Popup(AnimationTimeIn);

        //hiện thị focus
        GuideFocus.transform.position = guideType.transform.position + guideType.OffsetPos;
        GuideFocus.gameObject.SetActive(true);
        DisplayFocus();
    }

    //Điều chỉnh vị trí của panel
    private void AdjustPanelPosition(Transform transform)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        //Tính toán vị trí mới của panel dựa trên vị trí của đối tượng
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent.GetComponent<RectTransform>(),
            Camera.main.WorldToScreenPoint(transform.position),
            Camera.main,
            out Vector2 localPoint
        );

        //Thay đổi pivot cũng như hướng trái phải panel theo vị trí của đối tượng
        if(localPoint.x >0) {
            rectTransform.pivot = new Vector2(1.1f, 1.15f);
            Hand.transform.localScale = new Vector3(-1, 1, 1);
            Hand.GetComponent<RectTransform>().anchoredPosition = new Vector3(300, 200, 0);
            
        } else {
            rectTransform.pivot = new Vector2(-0.1f, 1.15f);
            Hand.transform.localScale = new Vector3(1, 1, 1);
            Hand.GetComponent<RectTransform>().anchoredPosition = new Vector3(-300, 200, 0);
        }

        //Thay đổi vị trí của panel
        rectTransform.anchoredPosition = localPoint;
    }

    //xử lý logic nút 
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

    //xử lý hiển thị panel làm nổi bật 
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
