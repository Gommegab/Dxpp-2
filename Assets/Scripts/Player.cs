using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float speed;

    // Direction X, captura las teclas del mov. horizontal
    float horizontal;

    Vector3 velocity;
    Rigidbody2D rb;
    Animator animator;


    void Start()
    {
        speed = 2.5f;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        Vector3 tmpPosition = transform.position;
        tmpPosition += velocity * Time.deltaTime;
        transform.position = tmpPosition;

        animator.SetBool( "running", horizontal != 0.0f );

        horizontal = Input.GetAxisRaw("Horizontal");

        if ( horizontal < 0.0f ) {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else if ( horizontal > 0.0f ) {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2( horizontal, rb.velocity.y);

        print( horizontal );
    }
}
