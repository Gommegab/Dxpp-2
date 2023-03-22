using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        if ( player == null ) { PrintInitErrorVar("player"); }

    }

    void Update()
    {
        if ( player != null )
        {
            Vector3 position = transform.position;
            position.x = player.transform.position.x;
            transform.position = position;
        }
    }

    void PrintInitErrorVar( string name )
    {
        Debug.Log($"CamFollow: La variable '{name}' no está correctamente inicializada");
    }
}
