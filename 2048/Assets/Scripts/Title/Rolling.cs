﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rolling : MonoBehaviour
{
    public Transform startPoint;
    public Transform targetPoint;

    float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = Random.Range(500.0f, 700.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position == targetPoint.position)
        {
            transform.position = startPoint.position;
            moveSpeed = Random.Range(500.0f, 700.0f);
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
    }


    public void OnClick()
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
