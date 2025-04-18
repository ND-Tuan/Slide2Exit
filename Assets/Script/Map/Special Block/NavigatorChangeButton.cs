using System.Collections;
using System.Collections.Generic;
using ObserverPattern;
using UnityEngine;

public class NavigatorChangeButton : MonoBehaviour, IResetLevel
{
    [SerializeField] private Diraction diraction;
    [SerializeField] private int NavigatorID;
    [SerializeField] private Sprite PressDown;

    private bool isPress = false;
    private SpriteRenderer spriteRenderer;
    private Sprite PressUp;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        PressUp = spriteRenderer.sprite;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isPress)
        {
            Observer.PostEvent(EvenID.ChangeDiraction, new object[] {NavigatorID, diraction});
            spriteRenderer.sprite = PressDown;
            isPress = true;
        }
    }

    public void ResetLevel()
    {
        isPress = false;
        spriteRenderer.sprite = PressUp;
    }
  
}
