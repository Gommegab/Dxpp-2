using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed, jumpForce;

    float horizontal;

    Vector2 velocity;
    Rigidbody2D rb;
    Animator animator;


    void Start()
    {
        speed = 2.5f;
        jumpForce = 8f;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        Vector2 tmpPosition = transform.position;
        tmpPosition += velocity * Time.deltaTime;
        transform.position = tmpPosition;

        animator.SetBool( "running", horizontal != 0.0f );

        horizontal = Input.GetAxisRaw("Horizontal") * speed;

        if ( horizontal < 0.0f ) {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else if ( horizontal > 0.0f ) {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        if( Input.GetKeyDown(KeyCode.W) && IsGrounded() )
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2( horizontal, rb.velocity.y);
    }


    void Jump()
    {
        rb.AddForce( Vector2.up * jumpForce, ForceMode2D.Impulse );
        animator.SetBool( "jumping", true );
    }

    bool IsGrounded()
    {
        // Physics.Raycast

        Vector3 origin = transform.position;
        origin.y -= 0.64f;
        Vector3 direction = Vector3.down;
        float maxDistance = 0.1f;
        // LayerMask mask;

        RaycastHit2D rc = Physics2D.Raycast(origin, direction, maxDistance );
        Debug.DrawRay(origin, direction * 0.1f, Color.red );

        bool grounded = rc ? true : false;

        // print("Player.IsGrounded() " + grounded );

        return grounded;
    }
}
