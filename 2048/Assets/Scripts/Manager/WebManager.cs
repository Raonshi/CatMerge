using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebManager : MonoBehaviour
{
    string url = "";



    private static WebManager instance;

    public static WebManager Singleton
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = GameObject.Find("WebManager");

                if (obj == null)
                {
                    obj = new GameObject("WebManager");
                    obj.AddComponent<WebManager>();
                }

                instance = obj.GetComponent<WebManager>();
            }
            return instance;
        }
    }


    public void InitWebManager()
    {
        DontDestroyOnLoad(gameObject);
        Debug.Log("WebManager Loaded!");
    }


    IEnumerator Start()
    {
        WWW www = new WWW(url);

        yield return www.text;
    }
}
