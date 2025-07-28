using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ObserverPattern;

public class Lock : MonoBehaviour, IResetLevel
{
    [Range(0, 2)]
    [SerializeField] private int lockID;
    [SerializeField] private ParticleSystem effect;
    private ParticleSystem.MainModule mainModule;
    private Collider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        Observer.AddListener(EvenID.UnlockBlock, UnlockBlock);
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<Collider2D>();
        mainModule = effect.main;
        mainModule.startColor = spriteRenderer.color;
    }

    private void UnlockBlock(object[] data)
    {
        if ((int)data[0] == lockID)
        {
           
            boxCollider2D.enabled = false;
            Invoke("Disapear", 0.1f);
        }
    }

    private void Disapear()
    {
        effect.gameObject.SetActive(true);
        spriteRenderer.enabled = false;
    }

    public void ResetLevel()
    {
        
        effect.gameObject.SetActive(false);
        boxCollider2D.enabled = true;
        spriteRenderer.enabled = true;
    }

    void OnValidate()
    {
        ColorChanger.ChangeColor(GetComponent<SpriteRenderer>(), lockID);
    }

    private void OnDestroy()
    {
        Observer.RemoveListener(EvenID.UnlockBlock, UnlockBlock);
    }
}

