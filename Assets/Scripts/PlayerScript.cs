using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    //para no traerlos con el getcomponente, los instancio y los declaro aquí
    SpriteRenderer sprite;
    Rigidbody2D rigidBody;
    BoxCollider2D boxCollider;
    public LayerMask layerMask; //público para inicializarlo por fuera, en el IDE de Unity en el componente del script
    Animator animator;
    Boolean doubleJump, hit, control;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>(); //para hacer un método de saber si estamos pegados o no al suelo
        animator = GetComponent<Animator>();

        doubleJump = true;
        hit = false;
        control = true; //el usuario puede usar el control?
    }

    void Update()
    {
        //
        // Control
        // idle 0
        // run 1
        // jump 2
        // fall 3
        // doublejump 4
        // hit 5
        //

        if (control)
        {
            //getkey permite mantenerlo picado, getkeydown es para picarle varias veces
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                sprite.flipX = true; //para que volteé
                if (Input.GetKey(KeyCode.LeftShift)) //si presiona también el shift, corre
                {
                    rigidBody.linearVelocity = new Vector2(-10f, rigidBody.linearVelocity.y);
                    animator.speed = 2; //usar atributo speed
                }
                else
                {
                    rigidBody.linearVelocity = new Vector2(-5f, rigidBody.linearVelocity.y);
                    animator.speed = 1;
                }
            }

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                sprite.flipX = false;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    rigidBody.linearVelocity = new Vector2(10f, rigidBody.linearVelocity.y);
                    animator.speed = 2;
                }
                else
                {
                    rigidBody.linearVelocity = new Vector2(5f, rigidBody.linearVelocity.y);
                    animator.speed = 1;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGrounded())
                {
                    doubleJump = true;
                    rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, 17.5f);
                }
                else if (doubleJump)
                {
                    doubleJump = false;
                    rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, 17.5f);
                }
            }
        }


        //
        // Animator
        //

        if (!hit) //si no le están pegando, puede hacer cualquier animación
        {
            //estado de reposo
            //está en el piso y no está presinando ni izq ni derecha
            if (isGrounded() && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                animator.SetInteger("state", 0);
            }
            //estado de caminar
            //reposo y izq o derecha
            else if (isGrounded() && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
            {
                animator.SetInteger("state", 1);
            }
            else if (rigidBody.linearVelocity.y > 0)
            {
                if (doubleJump) animator.SetInteger("state", 2);
                else animator.SetInteger("state", 4);
            }
            else if (rigidBody.linearVelocity.y < 0)
            {
                animator.SetInteger("state", 3);
            }
        }
    }

    //método convencional para saber si el player está sobre el piso
    public bool isGrounded()
    {
        //método de clase physics2d llamado boxcast, pide el boxcollider del jugador, saca el centro, los límites o sea el tamaño, ángulo, dirección y cuánto quiero aplicar, layer
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, layerMask);
        // entonces evalua lo inferior en un margen de 0.1 respecto abajo, está tocando un objeto en la capa terrain?
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Limit")
        {
            animator.SetTrigger("disappear");
        }

        if (collision.gameObject.tag == "Fire")
        {
            control = false;
            hit = true;

            animator.SetInteger("state", 5);

            if (sprite.flipX) //izquierda
            {
                rigidBody.linearVelocity = new Vector2(10f, 4f);
            }
            else
            {
                rigidBody.linearVelocity = new Vector2(-10f, 4f);
            }
        }
    }

    public void goToDisappearing()
    {
        SceneManager.LoadScene(0);
    }

    public void goToIdle()
    {
        control = true;
        hit = false;

        animator.SetInteger("state", 0);
    }
}
