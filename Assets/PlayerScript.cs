using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public TextMeshProUGUI textManzanas;
    int num_manzanas;
    public TextMeshProUGUI textTimer;
    float timer;
    int level_timer;
    //para no traerlos con el getcomponente, los instancio y los declaro aquí
    SpriteRenderer sprite;
    Rigidbody2D rigidbody;
    BoxCollider2D boxCollider;
    public LayerMask layerMask; //público para inicializarlo por fuera, en el IDE de Unity en el componente del script
    Animator animator;
    bool double_jump;
    bool hit;
    bool control;
    bool water;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>(); //para hacer un método de saber si estamos pegados o no al suelo
        animator = GetComponent<Animator>();

        double_jump = false;
        hit = false;
        control = true; //el usuario puede usar el control?
        water = false;
        num_manzanas = 0;

        timer = Time.time; // para obtener tiempo del reloj del juego
        level_timer = 100;
        textTimer.SetText("" + level_timer);
    }

    // Update is called once per frame
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

        //para que cada segundo se vaya aumentando, mientras el reloj del juego sea mayor
        if (Time.time > timer)
        {
            level_timer--;
            textTimer.SetText("" + level_timer);

            if (level_timer == 0)
            {
                SceneManager.LoadScene(0);
            }

            timer++;
        }

        if (control)
        {
            //Control
            //getkey permite mantenerlo picado, getkeydown es para picarle varias veces
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                sprite.flipX = true; //para que volteé

                if (Input.GetKey(KeyCode.LeftShift)) //si presiona también el shift, corre
                {
                    rigidbody.linearVelocity = new Vector2(-10f, rigidbody.linearVelocity.y);
                    animator.speed = 2; //usar atributo speed
                }
                else
                {
                    rigidbody.linearVelocity = new Vector2(-5f, rigidbody.linearVelocity.y);
                    animator.speed = 1;
                }
            }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            sprite.flipX = false;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                rigidbody.linearVelocity = new Vector2(10f, rigidbody.linearVelocity.y);
                animator.speed = 2;
            }
            else
            {
                rigidbody.linearVelocity = new Vector2(5f, rigidbody.linearVelocity.y);
                animator.speed = 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (water)
            {
                double_jump = true;
                rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, 4);    
            }
            else if (isGrounded())
            {
                double_jump = true;
                rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x , 15f);  
            }
            else if (double_jump)
            {
                double_jump = false;
                rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x , 15f);     
            }
        }
        }

        //Animator

        if (!hit) //si no le están pegando, puede hacer cualquier animación
        {
            //estado de reposo
            //está en el piso y no está presinando ni izq ni derecha
            if (isGrounded() && !Input.GetKey(KeyCode.LeftArrow)
            && !Input.GetKey(KeyCode.RightArrow))
            {
                animator.SetInteger("estado", 0);
            }
            //estado de caminar
            //reposo y izq o derecha
            else if (isGrounded() && (Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.RightArrow)))
            {
                animator.SetInteger("estado", 1);
            }
            else if (rigidbody.linearVelocity.y > 0)
            {
                if (double_jump)
                   animator.SetInteger("estado", 2);
                else
                    animator.SetInteger("estado", 4);
            }
            else if (rigidbody.linearVelocity.y < 0)
            {
                animator.SetInteger("estado", 3);
            }
        }
    }

    //método convencional para saber si el player está sobre el piso
    public bool isGrounded()
    {
        //método de clase physics2d llamado boxcast, pide el boxcollider del jugador, saca el centro, los límites o sea el tamaño, ángulo, dirección y cuánto quiero aplicar, layer
        return Physics2D.BoxCast(boxCollider.bounds.center, 
        boxCollider.bounds.size, 0, Vector2.down, 
        0.1f, layerMask);
        // entonces evalua lo inferior en un margen de 0.1 respecto abajo, está tocando un objeto en la capa terrain?
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Spikehead")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            // cuando colisione contra el trigger, pasa de ser rigidbody estático a dinámico, encontes caerá
            collision.gameObject.GetComponent<Rigidbody2D>().freezeRotation = true;
            // para que no rote
            collision.gameObject.GetComponent<Rigidbody2D>().mass = 100;
            // subir la masa para que no lo pueda mover el personaje
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 10;
            // fuerza con la que va a caer
        }

        if (collision.gameObject.tag == "Trampoline")
        {
            collision.gameObject.GetComponent<Animator>().SetInteger("trampestado", 1);
            rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x , 35f);
            // salta más
        }

        if (collision.gameObject.tag == "Apple")
        {
            num_manzanas++;
            textManzanas.SetText("" + num_manzanas);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Meta")
        {
            PlayerPrefs.SetInt("current_progress", 1); //llegaste al final del primer mundo
            SceneManager.LoadScene(1);
        }
    }

    // para cuando sales del trigger
    void OnTriggerStay2D(Collider2D collision)
    {
      if (collision.gameObject.tag == "Fan")
      {
            rigidbody.linearVelocity = new Vector2(0 , 4f);
      }  

      if (collision.gameObject.tag == "Water")
      {
            water = true;
            rigidbody.gravityScale = 0.5f;
      }  
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Trampoline")
        {
            collision.gameObject.GetComponent<Animator>().SetInteger("trampestado", 0);
        }

      if (collision.gameObject.tag == "Water")
      {
            water = false;
            rigidbody.gravityScale = 4f;
      }  
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Rino")
        {
            if (!isGrounded())
            {
                collision.gameObject.GetComponent<Animator>().
                SetTrigger("rinoend");
                rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, 15);
            }
            else
            {
                animator.SetTrigger("delete");
            }
        }

        if (collision.gameObject.tag == "Limite"
        || collision.gameObject.tag == "Spikehead")
        {
            animator.SetTrigger("delete");
        }

        if (collision.gameObject.tag == "Fire")
        {
            control = false;
            hit = true;

            animator.SetInteger("estado", 5);

            if (sprite.flipX) //izquierda
            {
                rigidbody.linearVelocity = 
                new Vector2(10f, 4f);
            }
            else
            {
                rigidbody.linearVelocity = 
                new Vector2(-10f, 4f);
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Plataforma")
        {
            transform.SetParent(collision.gameObject.transform);
            //vamos a setearle un padre al jugador
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Plataforma")
        {
            transform.SetParent(null);
        }
    }

    public void gotoDesapperearing()
    {
        SceneManager.LoadScene(2);
    }

    public void gotoIdle()
    {
        control = true;
        hit = false;

        animator.SetInteger("estado", 0);
    }
}
