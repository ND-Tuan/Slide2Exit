using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using ObserverPattern;
using UnityEngine;

public class Navigator : MonoBehaviour, IResetLevel
{
    [Range(0, 3)]
    [SerializeField] private int NavigatorID;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Diraction diraction;
    private Diraction originalDiraction;


    private void Awake()
    {
        Observer.AddListener(EvenID.ChangeDiraction, SetDiraction);
        originalDiraction = diraction;
    }


    //Đổi hướng người chơi
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = transform.position;
            Player player = other.GetComponent<Player>();
            Vector2 playerDirection = new();

            switch (diraction)
            {
                case Diraction.Up:
                    playerDirection = Vector2.up;
                    break;
                case Diraction.Down:
                    playerDirection = Vector2.down;
                    break;
                case Diraction.Left:
                    playerDirection = Vector2.left;
                    break;
                case Diraction.Right:
                    playerDirection = Vector2.right;
                    break;
            }

            player.ChangeDirection(playerDirection);

            Observer.PostEvent(EvenID.ReportTaskProgress, new object[] { TaskType.UseNavigator, 1, true });

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
        if ((int)data[0] == NavigatorID)
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
    
    private void OnDestroy()
    {
        Observer.RemoveListener(EvenID.ChangeDiraction, SetDiraction);
    }
}

public enum Diraction
{
    Up,
    Down,
    Left,
    Right
}
