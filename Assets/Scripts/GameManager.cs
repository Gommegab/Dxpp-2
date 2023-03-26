using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float gameDuration;

    float timeRemaining;
    float positionDeadByFall;
    bool gameOver, stageOver;

    public bool GameOver { get { return gameOver; } }
    public float PositionDeadByFall {get {return positionDeadByFall; } }

    void Awake() {
        instance = this;
    }

    void Start()
    {
        gameOver = false;
        stageOver = false;
        // gameDuration = 15f;
        positionDeadByFall = -4f;
        // Inicializar o cronómetro ca duración máxima do xogo
        timeRemaining = gameDuration;
    }

    void Update()
    {
        if( ! stageOver )
        {
            // Resta o tempo transcurrido dende o último frame ao tempo restante
            timeRemaining -= Time.deltaTime;

            if ( timeRemaining <= 0f )
            {
                // Cando o crono chegue a cero, remata o xogo
                GameEnd();
            }

            else {
                // Mostrar el tiempo restante por consola
                Debug.Log( "Chrono: " + ConvertSecondsToMinutesAndSeconds(timeRemaining) );
            }
        }
    }

    string ConvertSecondsToMinutesAndSeconds(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60f);
        int remainingSeconds = Mathf.RoundToInt(seconds % 60f);

        // Formatear a cadea de saída
        return string.Format("{0:00}:{1:00}", minutes, remainingSeconds);
    }

    void GameEnd()
    {
        // Debug.Log("GAME OVER");
        gameOver = true;
    }

    public void StageEnd()
    {
        Debug.Log("WIN");
        stageOver = true;
    }

    public void SetGameOver()
    {
        GameEnd();
    }
}
