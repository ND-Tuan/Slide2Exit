using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObserverPattern;

public class FragileObstacle : MonoBehaviour, IResetLevel
{
    [SerializeField] private ParticleSystem Effect;
    private Animator animator;
    private Collider2D obstacleCollider;
    private Sprite sprite;
    private bool isBroken = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        obstacleCollider = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>().sprite;
    }

    private void  OnCollisionEnter2D(Collision2D other)
    {
        if(isBroken) return;
        if (other.gameObject.CompareTag("Player"))
        {
            isBroken = true;
            animator.enabled = true;
            obstacleCollider.enabled = false;

            Observer.PostEvent(EvenID.ReportTaskProgress, new object[] { TaskType.UseFragileObstacle, 1, true});
            Invoke("Disapear", 0.1f);

        }
    }

    private void Disapear()
    {
        Effect.gameObject.SetActive(true);
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void ResetLevel()
    {
        isBroken = false;
        animator.enabled = false;
        Effect.gameObject.SetActive(false);
        obstacleCollider.enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<SpriteRenderer>().sprite = sprite;

    }

    
}
