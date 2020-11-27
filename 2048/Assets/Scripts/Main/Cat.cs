﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class Cat : MonoBehaviour
{
    List<Transform> targetList = new List<Transform>();
    public Transform target;
    public GameObject[] array;

    public Image image;

    //포인트 선물
    public float giftTime;
    public GameObject gift;
    public GameObject adGift;
    public int giftPoint;

    //선물 젠 시간
    public int minTime;
    public int maxTime;

    //신규 생성한 고양이 체크
    public bool isNew;

    float moveSpeed;

    private void Awake()
    {
        gift.SetActive(false);
        adGift.SetActive(false);
    }

    void Start()
    {
        moveSpeed = UnityEngine.Random.Range(50.0f, 100.0f);
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
                moveSpeed = UnityEngine.Random.Range(50.0f, 100.0f);
                target = targetList[UnityEngine.Random.Range(0, targetList.Count)];
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            }
        }

        if (giftTime <= 0)
        {
            giftTime = 0;
            Main.instance.isGift = true;
            Gift();
        }
    }

    public void Gift()
    {
        if (gift.activeSelf == true || adGift.activeSelf == true)
        {
            return;
        }

        int rand = UnityEngine.Random.Range(0, 3);

        if(rand == 0)
        {
            Main.instance.giftInfo.SetActive(true);
            giftPoint = GameManager.Singleton.townLevel;
            gift.SetActive(true);
        }
        else if(rand == 1)
        {
            Main.instance.giftInfo.SetActive(true);
            //광고 시청 보상
            giftPoint = GameManager.Singleton.townLevel;
            adGift.SetActive(true);
            Debug.Log("광고 시청 보상!");
        }
        else if(rand == 2)
        {
            giftTime = UnityEngine.Random.Range(minTime, maxTime);
        }
    }

    public void OnClickGift(int tmp)
    {
        if(tmp == 0)
        {
            SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_TimeRecovery"));

            GameManager.Singleton.totalPoint += giftPoint;
            SaveManager.Singleton.SaveUserJson();

            gift.SetActive(false);
            giftTime = UnityEngine.Random.Range(minTime, maxTime);
        }
        else if(tmp == 1)
        {
            SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Click"));
            adGift.SetActive(false);
            giftTime = UnityEngine.Random.Range(minTime, maxTime);
            Main.instance.adGiftPanel.SetActive(true);
        }
    }

    public void OnClickCat()
    {
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_CatSummon"));
        StartCoroutine(Animation());
    }


    IEnumerator Animation()
    {
        gameObject.GetComponent<Animator>().SetBool("Touch", true);

        yield return new WaitForSeconds(0.3f);

        gameObject.GetComponent<Animator>().SetBool("Touch", false);
    }
}
