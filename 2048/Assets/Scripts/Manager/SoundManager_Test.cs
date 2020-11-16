using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager_Test : MonoBehaviour
{

    public AudioSource[] audio = new AudioSource[5];


    private static SoundManager_Test instance;

    public static SoundManager_Test Singleton
    {
        get
        {
            if(instance == null)
            {
                GameObject obj = GameObject.Find("SoundManager");

                if(obj == null)
                {
                    obj = new GameObject("SoundManager");
                    obj.AddComponent<SoundManager_Test>();
                }
                instance = obj.GetComponent<SoundManager_Test>();
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        for(int i = 0; i < audio.Length; i++)
        {
            audio[i] = gameObject.AddComponent<AudioSource>();
            audio[i].Stop();
        }

    }
    public void PlayBGM(AudioClip clip)
    {
        for(int i = 0; i < audio.Length; i++)
        {
            //재생 중인 경우
            if(audio[i].isPlaying == true)
            {
                if(audio[i].clip.name == clip.name)
                {
                    audio[i].clip = clip;
                    audio[i].time = 0;
                    audio[i].loop = true;
                    audio[i].Play();
                    return;
                }
            }

            //재생중이 아닌 경우
            else if(audio[i].isPlaying == false)
            {
                audio[i].clip = clip;
                audio[i].time = 0;
                audio[i].loop = true;
                audio[i].Play();
                return;
            }
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        
    }
}
