using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float gameDuration;
    public List<Transform> continuePlayerPoints;

    // --- Música | efectos de son
    // Audio Scource do GameObject Player
    AudioSource playerAudio;

    // --- Menú Canvas
    [SerializeField] private TextMeshProUGUI timeCounter;
    [SerializeField] private TextMeshProUGUI guideText;
    [SerializeField] private float blinkingStartSeconds = 5f;
    [SerializeField] private List<Image> heartImages;
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject buttonPlay;
    [SerializeField] private GameObject buttonContinue;
    [SerializeField] private GameObject buttonRestart;
    [SerializeField] private GameObject levelCompletedText;
    [SerializeField] private GameObject gameOverText;
    
    float timeRemaining;
    float positionDeadByFall;
    bool gameOver, stageOver;
    int heartCount;

    public bool GameOver { get { return gameOver; } }
    public bool StageOver { get { return stageOver; } }
    public float PositionDeadByFall {get {return positionDeadByFall; } }

    private Color notBlinkingColor;
    private Light2D finishLight;
    private Light2D globalLight;
    private GameObject player;
    private Vector3 initialPlayerPosition;
    private Vector3 initialPlayerScale;
    private Vector3 initialCameraPosition;
    private SpriteRenderer playerSr;

    void Awake() {
        instance = this;
    }

    void Start()
    {
        heartCount = heartImages.Count;

        player = GameObject.Find("Player");
        // AudioSource do Player (música da escea)
        playerAudio = player.GetComponent<AudioSource>();
        // Volume de inicio
        playerAudio.volume = 1f;

        playerSr = player.GetComponent<SpriteRenderer>();

        finishLight = GameObject.Find("FinishLight").GetComponent<Light2D>();
        globalLight = GameObject.Find("GlobalLight").GetComponent<Light2D>();

        initialPlayerPosition = player.transform.position;
        initialPlayerScale = player.transform.localScale;
        initialCameraPosition = Camera.main.gameObject.transform.position;
        notBlinkingColor = timeCounter.color;

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

        if (stageOver) {
            finishLight.intensity += Time.deltaTime * 0.6f;
            finishLight.pointLightOuterRadius += Time.deltaTime;
        }

        if (gameOver) {
            playerSr.color = new Color(playerSr.color.r, playerSr.color.g, playerSr.color.b, playerSr.color.a - Time.deltaTime * 0.6f);
            globalLight.intensity -= Time.deltaTime * 0.4f;
            if (globalLight.intensity <= 0) {
                globalLight.intensity = 0;
                gameOverText.SetActive(true);
                StartCoroutine(GameOverRestartCoroutine(2f));
            }
        }

        if ( Input.GetKeyDown(KeyCode.Escape)){
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
    }

    public void StageEnd()
    {
        Debug.Log("WIN");
        stageOver = true;
        player.GetComponent<Player>().SetPlayerWon(true);
    }

    public void SetGameOver()
    {
        GameEnd();
    }

    public void Pause() {
        Time.timeScale = 0f;

        // Pausa o audio clip do AudioSource de Player
        playerAudio.Pause();
    }

    public void StartGame() {
        Time.timeScale = 1f;

        // Continúa o audio clip do AudioSource de Player
        playerAudio.Play();
    }

    public void Restart() {
        InitializeLevel();
        player.transform.position = initialPlayerPosition;
        player.transform.localScale = initialPlayerScale;
        Camera.main.gameObject.transform.position = initialCameraPosition;

        heartCount = heartImages.Count;
        heartImages.ForEach(h => h.color = Color.white);

        menuCanvas.SetActive(false);

        Time.timeScale = 1f;
        timeCounter.color = notBlinkingColor;

        // Inicia o audio clip do AudioSource de Player
        playerAudio.Play();
    }

    public void PlayerFlop( Vector3 flopPosition )
    {
        RemoveHearts();
        // Accions cando o Player cae nun foso
        RepositionPlayer( flopPosition );
    }

    void RepositionPlayer(Vector3 flopPosition )
    {
        Vector3 iPosition;

        // Posición de inicio do Player
        Vector3 continuePoint = continuePlayerPoints[0].position;

        for( int i = 0; i < continuePlayerPoints.Count; i++ )
        {
            iPosition = continuePlayerPoints[i].position;

            if( flopPosition.x > iPosition.x )
            {
                continuePoint = iPosition;
            }
        }

        player.transform.position = continuePoint;
        Camera.main.gameObject.transform.position = initialCameraPosition;
    }

    public void RemoveHearts()
    {
        if (heartCount > 0) {
            heartCount--;
            if( heartCount == 0 )
            {
                GameEnd();
            }

            Image lastHeart = heartImages[heartCount];
            lastHeart.color = Color.black;

            Debug.Log($"GameManager.PlayerFlop. Quedan {heartCount} corazonziños");
        }
    }

    public int GetHeartCount() {
        return heartCount;
    }

    public void StageCompleted() {
        levelCompletedText.SetActive(true);
        StartCoroutine(GameManager.instance.GameOverRestartCoroutine(1.8f));
    }

    public void DeactivateGuide() {
        StartCoroutine(coDeactivateGuide());
    }

    private void InitializeLevel() {
        gameOver = false;
        stageOver = false;
        // gameDuration = 15f;
        positionDeadByFall = -4f;
        // Inicializar o cronómetro ca duración máxima do xogo
        timeRemaining = gameDuration;

        // Para e inicializa o audio clip do AudioSource de Player
        playerAudio.Stop();

        timeCounter.text = ConvertSecondsToMinutesAndSeconds(gameDuration);
    }

    private IEnumerator BlinkingTime() {
        while (timeRemaining > 0f && timeRemaining <= blinkingStartSeconds) {
            timeCounter.color = Color.Lerp(notBlinkingColor, Color.red, Mathf.PingPong(Time.time, 1f));

            // Reducir o volume do AudioSource do Player
            if ( ! playerAudio.mute )
            {
                playerAudio.volume = Mathf.Lerp( playerAudio.volume, 0f, Time.deltaTime / blinkingStartSeconds );
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator GameOverRestartCoroutine(float seconds) {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator coDeactivateGuide() {
        float elapsedTime = 0f;
        float waitTime = 3f;
        float initialAlpha = guideText.color.a;
        while (elapsedTime < waitTime) {
            float newAlpha = Mathf.Lerp(initialAlpha, 0, (elapsedTime / waitTime));
            guideText.color = new Color(guideText.color.r, guideText.color.g, guideText.color.b, newAlpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        guideText.gameObject.SetActive(false);
        yield return null;
    }
}
