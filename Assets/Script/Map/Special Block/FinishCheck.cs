using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FinishCheck : MonoBehaviour
{
    [SerializeField] private ParticleSystem WinEffect;


    private void Start()
    {
        ParticleSystem.MainModule mainModule = WinEffect.main;
        Sprite PlayerSkin = GameManager.Instance.PlayerDataManager.GetEquippedSkin().SkinImage;
        Color32 mainColor = ColorAnalyzer.MainColorFromTexture(PlayerSkin.texture);
        mainModule.startColor = new ParticleSystem.MinMaxGradient(mainColor);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().Stop();
            collision.gameObject.transform.position = transform.position;

            WinEffect.gameObject.SetActive(true);
            WinEffect.Play();
            collision.gameObject.transform.DOScale(0, 0.1f).SetEase(Ease.InBack);

            Invoke(nameof(Win), 0.6f);
        }
    }


    private void Win(){
        GameManager.Instance.GameWin();
    }
    
}
