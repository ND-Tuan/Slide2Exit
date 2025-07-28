using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ObserverPattern;

public class ObstaclePopup : MonoBehaviour, IResetLevel
{
    private Animator animator;
    private BoxCollider2D boxCollider;
    private Sprite sprite;

    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>().sprite;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Observer.PostEvent(EvenID.ReportTaskProgress, new object[] { TaskType.UseObstacleMaker, 1, true});
            Invoke(nameof(PopupWall), 0.2f);
        }
    }

    private void PopupWall()
    {
        //chạy animation
        animator.enabled = true; 
        animator.Play("Obstacle_Popup", 0, 0f); 
        
        //Điều chỉnh lại collider
        boxCollider.size = Vector2.one;
        boxCollider.isTrigger = false;
    }

    public void ResetLevel()
    {
        //Vô hiệu hóa animation
        animator.enabled = false; 
        
        //Đặt lại collider
        boxCollider.size = Vector2.one * 0.5f;
        boxCollider.isTrigger = true;
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
