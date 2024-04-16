using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private float speed; //[SerializeField] чтобы появился ползунок в unity
    private Rigidbody2D body; // чтобы управлять слаймом
    private Animator animator; // доступ к аниматору
    private bool grounded;
    
    private void Awake() // Вызывается каждый раз когда начинается игра
    {
        body = GetComponent<Rigidbody2D>(); // Получить доступ к слайму - проверить player на наличие rigidbody2d и будет хранить внутри body
        // условно говоря получение ссылки
        animator = GetComponent<Animator>();
    } 

    private void Update()
    {
        // выполняется на каждом фрейме
        var horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y); //как быстро и в каком направлении движется слайм
        //Input.GetAxis("Horizontal") отслеживает A D и сдвигается на 1 px
        if (horizontalInput > 0.01f) // двигается вправо
            transform.localScale = Vector3.one; // чтоб лицом поворачивался в нужную сторону
        if (horizontalInput < -0.01f) // двигается вправо
            transform.localScale = new Vector3(-1, 1, 1); // чтоб лицом поворачивался в нужную сторону 
        if (Input.GetKey(KeyCode.Space) && grounded) // прыгать можно только когда нажат пробел и челик на земле
            Jump();
        
        // Set Animations
        animator.SetBool("run", horizontalInput != 0);
        animator.SetBool("grounded", grounded);
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, speed);
        // с тригером анимация правильная
        animator.SetTrigger("jump");
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision) // Встроенный метод
    {
        if (collision.gameObject.tag == "ground")
            grounded = true;
    }
}
