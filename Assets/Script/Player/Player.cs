using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using ObserverPattern;

public class Player : MonoBehaviour, IResetLevel
{
    public bool IsRunning = false;

    [Header("Main Components")]
    [SerializeField] private GameObject Interface;
    [SerializeField] private ParticleSystem particle;

    [Header("Player Settings")]
    [SerializeField] private float speed = 10.0f;

    [Header("Audio Settings")]
    [SerializeField] private FxAudioDataSO CollisionFx;


    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 startTouchPosition, endTouchPosition;
    private Vector2 moveDirection;
    private ParticleSystem[] Module;
    private Vector2 originalPosition;


    void Awake()
    {
        Interface.SetActive(false);
        Interface = null;

        GameObject currentSkin = PoolManager.Instance.Get(GameManager.Instance.PlayerDataManager.GetEquippedSkin().SkinPrefab);
        currentSkin.transform.SetParent(this.transform);

        Interface = currentSkin;
        Interface.transform.localPosition = Vector3.zero;
        Interface.transform.localScale = Vector3.one;

        particle = Interface.GetComponentInChildren<ParticleSystem>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = Interface.GetComponent<Animator>();
        Module = particle.gameObject.GetComponentsInChildren<ParticleSystem>();

        // Save the initial position
        originalPosition = transform.position;
    }


    void Update()
    {
        // Handle swipe input
        if (Input.touchCount > 0 && !IsRunning)
        {
            IsRunning = true;
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position; // Save start position
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endTouchPosition = touch.position; // Save end position
                HandleSwipe(); // Handle swipe
            }
        }

        // Stop moving when velocity is zero
        if (rb.velocity == Vector2.zero && IsRunning)
        {
            IsRunning = false;

            // Snap to grid position
            Vector2 pos = transform.position;
            pos.x = Mathf.Round(pos.x);
            pos.y = Mathf.Round(pos.y);
            transform.position = pos;
        }

        // Update animation state and effects
        if (animator != null) animator.SetBool("IsMoving", IsRunning);
        SetActiveMainModule(IsRunning);
    }


    // Handle swipe input
    void HandleSwipe()
    {
        Vector2 swipeDirection = endTouchPosition - startTouchPosition;

        // Ignore if swipe is too short
        if (swipeDirection.magnitude < 20)
            return;

        // Determine swipe direction and move
        if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
            Move(swipeDirection.x > 0 ? Vector2.right : Vector2.left); // Horizontal swipe
        else
            Move(swipeDirection.y > 0 ? Vector2.up : Vector2.down); // Vertical swipe
    }


    // Move the player
    private async void Move(Vector2 direction)
    {
        moveDirection = direction.normalized;
        rb.AddForce(direction * speed, ForceMode2D.Impulse);

        await Task.Delay(100); // Wait for physics update

        if (rb == null) return;
        // If still moving, update state
        if (rb.velocity == Vector2.zero) return;

        IsRunning = true;
        if (animator != null) animator.SetBool("IsMoving", true);
        GameManager.Instance.LevelManager.MinusMoveCount();
    }


    // Handle collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Deformation(moveDirection);
        Observer.PostEvent(EvenID.PlayFX, CollisionFx); // Play collision sound
    }

    // Deform on collision
    private void Deformation(Vector2 direction)
    {
        // Calculate deformation scale
        Vector3 deformation = new Vector3(
            Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ? 0.8f : 1.2f,
            Mathf.Abs(direction.y) > Mathf.Abs(direction.x) ? 0.8f : 1.2f,
            1f
        );

        // Apply deformation and return to original scale
        Interface.transform.DOScale(deformation, 0.2f).OnComplete(() =>
        {
            Interface.transform.DOScale(Vector3.one, 0.2f);
        });
    }


    // Change movement direction
    public void ChangeDirection(Vector2 direction)
    {
        rb.velocity = direction * speed;
    }


    // Stop the player
    public void Stop()
    {
        SetActiveMainModule(false);
        rb.velocity = Vector2.zero;
    }


    // Reset level state
    public void ResetLevel()
    {
        transform.localScale = Vector3.one;
        rb.velocity = Vector2.zero;
        transform.position = originalPosition;
        IsRunning = false;
        if (animator != null) animator.SetBool("IsMoving", false);
        SetActiveMainModule(false);
    }

    // Cleanup when object is destroyed
    private void OnDestroy()
    {
        DOTween.Kill(Interface.transform); // Kill tweens related to Interface
        Interface.transform.SetParent(null); // Detach Interface
        DontDestroyOnLoad(Interface); // Prevent Interface from being destroyed on scene load

    }
    
    private void SetActiveMainModule(bool active)
    {
        foreach (ParticleSystem module in Module)
        {
            var main = module.main;
            main.startColor = main.startColor.color.WithAlpha(active ? 1f : 0f);
        }
    }
}