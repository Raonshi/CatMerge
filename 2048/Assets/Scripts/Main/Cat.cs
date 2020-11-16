﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cat : MonoBehaviour
{
    List<Transform> targetList = new List<Transform>();
    public Transform target;
    public GameObject[] array;

    public Image image;

    //포인트 선물
    public float giftTime;
    public GameObject gift;
    public int giftPoint;

    //선물 젠 시간
    public int minTime;
    public int maxTime;

    //신규 생성한 고양이 체크
    public bool isNew;

    private void Awake()
    {
        gift.SetActive(false);
    }

    void Start()
    {
        if (isNew == true)
        {
            giftTime = UnityEngine.Random.Range(minTime, maxTime);
            isNew = false;
        }
        else if(isNew == false)
        {
            if (TimeManager.Singleton.time.TotalSeconds > maxTime && GameManager.Singleton.isStart == true)
            {
                gift.SetActive(true);
                GameManager.Singleton.isStart = false;
            }
            else
            {
                giftTime = UnityEngine.Random.Range(minTime - (float)TimeManager.Singleton.time.TotalSeconds, maxTime - (float)TimeManager.Singleton.time.TotalSeconds);
            }
        }

        array = GameObject.FindGameObjectsWithTag("Target");
        for (int i = 0; i < array.Length; i++)
        {
            targetList.Add(array[i].transform);
        }
    }


    void Update()
    {
        giftTime -= Time.deltaTime;

        if(target == null)
        {
            target = targetList[UnityEngine.Random.Range(0, targetList.Count)];
        }
        else
        {
            if(Vector3.Distance(transform.localPosition, target.localPosition) <= 1)
            {
                target = targetList[UnityEngine.Random.Range(0, targetList.Count)];
            }
            else
            {
                Move();
            }
        }

        if (giftTime <= 0)
        {
            giftTime = 0;
            Main.instance.isGift = true;
            Gift();
        }
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, 0.5f);
    }

    public void Gift()
    {
        int rand = UnityEngine.Random.Range(0, 2);
        if(rand == 0)
        {
            giftPoint = GameManager.Singleton.townLevel;
            gift.SetActive(true);
        }
        else if(rand == 1)
        {
            giftTime = UnityEngine.Random.Range(minTime, maxTime);
        }
    }

    public void OnClickGift()
    {
        GameManager.Singleton.totalPoint += giftPoint;
        SaveManager.Singleton.SaveUserJson();

        gift.SetActive(false);
        giftTime = UnityEngine.Random.Range(minTime, maxTime);
    }

    public void OnClickCat()
    {
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_CatSummon"));
    }
}
