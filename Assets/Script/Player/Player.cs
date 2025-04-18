using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 10.0f;
    public bool IsRunning = false;
    [SerializeField] private GameObject Interface;
    private Animator animator;
    private Vector2 startTouchPosition, endTouchPosition;
    private Vector2 moveDirection;
    [SerializeField] private ParticleSystem particle;

    private ParticleSystem.MainModule mainModule;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = Interface.GetComponent<Animator>();
        mainModule = particle.main;
    }

    void Update()
    {
        if (Input.touchCount > 0 && !IsRunning)
        {
            IsRunning = true;
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endTouchPosition = touch.position;
                HandleSwipe();
            }
        }

        if(rb.velocity == Vector2.zero && IsRunning){
            IsRunning = false;
            
            //Khớp lại vị trí
            Vector2 pos = transform.position;
            pos.x = Mathf.Round(pos.x);
            pos.y = Mathf.Round(pos.y);
            transform.position = pos;
        } 

        animator.SetBool("IsMoving", IsRunning);
        mainModule.startColor = IsRunning ? Color.white : new Color(1, 1, 1, 0);

    }

    void HandleSwipe()
    {
        Vector2 swipeDirection = endTouchPosition - startTouchPosition;

        if (swipeDirection.magnitude < 20)
            return; //bỏ qua nếu độ dài lướt quá ngắn

        if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
            Move(swipeDirection.x > 0 ? Vector2.right : Vector2.left); // Horizontal swipe
        else
            Move(swipeDirection.y > 0 ? Vector2.up : Vector2.down); // Vertical swipe
    }

    public async void Move(Vector2 direction)
    {
        moveDirection = direction.normalized;
        rb.AddForce(direction * speed, ForceMode2D.Impulse);

        await Task.Delay(100); //đợi physic update

        if(rb.velocity == Vector2.zero) return;

        IsRunning = true;
        animator.SetBool("IsMoving", true);
        GameManager.Instance.MinusMoveCount();
    }

    // hàm này sẽ được gọi khi nhân vật va chạm với các đối tượng khác
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Deformation(moveDirection);
    }

    private void Deformation(Vector2 direction)
    {
        Vector3 deformation = new Vector3(
            Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ? 0.8f : 1.2f, 
            Mathf.Abs(direction.y) > Mathf.Abs(direction.x) ? 0.8f : 1.2f, 
            1f
        );

        // mô phỏng biến dạng
        Interface.transform.DOScale(deformation, 0.2f).OnComplete(() =>
        {
            // Trả về kích thước ban đầu
            Interface.transform.DOScale(Vector3.one, 0.2f);
        });
    }

    public void Win(){
        mainModule.startColor = new Color(1, 1, 1, 0);
        rb.velocity = Vector2.zero;
    }

    private void OnDestroy()
    {
        DOTween.Kill(Interface.transform);
    }

}
