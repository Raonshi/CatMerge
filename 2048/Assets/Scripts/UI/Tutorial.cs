using System.Collections;
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
        Game.instance.isClose = true;
    }

    public void OnClickOK()
    {

        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Click"));

        switch (gameObject.name)
        {
            case "Tutorial0":
                GameManager.Singleton.tutorial0 = isToggle;
                Game.instance.isClose = true;
                SaveManager.Singleton.SaveTutorialJson();
                gameObject.SetActive(false);
                break;

            case "Tutorial1":
                GameManager.Singleton.tutorial1 = isToggle;
                Game.instance.isClose = true;
                SaveManager.Singleton.SaveTutorialJson();
                gameObject.SetActive(false);
                break;

            case "Tutorial2":
                GameManager.Singleton.tutorial2 = isToggle;
                Game.instance.isClose = true;
                SaveManager.Singleton.SaveTutorialJson();
                gameObject.SetActive(false);
                break;

            case "Tutorial3":
                GameManager.Singleton.tutorial3 = isToggle;
                Game.instance.isClose = true;
                SaveManager.Singleton.SaveTutorialJson();
                gameObject.SetActive(false);
                break;

            case "Tutorial4":
                GameManager.Singleton.tutorial4 = isToggle;

                SaveManager.Singleton.SaveTutorialJson();
                gameObject.SetActive(false);
                break;
        }
    }

    public void OnClickToggle()
    {
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Click"));

        if (toggle.isOn == true)
        {
            isToggle = false;
        }
        else
        {
            isToggle = true;
        }
    }
}
