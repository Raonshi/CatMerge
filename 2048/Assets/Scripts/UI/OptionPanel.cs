﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 옵션 창 활성화 시 실행
/// </summary>
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
        if(SceneManager.GetActiveScene().name == "Game")
        {
            Game.instance.isClose = true;
        }
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Click"));
        gameObject.SetActive(false);
    }

    public void OnClickDataSave()
    {
        SaveManager.Singleton.SaveDataOutSide();
    }

    public void OnClickDataLoad()
    {
        SaveManager.Singleton.LoadDataOutSide();

        Main.instance.CatDispose();
        Main.instance.UpdateCatTown();
        Main.instance.CatSpawn(GameManager.Singleton.catCount, true);
    }
}
