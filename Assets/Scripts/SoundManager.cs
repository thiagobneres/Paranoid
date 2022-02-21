using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip launch;
    public AudioClip beep;
    public AudioClip powerUp;
    public AudioClip powerDown;
    public AudioClip dropBall;
    public AudioClip timeUp;
    public AudioClip gameOver;
    public AudioClip win;
    public AudioClip explosion;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Launch()
    {
        audioSource.PlayOneShot(launch, 0.7F);
    }

    public void Beep()
    {
        audioSource.PlayOneShot(beep, 0.7F);
    }

    public void PowerUp()
    {
        audioSource.PlayOneShot(powerUp, 0.9F); // Need to boost volume
    }

    public void PowerDown()
    {
        audioSource.PlayOneShot(powerDown, 0.7F);
    }

    public void DropBall()
    {
        audioSource.PlayOneShot(dropBall, 0.9F);
    }

    public void TimeUp()
    {
        audioSource.PlayOneShot(timeUp, 0.9F);
    }

    public void Win()
    {
        audioSource.PlayOneShot(win, 0.9f);
    }

    public void GameOver()
    {
        audioSource.PlayOneShot(gameOver, 0.7F);
    }

    public void Explosion()
    {
        audioSource.PlayOneShot(explosion, 0.7f);
    }

}
