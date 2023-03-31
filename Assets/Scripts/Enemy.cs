using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public Collider2D detectionCollider;
    public Collider2D guardCollider;

    public float speed;

    int horizontal;

    Vector3 velocity;
    Animator animator;


    void Start()
    {
        // Dirección do mov [ -1 | +1]
        horizontal = 1;

        animator = GetComponent<Animator>();
        velocity = new Vector3(speed, 0, 0) * horizontal;
    }

    // Update is called once per frame
    void Update()
    {
        // Movemento
        Vector3 position = transform.position;
        position += velocity * Time.deltaTime;
        transform.position = position;

        // Dirección
        Vector3 localScale = transform.localScale;
        localScale.x = horizontal;
        transform.localScale = localScale;

        // Ignorando a colisión entre
        // o collider de detección da Player e os Colliders de garda
        Physics2D.IgnoreCollision( detectionCollider, guardCollider );
    }

    void ReverseMovement()
    {
        horizontal = -horizontal;
        velocity = new Vector3(speed, 0, 0) * horizontal;
    }

    void LookAtPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;

        if( direction.x < 0.0f )
        {
            ReverseMovement();
        }

        // if( direction.x >= 0.0f )
        // {
        //     transform.localScale = new Vector3( 1.0f, 1.0f, 1.0f );
        // }
        //
        // else {
        //     transform.localScale = new Vector3( -1.0f, 1.0f, 1.0f );
        // }
    }

    void OnTriggerEnter2D( Collider2D other )
    {
        if( other.gameObject.CompareTag("EnemyGuard") )
        {
            print($"Enemy.OnTriggerEnter2D {other.name}");
            ReverseMovement();
        }

        if( other.gameObject.CompareTag("Player") )
        {
            print($"Enemy.OnTriggerEnter2D {other.name}");

            animator.SetBool( "attack", true );
        }
    }

    void OnTriggerExit2D( Collider2D other )
    {
        if( other.gameObject.CompareTag("Player") )
        {
            print($"Enemy.OnTriggerExit2D {other.name}");

            animator.SetBool( "attack", false );
        }
    }
}
