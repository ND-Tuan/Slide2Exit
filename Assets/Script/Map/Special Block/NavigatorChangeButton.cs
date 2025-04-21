using System.Collections;
using System.Collections.Generic;
using ObserverPattern;
using UnityEngine;

public class NavigatorChangeButton : MonoBehaviour, IResetLevel
{
    [SerializeField] private Diraction diraction;
    [Range(0, 3)]
    [SerializeField] private int NavigatorID;
    [SerializeField] private Sprite PressDown;

    private bool isPress = false;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Sprite PressUp;


    private void Awake()
    {
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

    private void OnValidate()
    {
        ColorChanger.ChangeColor(spriteRenderer, NavigatorID);
    }
  
}
