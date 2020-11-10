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
        isToggle = false;
        if (gameObject.name != "Tutorial4")
        {
            Game.instance.isOver = true;
            Game.instance.isClose = true;
        }
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
                Game.instance.itemTutorial1.SetActive(true);
                break;

            case "Tutorial1":
                GameManager.Singleton.tutorial1 = isToggle;
                Game.instance.isOver = false;
                SaveManager.Singleton.SaveTutorialJson();
                gameObject.SetActive(false);
                Game.instance.itemTutorial2.SetActive(true);
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

            case "ItemTutorial1":
                GameManager.Singleton.itemTutorial1 = isToggle;
                Game.instance.isOver = false;
                SaveManager.Singleton.SaveTutorialJson();
                gameObject.SetActive(false);
                break;

            case "ItemTutorial2":
                GameManager.Singleton.itemTutorial2 = isToggle;
                Game.instance.isOver = false;
                SaveManager.Singleton.SaveTutorialJson();
                gameObject.SetActive(false);
                break;
        }
    }

    public void OnClickToggle()
    {
        isToggle = !isToggle;

        if (isToggle == true)
        {
            toggle.isOn = false;
        }
        else
        {
            toggle.isOn = true;
        }

    }
}
