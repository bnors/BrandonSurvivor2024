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
    [SerializeField] private AudioSource weakPointHitAudio;
    [SerializeField] private AudioSource playerHitAudio;
    [SerializeField] private AudioSource fireballLaunchAudio;

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

    public void PlayPlayerHitAudio()
    {
        if (playerHitAudio != null)
        {
            playerHitAudio.Play();
        }
        else
        {
            Debug.LogWarning("Player hit audio is not assigned.");
        }
    }

    public void PlayFireballLaunchAudio()
    {
        if (fireballLaunchAudio != null)
        {
            fireballLaunchAudio.Play();
        }
        else
        {
            Debug.LogWarning("Fireball launch audio is not assigned.");
        }
    }

    public void PlayDeathAudio()
    {
        deathAudio.Play();
    }

    public void PlayHitAudio()
    {
        hitAudio.Play();
    }

    public void PlayOrbitHitAudio()
    {
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
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }
    }

    // New method for playing the weak point hit sound
    public void PlayWeakPointHitAudio()
    {
        if (weakPointHitAudio != null)
        {
            weakPointHitAudio.Play();
        }
        else
        {
            Debug.LogWarning("Weak point hit audio is not assigned.");
        }
    }
}
