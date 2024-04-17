using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private float speed; //[SerializeField] чтобы появился ползунок в unity
    [SerializeField]private float jumpPower; 
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body; // чтобы управлять слаймом
    private Animator animator; // доступ к аниматору
    private BoxCollider2D BoxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;
    
    private void Awake() // Вызывается каждый раз когда начинается игра
    {
        body = GetComponent<Rigidbody2D>(); // Получить доступ к слайму - проверить player на наличие rigidbody2d и будет хранить внутри body
        // условно говоря получение ссылки
        animator = GetComponent<Animator>();
        BoxCollider = GetComponent<BoxCollider2D>();
    } 

    private void Update()
    {
        // выполняется на каждом фрейме
        horizontalInput = Input.GetAxis("Horizontal");
        
        //Input.GetAxis("Horizontal") отслеживает A D и сдвигается на 1 px
        if (horizontalInput > 0.01f) // двигается вправо
            transform.localScale = Vector3.one; // чтоб лицом поворачивался в нужную сторону
        if (horizontalInput < -0.01f) // двигается вправо
            transform.localScale = new Vector3(-1, 1, 1); // чтоб лицом поворачивался в нужную сторону 
        
        print(OnWall());
        if (wallJumpCooldown > 0.2f)
        {
            
            body.velocity =
                new Vector2(horizontalInput * speed, body.velocity.y); //как быстро и в каком направлении движется слайм
            if (OnWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 7;
            if (Input.GetKey(KeyCode.Space)) // прыгать можно только когда нажат пробел и челик на земле
                Jump();
        }
        else
            wallJumpCooldown += Time.deltaTime;
        
        
        // Set Animations
        animator.SetBool("run", horizontalInput != 0);
        animator.SetBool("grounded", isGrounded());
    }

    private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            // с тригером анимация правильная
            animator.SetTrigger("jump");
        }
        else if (OnWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y,
                    transform.localScale.z);
            }
            else 
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
            wallJumpCooldown = 0;
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // Встроенный метод
    {
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit =
            Physics2D.BoxCast(BoxCollider.bounds.center, BoxCollider.bounds.size,  0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    
    private bool OnWall()
    {
        RaycastHit2D raycastHit =
            Physics2D.BoxCast(BoxCollider.bounds.center, BoxCollider.bounds.size,  0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
}
