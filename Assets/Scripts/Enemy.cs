using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;

    int horizontal;
    Vector3 velocity;
    Animator animator;


    void Start()
    {
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
    }

    void OnTriggerEnter2D( Collider2D other )
    {
        if( other.gameObject.CompareTag("EnemyGuard") )
        {
            // print("Enemy.OnTriggerEnter2D");
            ReverseMovement();
        }
    }

    void ReverseMovement()
    {
        horizontal = -horizontal;
        velocity = new Vector3(speed, 0, 0) * horizontal;
    }
}
