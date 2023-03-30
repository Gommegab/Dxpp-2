using UnityEngine;

public class Menu : MonoBehaviour {

    [SerializeField] private GameObject creditsPanel;

    public void Play() {
        gameObject.SetActive(false);
        GameManager.instance.StartGame();
        // Debug.Log("Play started");
    }

    public void Quit() {
        Debug.Log("Quit game");
        Application.Quit();
    }

    public void Restart() {
        GameManager.instance.Restart();
    }

    public void Credits() {
        creditsPanel.SetActive(true);
    }

    public void CloseCredits() {
        creditsPanel.SetActive(false);
    }
}
