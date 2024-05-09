using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource deathAudio;
    [SerializeField] private AudioSource hitAudio;
    [SerializeField] private AudioSource orbitHitAudio;
    [SerializeField] private AudioSource xpCollectAudio;
    [SerializeField] private AudioSource backgroundMusic;

    private static SoundPlayer instance;

    public static SoundPlayer GetInstance() => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayDeathAudio()
    {
        deathAudio.Play();
    }

    public void PlayHitAudio()
    {
        //Debug.Log("Playing scythe hit sound");
        hitAudio.Play();
    }

    public void PlayOrbitHitAudio()
    {
        //Debug.Log("Playing hammer hit sound" + orbitHitAudio.clip.name);
        orbitHitAudio.Play();
    }

    public void PlayXPCollectAudio()
    {
        if (xpCollectAudio != null)
        {
            xpCollectAudio.Play();
        }
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.loop = true; // Loop the background music
            backgroundMusic.Play(); // Start playing the music
        }
    }
}
