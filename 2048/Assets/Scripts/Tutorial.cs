﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public bool isToggle;
    public Toggle toggle;

    void OnEnable()
    {
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Popup"));

        isToggle = false;
        toggle.isOn = true;

        Game.instance.isOver = true;
        Game.instance.isClose = true;
    }

    public void OnClickOK()
    {

        switch(gameObject.name)
        {
            case "Tutorial0":
                GameManager.Singleton.tutorial0 = isToggle;
                Game.instance.isOver = false;
                SaveManager.Singleton.SaveTutorialJson();
                gameObject.SetActive(false);
                break;

            case "Tutorial1":
                GameManager.Singleton.tutorial1 = isToggle;
                Game.instance.isOver = false;
                SaveManager.Singleton.SaveTutorialJson();
                gameObject.SetActive(false);
                break;

            case "Tutorial2":
                GameManager.Singleton.tutorial2 = isToggle;
                Game.instance.isOver = false;
                SaveManager.Singleton.SaveTutorialJson();
                gameObject.SetActive(false);
                break;

            case "Tutorial3":
                GameManager.Singleton.tutorial3 = isToggle;

                Game.instance.isOver = false;
                SaveManager.Singleton.SaveTutorialJson();
                gameObject.SetActive(false);
                break;
        }
    }

    public void OnClickToggle()
    {
        
        if(toggle.isOn == true)
        {
            isToggle = false;
        }
        else
        {
            isToggle = true;
        }
    }
}
