using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public GameObject player;
    public GameObject gameOverSprite;

    [SerializeField] private float endXposition = 13f;
    private float initialXposition;

    void Start()
    {
        if ( player == null ) { PrintInitErrorVar("player"); }
        if ( gameOverSprite == null ) { PrintInitErrorVar("gameOverSprite"); }
        initialXposition = transform.position.x;

    }

    void Update()
    {
        if ( player != null )
        {
            GameObjectPosition( player );
        }

        if ( gameOverSprite!= null && GameManager.instance.GameOver )
        {
            GameObjectPosition( gameOverSprite );

            gameOverSprite.SetActive( true );
        }
    }

    void PrintInitErrorVar( string name )
    {
        Debug.Log($"CamFollow: La variable '{name}' no está correctamente inicializada");
    }

    void GameObjectPosition( GameObject go )
    {
        Vector3 position = transform.position;
        position.x = go.transform.position.x;
        if (position.x > initialXposition && position.x < endXposition) {
            transform.position = position;
        }
    }
}
