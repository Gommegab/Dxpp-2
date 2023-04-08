using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform groundAdvanceController;
    [SerializeField] private Vector3 gcDimensionBox;

    private int horizontal;
    float startPositionY, dyingForce;

    Rigidbody2D rb;
    private GameObject player;
    private Vector3 velocity;
    private Animator animator;

    private bool playerIsClose = false;


    void Start()
    {
        player = GameObject.Find("Player");

        // Dirección do mov [ -1 | +1]
        horizontal = 1;
        dyingForce = 15f;

        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
        velocity = new Vector3(speed, 0, 0) * horizontal;
        startPositionY = transform.position.y;
    }


    void Update()
    {
        bool dying = transform.position.y < startPositionY - 1f;

        if ( !playerIsClose ) { walkAround(); }

        else { MoveTowardsPlayer(); }

        if ( dying )
        {
            animator.SetBool( "dying", dying );
            rb.AddForce( Vector2.up * dyingForce );
        }

    }

    void FixedUpdate()
    {
        bool test = Physics2D.OverlapBox(groundAdvanceController.position, gcDimensionBox, 0f, LayerMask.GetMask("Ground"));

        if ( !test ) { ReverseMovement(); }
    }

    void OnCollisionEnter2D( Collision2D other )
    {
        if ( other.gameObject.CompareTag("Player") )
        {
            player.GetComponent<Player>().ReceiveAttack(horizontal);
        }
    }

    void walkAround()
    {
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

    void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        float direction = player.transform.position.x - transform.position.x;

        Vector3 localScale = transform.localScale;

        localScale.x = direction > 0 ? Mathf.Abs(localScale.x) : Mathf.Abs(localScale.x) * -1;

        transform.localScale = localScale;
        horizontal = (int) Mathf.Sign(localScale.x);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundAdvanceController.position, gcDimensionBox);
    }


    public void ReverseMovement()
    {
        horizontal = -horizontal;
        velocity = new Vector3(speed, 0, 0) * horizontal;
    }

    public void SetPlayerIsClose( bool isClose )
    {
        playerIsClose = isClose;
        animator.SetBool( "attack", isClose );
    }

}
