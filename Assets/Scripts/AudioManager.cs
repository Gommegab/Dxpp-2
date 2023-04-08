using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    AudioSource audioSource;
    AudioSource playerAudio;

    public AudioSource PlayerAudio { get { return playerAudio; } }

    void Awake() {
        instance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // AudioSource do Player (música da escea)
        playerAudio = GameObject.Find("Player").GetComponent<AudioSource>();

        // Volume de inicio do reproductor de audio da Player
        playerAudio.volume = 1f;
    }

    // PlayOneShot
    public void PlaySync( AudioClip clip )
    {
        audioSource.PlayOneShot( clip );
    }

    // Métodos para o reproductor de música da Player

    public void PlayerPlay() {
        if ( playerAudio != null ) { playerAudio.Play(); }
    }

    public void PlayerPause() {
        if ( playerAudio != null ) { playerAudio.Pause(); }
    }

    public void PlayerStop() {
        if ( playerAudio != null ) { playerAudio.Stop(); }
    }

    public AudioSource GetPlayerAudio () { return playerAudio; }


    // Corrutina pública RepeatingClipCoroutine()
    // para reproducir un clip de audio completo n veces
    // @param: Audioclip clip. Clip de audio a reproducir
    // @param: Int n. Número de repeticións. Default 1

    public IEnumerator RepeatingClipCoroutine ( AudioClip clip, int n = 1 )
    {
        for ( int i=0; i < n; i++ )
        {
            // reproducción simultánea con otros clips
            audioSource.PlayOneShot( clip );
            yield return new WaitForSeconds( clip.length );
        }
    }

    // Corrutina pública FadeOutClipCoroutine()
    // Corrutina para reproducir un clip de audio completo
    // e que baixe paulatinamente o volume ate 0 ao rematar o clip
    // @param: AudioClip clip. Clip de audio a reproducir co efecto FadeOut

    public IEnumerator FadeOutClipCoroutine ( AudioClip clip )
    {
        // reproducción simultánea con otros clips
        audioSource.PlayOneShot( clip );

        // Audio decrescendo
        StartCoroutine( VolumeFadeOutCoroutine( audioSource, clip.length ) );

        // Para reproducir todo o clip
        yield return new WaitForSeconds( clip.length );
    }

    // Corrutina VolumeFadeOutCoroutine()
    // para baixar paulatinamente o volume de un AudioSource 'a' ate 0
    // durante un tempo 't'
    // @param: AudioSource a. O reproductor de audio
    // @param: float t. Duración do efecto FadeOut ate o mute

    private IEnumerator VolumeFadeOutCoroutine( AudioSource a, float t )
    {
        // Gardar o volume inicial
        float startVolume = a.volume;

        // Baixando o volumen paulatinamente por frame
        while ( a.volume > 0 )
        {
            a.volume -= startVolume * Time.deltaTime / t;

            yield return null;
        }

        // Parar o audio cando se chege ao tempo 't'
        a.Stop();

        // Restablecer o volume inicial do AudioSource
        a.volume = startVolume;
    }
}
