using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionCollider : MonoBehaviour {
    
    [SerializeField] private GameObject goEnemy;
    [SerializeField] private List<Collider2D> guardColliders;

    private Enemy enemy;

    void Start() {
        enemy = goEnemy.GetComponent<Enemy>();
    }

    void Update() {
        transform.position = goEnemy.transform.position;

        foreach( Collider2D guardCollider in guardColliders) {
            Physics2D.IgnoreCollision( GetComponent<Collider2D>(), guardCollider);
        }
    }

    void OnTriggerEnter2D( Collider2D other ) {
        if( other.gameObject.CompareTag("Player")) {
            enemy.SetPlayerIsClose(true);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            enemy.SetPlayerIsClose(false);
        }
    }
}
