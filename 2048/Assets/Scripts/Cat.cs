using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cat : MonoBehaviour
{
    public List<Transform> targetList = new List<Transform>();
    public Transform target;
    public GameObject[] array;

    public Image image;

    //포인트 선물
    public float giftTime;
    public GameObject gift;


    // Start is called before the first frame update
    void Start()
    {
        gift.SetActive(false);
        giftTime = UnityEngine.Random.Range(300, 601);
        array = GameObject.FindGameObjectsWithTag("Target");
        for (int i = 0; i < array.Length; i++)
        {
            targetList.Add(array[i].transform);
        }

        //마지막 게임 종료로부터 10분 이상 지났다면 선물 활성화
        if (TimeManager.Singleton.time.TotalSeconds > 600)
        {
            gift.SetActive(true);
        }
    }




    // Update is called once per frame
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
            gift.SetActive(true);
        }
        else if(rand == 1)
        {
            giftTime = UnityEngine.Random.Range(300, 601);
        }
    }

    public void OnClickGift()
    {
        int point = PlayerPrefs.GetInt("point");
        PlayerPrefs.SetInt("point", point + 5);

        gift.SetActive(false);
        giftTime = UnityEngine.Random.Range(300, 601);
    }
}
