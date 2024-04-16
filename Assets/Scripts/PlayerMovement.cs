using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private float speed; //[SerializeField] чтобы появился ползунок в unity
    private Rigidbody2D body; // чтобы управлять слаймом

    private void Awake() // Вызывается каждый раз когда начинается игра
    {
        body = GetComponent<Rigidbody2D>(); // Получить доступ к слайму - проверить player на наличие rigidbody2d и будет хранить внутри body
        // условно говоря получение ссылки
    } 

    private void Update()
    {

        // выполняется на каждом фрейме
        body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y); //как быстро и в каком направлении движется слайм
        //Input.GetAxis("Horizontal") отслеживает A D и сдвигается на 1 px
        if (Input.GetKey(KeyCode.Space))
            body.velocity = new Vector2(body.velocity.x, speed);
    }
}
