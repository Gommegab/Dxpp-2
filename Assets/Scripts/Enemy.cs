using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public Collider2D detectionCollider;
    public List<Collider2D> guardColliders;

    public float speed;

    int horizontal;

    Vector3 velocity;
    Animator animator;

    private bool playerIsClose = false;


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
        if (!playerIsClose) {
            walkAround();
        } else {
            MoveTovardsPlayer();
        }
        
        // Ignorando a colisión entre
        // o collider de detección da Player e os Colliders de garda
        foreach( Collider2D guardCollider in guardColliders )
        {
            Physics2D.IgnoreCollision( detectionCollider, guardCollider );
        }
    }

    void ReverseMovement()
    {
        horizontal = -horizontal;
        velocity = new Vector3(speed, 0, 0) * horizontal;
    }

    void OnTriggerEnter2D( Collider2D other )
    {
        if( other.gameObject.CompareTag("EnemyGuard") )
        {
            ReverseMovement();
        }

        if( other.gameObject.CompareTag("Player") )
        {
            playerIsClose = true;
            animator.SetBool( "attack", true );
        }
    }

    void OnTriggerExit2D( Collider2D other )
    {
        if( other.gameObject.CompareTag("Player") )
        {
            playerIsClose = false;
            animator.SetBool( "attack", false );
        }
    }

    private void walkAround() {
        // Movemento
        Vector3 position = transform.position;
        position += velocity * Time.deltaTime;
        transform.position = position;

        // Dirección
        Vector3 localScale = transform.localScale;
        localScale.x = horizontal;
        transform.localScale = localScale;
    }

    private void MoveTovardsPlayer() {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        float direction = player.transform.position.x - transform.position.x;
        Vector3 localScale = transform.localScale;
        localScale.x = direction > 0 ? Mathf.Abs(localScale.x) : Mathf.Abs(localScale.x) * -1;
        transform.localScale = localScale;
    }
}
