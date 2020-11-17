using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{

    public Toggle bgmToggle;
    public Toggle sfxToggle;


    private void OnEnable()
    {
        bgmToggle.isOn = GameManager.Singleton.bgm;
        sfxToggle.isOn = GameManager.Singleton.sfx;
    }

    // Update is called once per frame
    void Update()
    {

    }



    public void OnClickClose()
    {
        gameObject.SetActive(false);
    }

    public void OnToggleBGM()
    {
        if(bgmToggle.isOn == true)
        {
            GameManager.Singleton.bgm = true;
        }
        else
        {
            GameManager.Singleton.bgm = false;
        }
        SaveManager.Singleton.SaveOptionJson();

        SoundManager.Singleton.BgmMute();
    }

    public void OnToggleSFX()
    {
        if (sfxToggle.isOn == true)
        {
            GameManager.Singleton.sfx = true;
        }
        else
        {
            GameManager.Singleton.sfx = false;
        }
        SaveManager.Singleton.SaveOptionJson();

        SoundManager.Singleton.SfxMute();
    }
}
