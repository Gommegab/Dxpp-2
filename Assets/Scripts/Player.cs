using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Públicas en Dev para ajustar al tiempo
    public float speed, jumpForce;

    float horizontal;

    Vector2 velocity;
    Rigidbody2D rb;
    Animator animator;

    private bool isGrounded;
    private bool isBeingAttacked = false;
    private bool isFlickerEnabled = false;


    void Start()
    {
        // Inicialización (Futura función según fases)
        speed = 2.5f;
        jumpForce = 8f;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        animator.SetBool( "dying", false );
    }


    void Update()
    {
        if ( ! GameManager.instance.GameOver )
        {
            isGrounded = IsGrounded();

            // Values -1.0f, 0f, 1.0f
            horizontal = Input.GetAxisRaw("Horizontal");

            // Se activa la animación de caer a velocidad Y hacia abajo
            animator.SetBool( "falling", rb.velocity.y < 0 && !isGrounded );

            if ( transform.position.y < GameManager.instance.PositionDeadByFall )
            {
                // Debug.Log( "Player: Vacuum fall!" );
                GameManager.instance.PlayerFlop( transform.position );
            }

            if (isGrounded)
            {

                // Se activa la animación de correr cuando Player no esté parado
                animator.SetBool( "running", horizontal != 0.0f );

                // Player mira en la misma dirección que su mvto.
                if ( horizontal < 0.0f ) {
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }
                else if ( horizontal > 0.0f ) {
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }

                if( Input.GetButtonDown("Jump") )
                {
                    Jump();
                }

                // Si no se pulsa la tecla de salto, se anula la animación
                else if ( animator.GetBool("jumping") )
                {
                    animator.SetBool( "jumping", false );
                }
            }
        }

        else {
            speed = 0;
            animator.SetBool( "dying", true );
        }
    }

    void FixedUpdate()
    {
        if(isGrounded && !isBeingAttacked)
        {
            // Movimiento con las teclas GetAxisRaw("Horizontal");
            rb.velocity = new Vector2( horizontal * speed, rb.velocity.y);
        }
    }

    public void ReceiveAttack(int horizontal) {
        if (!isFlickerEnabled) {
            isBeingAttacked = true;
            isFlickerEnabled = true;

            rb.AddForce( new Vector3(horizontal, 1) * 2.5f, ForceMode2D.Impulse);

            animator.SetTrigger("hit");
            animator.SetBool("jumping", true);
            
            StartCoroutine(coIsBeingAttacked());
            StartCoroutine(coFlickOnAttack());

            GameManager.instance.RemoveHearts();
            
        }        
    }

    void Jump()
    {
        rb.AddForce( Vector2.up * jumpForce, ForceMode2D.Impulse );
        animator.SetBool( "jumping", true );
    }

    void ReverseMovement()
    {
        horizontal = -horizontal;
        rb.velocity = new Vector2( horizontal * speed, rb.velocity.y);
    }

    void OnCollisionEnter2D( Collision2D other )
    {
        if( other.gameObject.CompareTag("Border") )
        {
            print("Player.OnTriggerEnter2D");
            ReverseMovement();
        }
    }

    bool IsGrounded()
    {
        // Physics.Raycast
        Vector3 origin = transform.position;
        origin.y -= 0.65f;
        Vector3 direction = Vector3.down;
        float maxDistance = 0.1f;

        LayerMask mask = LayerMask.GetMask("Ground");
        RaycastHit2D rc = Physics2D.Raycast(origin, direction, maxDistance, mask);
        Debug.DrawRay(origin, direction * maxDistance, Color.red );

        bool grounded = rc.collider != null;

        // print($"Player.IsGrounded() = {grounded}" );

        return grounded;
    }

    private IEnumerator coIsBeingAttacked() {
        while (isBeingAttacked) {
            yield return new WaitForSeconds(0.5f);
            if (isGrounded) {
                isBeingAttacked = false;
            }
        }
    }

    private IEnumerator coFlickOnAttack() {
        // while (isFlickerEnabled) {
        for (int i = 0; i < 5; i++) {
            GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.4f);
            GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(0.4f);
        }
        isFlickerEnabled = false;
    }
}
