using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float gameDuration;
    public List<Transform> spawnPlayerPoints;

    [SerializeField] private TextMeshProUGUI timeCounter;
    [SerializeField] private float blinkingStartSeconds = 5f;
    [SerializeField] private List<Image> heartImages;
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject buttonPlay;
    [SerializeField] private GameObject buttonContinue;
    [SerializeField] private GameObject buttonRestart;

    float timeRemaining;
    float positionDeadByFall;
    bool gameOver, stageOver;
    int heartCount;

    public bool GameOver { get { return gameOver; } }
    public float PositionDeadByFall {get {return positionDeadByFall; } }

    private Color notBlinkingColor;

    private GameObject player;
    private Vector3 initialPlayerPosition;
    private Vector3 initialPlayerScale;
    private Vector3 initialCameraPosition;

    void Awake() {
        instance = this;
    }

    void Start()
    {
        heartCount = heartImages.Count;
        print($"GameManager. Vidas {heartCount}");

        player = GameObject.Find("Player");
        initialPlayerPosition = player.transform.position;
        initialPlayerScale = player.transform.localScale;
        initialCameraPosition = Camera.main.gameObject.transform.position;
        notBlinkingColor = timeCounter.color;

        Pause();    // Inicializamos o xogo en pausa para arrancalo dende o menú
        InitializeLevel();
    }

    void Update() {

        if( ! stageOver && ! gameOver ) {
            // Resta o tempo transcurrido dende o último frame ao tempo restante
            timeRemaining -= Time.deltaTime;

            if ( timeRemaining <= 0f )
            {
                // Cando o crono chegue a cero, remata o xogo
                GameEnd();
            }

            else
            {
                // Mostrar el tiempo restante en GUI
                timeCounter.text = ConvertSecondsToMinutesAndSeconds(timeRemaining);

                if (timeRemaining <= blinkingStartSeconds) {
                    StartCoroutine(BlinkingTime());
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Pause();
            menuCanvas.SetActive(true);
            if (buttonPlay.activeSelf) {
                buttonPlay.SetActive(false);
                buttonRestart.SetActive(true);
                buttonContinue.SetActive(true);
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
        timeCounter.text = "";
        heartImages.ForEach(h => h.color = Color.black);

        StartCoroutine(GameOverRestartCoroutine());
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

    public void Pause() {
        Time.timeScale = 0f;
    }

    public void StartGame() {
        Time.timeScale = 1f;
    }

    public void Restart() {
        InitializeLevel();
        player.transform.position = initialPlayerPosition;
        player.transform.localScale = initialPlayerScale;
        Camera.main.gameObject.transform.position = initialCameraPosition;
        menuCanvas.SetActive(false);
        Time.timeScale = 1f;
        timeCounter.color = notBlinkingColor;
    }

    public void PlayerFlop( Vector3 flopPosition )
    {
        if( heartCount == 0 )
        {
            GameEnd();
        }

        // Colócase o Player no último punto de espanea antes do foso
        else {
            // Accions cando o Player cae nun foso
            print($"GameManager: flopPosition = {flopPosition}");

            heartImages.RemoveAt(0);
            heartCount = heartImages.Count;

            Vector3 iPosition;

            // Posición de inicio do Player
            Vector3 spawnPoint = spawnPlayerPoints[0].position;


            for( int i = 0; i < spawnPlayerPoints.Count; i++ )
            {
                iPosition = spawnPlayerPoints[i].position;
                print($"GameManager: position {i} {iPosition.x}");

                if( flopPosition.x > iPosition.x )
                {
                    spawnPoint = spawnPlayerPoints[i].position;
                }

                // O punto de caída nunca será menor que o de inicio
                else {
                    print($"GameManager: position else {i} {iPosition.x}");
                    spawnPoint = spawnPlayerPoints[i-1].position;
                }
            }

            print($"GameManager: spawnPoint = {spawnPoint.x}");

            player.transform.position = spawnPoint;
            Camera.main.gameObject.transform.position = initialCameraPosition;
        }

        Debug.Log($"GameManager.PlayerFlop. Quedan {heartCount} corazonziños");
    }

    private void InitializeLevel() {
        gameOver = false;
        stageOver = false;
        // gameDuration = 15f;
        positionDeadByFall = -4f;
        // Inicializar o cronómetro ca duración máxima do xogo
        timeRemaining = gameDuration;
        timeCounter.text = ConvertSecondsToMinutesAndSeconds(gameDuration);
    }

    private IEnumerator BlinkingTime() {
        while (timeRemaining > 0f && timeRemaining <= blinkingStartSeconds) {
            timeCounter.color = Color.Lerp(notBlinkingColor, Color.red, Mathf.PingPong(Time.time, 1f));
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator GameOverRestartCoroutine() {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
