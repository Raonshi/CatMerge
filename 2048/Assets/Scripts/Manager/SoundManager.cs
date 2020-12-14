﻿using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<AudioSource> audio = new List<AudioSource>();
    public int audioSourceCount = 5;


    private static SoundManager instance;

    public static SoundManager Singleton
    {
        get
        {
            if(instance == null)
            {
                GameObject obj = GameObject.Find("SoundManager");

                if(obj == null)
                {
                    obj = new GameObject("SoundManager");

                    obj.AddComponent<SoundManager>();
                }

                instance = obj.GetComponent<SoundManager>();
            }

            return instance;
        }
    }


    public void InitSoundManager()
    {
        for (int i = 0; i < audioSourceCount; i++)
        {
            audio.Add(gameObject.AddComponent<AudioSource>());
            audio[i].Stop();
        }

        DontDestroyOnLoad(gameObject);
    }


    private void Update()
    {
        VolumeControl();
    }

    /// <summary>
    /// GameManager의 bgm, sfx값을 통해 볼륨 조절
    /// </summary>
    public void VolumeControl()
    {
        for(int i = 0; i < audio.Count; i++)
        {
            if(audio[i].clip == null)
            {
                continue;
            }

            else if(audio[i].clip.name.Contains("BGM_") == true)
            {
                audio[i].volume = GameManager.Singleton.bgm;
            }
            else if(audio[i].clip.name.Contains("SFX_") == true)
            {
                audio[i].volume = GameManager.Singleton.sfx;
            }
        }
    }

    /// <summary>
    /// 모든 사운드를 정지
    /// </summary>
    public void SoundInit()
    {

        for (int i = 0; i < audio.Count; i++)
        {
            audio[i].Stop();
            audio[i].clip = null;
        }

    }

    /// <summary>
    /// 사운드를 재생한다.
    /// </summary>
    /// <param name="clip">재생할 사운드</param>
    public void PlaySound(AudioClip clip)
    {
        for (int i = 0; i < audio.Count; i++)
        {
            //재생 중인 경우
            if (audio[i].isPlaying == true)
            {
                if (audio[i].clip.name == clip.name)
                {
                    if (clip.name.Contains("BGM") == true)
                    {
                        audio[i].clip = clip;
                        audio[i].volume = 0.5f;
                        audio[i].time = 0;
                        audio[i].loop = true;
                    }
                    else if (clip.name.Contains("SFX") == true)
                    {
                        audio[i].loop = false;
                    }

                    audio[i].Play();
                    return;
                }
                else
                {
                    continue;
                }
            }

            //재생중이 아닌 경우
            else if (audio[i].isPlaying == false)
            {
                audio[i].clip = clip;
                audio[i].time = 0;

                if(clip.name.Contains("BGM") == true)
                {
                    audio[i].volume = 0.5f;
                    audio[i].loop = true;
                }
                else if(clip.name.Contains("SFX") == true)
                {
                    audio[i].loop = false;
                }
                
                audio[i].Play();
                return;
            }
        }
    }
}
