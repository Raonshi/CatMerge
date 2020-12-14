﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 남은 시간이 절반 이하일 경우 실행되며 플레이어에게 남은 시간 알림을 연출한다.
/// </summary>
public class HalfTime : MonoBehaviour
{
    Rigidbody2D rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_10Seconds"));
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rigidbody.position.y <= Screen.height * 0.5f)
        {
            rigidbody.velocity = Vector3.zero;
            return;
        }
        rigidbody.AddForce(new Vector2(0, -8000));

        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
