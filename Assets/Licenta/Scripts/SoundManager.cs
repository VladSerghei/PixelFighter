using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioClip menuMusic;
    public AudioClip gameMusic;
    private bool playingMenu = false;

    private AudioSource audioSource;
    private void Awake()
    {
        SetUpSingleton();
        
    }

    void SetUpSingleton()
    {
        if (FindObjectsOfType<SoundManager>().Length > 1)
            Destroy(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level")
        {
            if (playingMenu)
            {
                audioSource.clip = gameMusic;
                audioSource.Play();
                playingMenu = false;
            }
        }
        else if (!playingMenu)
        {
            audioSource.clip = menuMusic;
            audioSource.Play();
            playingMenu = true;
        }
    }
}
