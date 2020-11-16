using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Text loading;
    public float speed;

    string message;

    bool isTyping;


    void Start()
    {
        isTyping = false;
        message = "로 딩 중 . . .";

        SoundManager.Singleton.SoundInit();
    }

    // Update is called once per frame
    void Update()
    {
        if(isTyping == false)
        {
            isTyping = true;
            StartCoroutine(Typing(speed));
        }
    }

    IEnumerator Typing(float speed)
    {
        for (int i = 0; i < message.Length; i++)
        {
            loading.text = message.Substring(0, i + 1);
            yield return new WaitForSeconds(speed);
        }

        isTyping = false;
    }
}
