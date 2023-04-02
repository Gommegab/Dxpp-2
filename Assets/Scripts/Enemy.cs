using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject player;

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
            MoveTowardsPlayer();
        }
    }

    void OnTriggerEnter2D( Collider2D other ) {
        if (other.gameObject.CompareTag("EnemyGuard")) {
            ReverseMovement();
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            player.GetComponent<Player>().ReceiveAttack(horizontal);
        }
    }

    public void ReverseMovement()
    {
        horizontal = -horizontal;
        velocity = new Vector3(speed, 0, 0) * horizontal;
    }

    public void SetPlayerIsClose(bool isClose) {
        playerIsClose = isClose;
        animator.SetBool( "attack", isClose );
    }

    private void walkAround() {
        // Movemento
        Vector3 position = transform.position;
        position += velocity * Time.deltaTime;
        transform.position = position;

        horizontal = (int) Mathf.Sign(velocity.x);

        // Dirección
        Vector3 localScale = transform.localScale;
        localScale.x = horizontal;
        transform.localScale = localScale;
    }

    private void MoveTowardsPlayer() {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        float direction = player.transform.position.x - transform.position.x;
        Vector3 localScale = transform.localScale;
        localScale.x = direction > 0 ? Mathf.Abs(localScale.x) : Mathf.Abs(localScale.x) * -1;
        transform.localScale = localScale;
        horizontal = (int) Mathf.Sign(localScale.x);
    }
}
