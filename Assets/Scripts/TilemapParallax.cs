using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapParallax : MonoBehaviour {
    [Range(0,1f)][SerializeField] private float parallaxMultiplier = 0.75f;

    private Transform cameraTransform;
    private Vector3 previousCameraPosition;
    private float spriteWidth, startPosition;
    private float offsetTranslate = 3f;
    
    void Start() {
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;
        spriteWidth = GetComponent<TilemapRenderer>().bounds.size.x;
        startPosition = transform.position.x;
    }

    void LateUpdate() {
        float deltaX = (cameraTransform.position.x - previousCameraPosition.x) * parallaxMultiplier;    
        float moveAmount = cameraTransform.position.x * (1 - parallaxMultiplier);
        transform.Translate(new Vector3(deltaX, 0, 0));
        previousCameraPosition = cameraTransform.position;

        if (moveAmount > startPosition + spriteWidth - offsetTranslate) {
            transform.Translate(new Vector3(spriteWidth, 0, 0));
            startPosition += spriteWidth;
        } else if (moveAmount < startPosition - spriteWidth + offsetTranslate * 2) {
            transform.Translate(new Vector3(-spriteWidth, 0, 0));
            startPosition -= spriteWidth;
        }
    }
}
