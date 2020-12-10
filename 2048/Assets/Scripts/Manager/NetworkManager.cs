using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public string networkAddress = "192.168.0.46";

    float time = 0.5f;


    private static NetworkManager instance;


    public static NetworkManager Singleton
    {
        get
        {
            if(instance == null)
            {
                GameObject obj = GameObject.Find("NetworkManager");

                if(obj == null)
                {
                    obj = new GameObject("NetworkManager");
                    obj.AddComponent<NetworkManager>();
                    DontDestroyOnLoad(obj);
                }

                instance = obj.GetComponent<NetworkManager>();
            }
            return instance;
        }
    }


    public void InitNetworkManager()
    {
        Debug.Log("======================NetworkManger loaded======================");
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;

        if(time <= 0)
        {
            StartCoroutine(PingTest());
            time = 0.5f;
        }
    }

    IEnumerator PingTest()
    {
        Debug.Log("Ping Start : " + networkAddress);

        Ping ping = new Ping(networkAddress);

        yield return new WaitForSeconds(0.5f);

        if(ping.isDone)
        {
            Debug.Log(networkAddress + " : Ping Success");
        }
        else
        {
            Debug.Log(networkAddress + " : Ping Fail");
        }
    }

}
