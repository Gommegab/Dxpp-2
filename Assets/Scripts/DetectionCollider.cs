using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionCollider : MonoBehaviour {

    [SerializeField] private GameObject goEnemy;

    private Enemy enemy;

    void Start() {
        enemy = goEnemy.GetComponent<Enemy>();
    }

    void Update() {
        transform.position = goEnemy.transform.position;
    }

    void OnTriggerEnter2D( Collider2D other ) {
        if( other.gameObject.CompareTag("Player")) {
            enemy.SetPlayerIsClose(true);
        }
        if (other.gameObject.CompareTag("Enemy")) {
            enemy.SetPlayerIsClose(false);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            enemy.SetPlayerIsClose(false);
        }
    }
}
