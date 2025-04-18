using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstaclePopup : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D boxCollider;

    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Invoke(nameof(PopupWall), 0.2f);
        }
    }

    private void PopupWall()
    {
        animator.enabled = true; 
        
        boxCollider.size = Vector2.one;
        boxCollider.isTrigger = false;
    }
    
}
