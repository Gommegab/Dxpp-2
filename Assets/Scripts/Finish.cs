using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            // TODO: Chamada a GameManager para 
            Debug.Log("WIN");
        }
    }
    
}
