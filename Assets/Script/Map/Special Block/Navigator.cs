using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using ObserverPattern;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    [Range(0, 3)]
    [SerializeField] private int NavigatorID;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Diraction diraction;
    private Diraction originalDiraction => diraction;


    private void Awake()
    {
        Observer.AddListener(EvenID.ChangeDiraction, SetDiraction);
    }


    //Đổi hướng người chơi
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = transform.position;
            Player player = other.GetComponent<Player>();

            switch (diraction)
            {
                case Diraction.Up:
                    player.rb.velocity = Vector2.up * player.speed;
                    break;
                case Diraction.Down:
                    player.rb.velocity = Vector2.down * player.speed;
                    break;
                case Diraction.Left:
                    player.rb.velocity = Vector2.left * player.speed;
                    break;
                case Diraction.Right:
                    player.rb.velocity = Vector2.right * player.speed;
                    break;
            }
            
        }
    }


    //Đổi hướng của Navigator
    private void ChangeDiraction()
    {
        switch (diraction)
        {
            case Diraction.Up:
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            case Diraction.Down:
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180);
                break;
            case Diraction.Left:
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
                break;
            case Diraction.Right:
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, -90);
                break;
        }
    }

    public void SetDiraction(object[] data)
    {
        if((int)data[0] == NavigatorID)
        {
            diraction = (Diraction)data[1];
            ChangeDiraction();
        }
    }

    public void ResetLevel()
    {
        diraction = originalDiraction;
        ChangeDiraction();
    }


    void OnValidate()
    {
        ChangeDiraction();
        ColorChanger.ChangeColor(spriteRenderer, NavigatorID);
    }
}

public enum Diraction
{
    Up,
    Down,
    Left,
    Right
}
