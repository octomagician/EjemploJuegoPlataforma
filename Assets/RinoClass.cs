using UnityEngine;

public class RinoClass : MonoBehaviour
{
    Rigidbody2D rigidbody;
    SpriteRenderer sprite;
    int par_impar;
    float time_move;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        par_impar = 1;
        time_move = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > time_move)
        {
            if (par_impar % 2 == 0)
            {
                rigidbody.linearVelocity = new Vector2(10, 0);
                sprite.flipX = true;
            }
            else
            {
                rigidbody.linearVelocity = new Vector2(-10, 0);
                sprite.flipX = false;
            }
            
            par_impar++;
            time_move++;
        }
    }

    public void gotoEnd()
    {
        Destroy(gameObject);
    }
}
