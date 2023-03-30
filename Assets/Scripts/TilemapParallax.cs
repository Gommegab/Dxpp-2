using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapParallax : MonoBehaviour {

    [SerializeField] private float scrollSpeed = 0.75f;
    [SerializeField] private GameObject viewTarget; // Cámara
    
    private Tilemap tilemap;

    void Start() {
        tilemap = GetComponent<Tilemap>();
    }
    
    void FixedUpdate() {
        float newXPos = viewTarget.transform.position.x * scrollSpeed;
        tilemap.transform.position = new Vector3(newXPos, tilemap.transform.position.y, tilemap.transform.position.z);
    }
}
