using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public int destroy;


    private void Awake()
    {
        if(PlayerPrefs.HasKey("destroy" + gameObject.name))
        {
            destroy = PlayerPrefs.GetInt("destroy" + gameObject.name);
        }
        else
        {
            destroy = 0;
        }
    }

    void OnEnable()
    {
        if (gameObject.name != "Tutorial4")
        {
            Game.instance.isOver = true;
            Game.instance.isClose = true;
        }
    }

    public void OnClickOK()
    {
        if(gameObject.name != "Tutorial4")
        {
            Game.instance.isOver = false;
        }
        gameObject.SetActive(false);
    }

    public void OnClickToggle()
    {
        destroy = 1;
        PlayerPrefs.SetInt("destroy" + gameObject.name, destroy);
    }
}
