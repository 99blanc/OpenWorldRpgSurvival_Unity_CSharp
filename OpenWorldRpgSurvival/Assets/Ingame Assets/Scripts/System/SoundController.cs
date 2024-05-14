using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundController : MonoBehaviour
{
    [HideInInspector] public static SoundController instance;

    public string titleName = "Title";
    public string ingameName = "Ingame";

    public string[] playSoundName;

    [SerializeField] private double fadeInSeconds;

    private double fadeDeltaTime;
    private bool fadeIn;

    public AudioSource moveSource;
    public Sound[] soundMoveEffect;

    public AudioSource[] audioEffect;
    public AudioSource audioBackground;

    public Sound[] soundEffect;
    public Sound[] soundBackground;

    #region singleton
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
    #endregion singleton

    private void Start()
    {
        try
        {
            playSoundName = new string[audioEffect.Length];

            if (SceneManager.GetActiveScene().name.Equals(titleName))
            {
                PlayBackgroundSE("Title");
            }
            if (SceneManager.GetActiveScene().name.Equals(ingameName))
            {
                PlayBackgroundSE("Ingame");
            }
        }
        catch (NullReferenceException)
        {

        }
    }

    private void Update()
    {
        FadeIn();
    }

    public void MoveSE(bool run, bool sprint)
    {
        try
        {
            if (run && sprint)
            {
                if (!moveSource.isPlaying)
                {
                    if (ThirdPersonController.isSwimming)
                    {
                        moveSource.clip = soundMoveEffect[0].clip;
                    }
                    else
                    {
                        moveSource.clip = soundMoveEffect[1].clip;
                    }
                    moveSource.Play();
                    moveSource.pitch = 1f;
                }
                else
                {
                    moveSource.pitch = 1f;
                }
            }
            else if (run)
            {
                if (!moveSource.isPlaying)
                {
                    if (ThirdPersonController.isSwimming)
                    {
                        moveSource.clip = soundMoveEffect[0].clip;
                    }
                    else
                    {
                        moveSource.clip = soundMoveEffect[1].clip;
                    }
                    moveSource.Play();
                    moveSource.pitch = 0.85f;
                }
                else
                {
                    moveSource.pitch = 0.85f;
                }
            }
            else
            {
                moveSource.Stop();
            }
        }
        catch (NullReferenceException)
        {

        }
    }

    public void FadeIn()
    {
        if (fadeIn)
        {
            fadeDeltaTime += Time.deltaTime;
            if (fadeDeltaTime >= fadeInSeconds)
            {
                fadeDeltaTime = fadeInSeconds;
                fadeIn = false;
            }
            audioBackground.volume = (float)(fadeDeltaTime / fadeInSeconds);
        }
    }

    public void PlaySE(string name)
    {
        try
        {
            for (int i = 0; i < soundEffect.Length; i++)
            {
                if (name == soundEffect[i].name)
                {
                    for (int j = 0; j < audioEffect.Length; j++)
                    {
                        if (audioEffect[j].isPlaying)
                        {
                            if (playSoundName[j].Equals(soundEffect[i].name))
                            {
                                return;
                            }
                        }
                        else
                        {
                            playSoundName[j] = soundEffect[i].name;
                            audioEffect[j].clip = soundEffect[i].clip;
                            audioEffect[j].Play();
                            return;
                        }
                    }
                    return;
                }
            }
        }
        catch (NullReferenceException)
        {

        }
    }

    public void PlayBackgroundSE(string name = "Ingame")
    {
        try
        {
            for (int i = 0; i < soundBackground.Length; i++)
            {
                if (name == soundBackground[i].name)
                {
                    audioBackground.clip = soundBackground[i].clip;
                    audioBackground.loop = true;
                    fadeIn = true;
                    audioBackground.Play();
                }
            }
        }
        catch (NullReferenceException)
        {

        }
    }

    public void StopAllSE()
    {
        try
        {
            for (int i = 0; i < audioEffect.Length; i++)
            {
                audioEffect[i].Stop();
            }
        }
        catch (NullReferenceException)
        {

        }
    }

    public void StopSE(string name)
    {
        try
        {
            for (int i = 0; i < audioEffect.Length; i++)
            {
                if (playSoundName[i] == name)
                {
                    audioEffect[i].Stop();
                    return;
                }
            }
        }
        catch (NullReferenceException)
        {

        }
    }

    public void StopBackgroundSE(string name = "Ingame")
    {
        try
        {
            for (int i = 0; i < soundBackground.Length; i++)
            {
                if (name == soundBackground[i].name)
                {
                    audioBackground.clip = soundBackground[i].clip;
                    audioBackground.loop = false;
                    audioBackground.Stop();
                }
            }
        }
        catch (NullReferenceException)
        {

        }
    }
}
