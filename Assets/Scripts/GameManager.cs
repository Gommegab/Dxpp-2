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

    void Awake() {
        instance = this;
    }

    void Start() {

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
