using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public Text loading;

    string message;

    void Start()
    {
        message = "로 딩 중 . . .";

        SoundManager.Singleton.SoundInit();

        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(1.0f);

        AsyncOperation async = SceneManager.LoadSceneAsync(GameManager.Singleton.nextScene);

        async.allowSceneActivation = false;

        while(async.isDone == false)
        {
            //로딩 진행중일때
            if(async.progress < 0.9f)
            {
                loading.text = message;
            }
            //로딩이 끝나면
            else
            {
                async.allowSceneActivation = true;
                yield break;
            }
        }
    }
}
