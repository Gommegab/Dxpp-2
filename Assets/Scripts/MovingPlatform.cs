using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    [SerializeField] private Transform startPoint, endPoint;
    [SerializeField] private float speed;

    private Vector3 nextPosition;

    void Start() {
        nextPosition = startPoint.position;
    }

    void Update() {
        
        if (transform.position == startPoint.position) {
            nextPosition = endPoint.position;
        } 
        if (transform.position == endPoint.position) {
            nextPosition = startPoint.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);
        
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(startPoint.position, endPoint.position);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            other.gameObject.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            other.gameObject.transform.parent = null;
        }
    }
}
