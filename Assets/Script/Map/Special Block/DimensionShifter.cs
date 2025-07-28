using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine.UI;

public class DimensionShifter : MonoBehaviour, IResetLevel
{
    [SerializeField] private GameObject Board1;
    [SerializeField] private GameObject Board2;

    [SerializeField] private GameObject RotateObject;
    [SerializeField] private GameObject Wormhole;
    [SerializeField] private SpriteRenderer FakePlayerInterface;
    [SerializeField] private GameObject VisualEffect;

    [SerializeField] private float RotationSpeed = 100f;
    [SerializeField] private float timeIn;
    [SerializeField] private float timeOut;
    

    private void Start()
    {
        FakePlayerInterface.sprite = GameManager.Instance.PlayerDataManager.GetEquippedSkin().SkinImage;
    }

    void Update()
    {
        RotateObject.transform.Rotate(Vector3.forward * Time.unscaledDeltaTime * RotationSpeed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ShowEffect();
            //other.gameObject.GetComponent<Player>().Stop();
            other.transform.position = this.transform.position;
        }
    }


    private void ShowEffect()
    {
        Time.timeScale = 0f;
        VisualEffect.SetActive(true);
        Wormhole.SetActive(true);
        FakePlayerInterface.gameObject.SetActive(true);

        RotationSpeed+= 200f;

        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);

        //Hiệu ứng phóng to các thứ
        Sequence seq = DOTween.Sequence();
        seq.SetUpdate(true); 
        seq.Append(Camera.main.transform.DOLocalMove(targetPos, timeIn).SetEase(Ease.OutBack));
        seq.Join(Wormhole.transform.DOScale(30, timeIn).SetEase(Ease.OutBack));
        seq.Join(VisualEffect.transform.DOScale(6, timeIn).SetEase(Ease.OutBack));
        seq.Join(VisualEffect.GetComponent<SpriteRenderer>().DOFade(0.5F, timeIn).SetEase(Ease.OutBack));
        
        seq.OnComplete(() => {
            HideEffect();
            SwitchBoard();
        });

    }

    private async void HideEffect()
    {
        await Task.Delay(100);
        Vector3 targetPos = new Vector3(0, 0, Camera.main.transform.position.z);
        RotationSpeed -= 200f;

        //Thu nhỏ về ban đầu
        Sequence seq = DOTween.Sequence();
        seq.SetUpdate(true); 
        seq.Append(Camera.main.transform.DOLocalMove(targetPos, timeOut).SetEase(Ease.InBack));
        seq.Join(Wormhole.transform.DOScale(0, timeOut).SetEase(Ease.OutBack));
        seq.Join(VisualEffect.transform.DOScale(0, timeOut).SetEase(Ease.OutBack));
        seq.Join(VisualEffect.GetComponent<SpriteRenderer>().DOFade(0F, timeOut).SetEase(Ease.InBack));
        seq.OnComplete(() => {
            Time.timeScale = 1f;
            VisualEffect.SetActive(false);
            Wormhole.SetActive(false);
            FakePlayerInterface.gameObject.SetActive(false);
        });
    }

    private void SwitchBoard()
    {
        if(Board1.transform.position == Vector3.zero)
        {
            Board1.transform.position = Vector3.one * 1000;
            Board2.transform.position = Vector3.zero;
        } else {
            Board1.transform.position = Vector3.zero;
            Board2.transform.position = Vector3.one * 1000;
        }
    }

    public void ResetLevel()
    {
        Board1.transform.position = Vector3.zero;
        Board2.transform.position = Vector3.one * 1000;

        RotationSpeed = 100f;
        VisualEffect.transform.localScale = Vector3.zero;
        Wormhole.transform.localScale = Vector3.zero;
        Color color = VisualEffect.GetComponent<SpriteRenderer>().color;
        color.a = 0f;
        VisualEffect.GetComponent<SpriteRenderer>().color = color;

        VisualEffect.SetActive(false);
        Wormhole.SetActive(false);
        FakePlayerInterface.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
