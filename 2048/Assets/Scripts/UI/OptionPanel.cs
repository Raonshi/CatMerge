﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{

    public GameObject bgm;
    public GameObject sfx;

    Slider bgmSlider;
    Slider sfxSlider;

    

    private void Start()
    {
        bgmSlider = bgm.GetComponentInChildren<Slider>();
        sfxSlider = sfx.GetComponentInChildren<Slider>();

        bgmSlider.value = GameManager.Singleton.bgm;
        sfxSlider.value = GameManager.Singleton.sfx;
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.Singleton.bgm = bgmSlider.value;
        GameManager.Singleton.sfx = sfxSlider.value;

        SaveManager.Singleton.SaveOptionJson();
    }



    public void OnClickClose()
    {
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Click"));
        gameObject.SetActive(false);
    }
}
