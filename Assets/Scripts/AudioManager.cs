using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private static AudioSource audioSource;

    void Awake()
    {
        instance = this;

        if ( audioSource != null )
        {
            Destroy( gameObject );
        }

        else {
            DontDestroyOnLoad( this.gameObject );
            audioSource = GetComponent<AudioSource>();
        }
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic( AudioClip clip )
    {
        if ( audioSource.isPlaying ) { return; }

        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PauseMusic()
    {
        audioSource.Pause();
    }

    public void UnpauseMusic()
    {
        audioSource.UnPause();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}
