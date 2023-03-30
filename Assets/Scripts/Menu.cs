using UnityEngine;

public class Menu : MonoBehaviour {
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
}
