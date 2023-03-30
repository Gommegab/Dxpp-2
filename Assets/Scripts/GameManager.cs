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

    public bool GameOver { get { return gameOver; } }
    public float PositionDeadByFall {get {return positionDeadByFall; } }

    private Color notBlinkingColor;
    
    private GameObject player;
    private Vector3 initialPlayerPosition;
    private Vector3 initialPlayerScale;
    private Vector3 initialCameraPosition;

    private bool gameStarted = false;

    void Awake() {
        instance = this;
    }

    void Start() {

        player = GameObject.Find("Player");
        initialPlayerPosition = player.transform.position;
        initialPlayerScale = player.transform.localScale;
        initialCameraPosition = Camera.main.gameObject.transform.position;

        Pause();    // Inicializamos o xogo en pausa para arrancalo dende o menú
        InitializeLevel();
    }

    void Update()
    {
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
            if (!gameStarted) {
                buttonPlay.SetActive(false);
                buttonRestart.SetActive(true);
                buttonContinue.SetActive(true);
                gameStarted = true;
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

    private IEnumerator BlinkingTime() {     
        while (timeRemaining > 0f && gameStarted) {
            timeCounter.color = Color.Lerp(notBlinkingColor, Color.red, Mathf.PingPong(Time.time, 1f));
            yield return new WaitForSeconds(0.5f);
        }
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
        timeCounter.color = notBlinkingColor;
        gameStarted = false;
        menuCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    private void InitializeLevel() {
        gameOver = false;
        stageOver = false;
        // gameDuration = 15f;
        positionDeadByFall = -4f;
        // Inicializar o cronómetro ca duración máxima do xogo
        timeRemaining = gameDuration;
        notBlinkingColor = timeCounter.color;

        timeCounter.text = ConvertSecondsToMinutesAndSeconds(gameDuration);
    }
}
