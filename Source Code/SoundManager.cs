using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioClip playerAttack, playerDeath, playerWalk;
    public AudioClip enemyAttack, enemyDeath;
    public AudioClip backgroundMusic;

    private float lastPlayed;
    private AudioSource audioSrc;

    private void Start()
    {
        // make instance a Singleton
        if (SoundManager.instance != null) {
            Destroy(gameObject);
            return;
        }
        else {
            lastPlayed = -64f;
            audioSrc = GetComponent<AudioSource>();
        }
        instance = this;
    }

    private void Update() {
        if (Time.fixedUnscaledTime - lastPlayed >= 64f) {
            audioSrc.PlayOneShot(backgroundMusic, 0.05f);
            lastPlayed = Time.time;
        }
    }
    public void playSound(string clip) {
        switch (clip) {
            case "backgroundMusic":
                audioSrc.PlayOneShot(backgroundMusic, 0.05f);
                lastPlayed = Time.time;
                break;
            case "playerAttack":
                audioSrc.PlayOneShot(playerAttack, 0.7f);
                break;
            case "playerDeath":
                audioSrc.PlayOneShot(playerDeath, 1f);
                break;
            case "playerWalk":
                audioSrc.PlayOneShot(playerWalk, 0.7f);
                break;
            case "enemyAttack":
                audioSrc.PlayOneShot(enemyAttack, 0.7f);
                break;
            case "enemyDeath":
                audioSrc.PlayOneShot(enemyDeath, 1);
                break;
        }
    }
}
