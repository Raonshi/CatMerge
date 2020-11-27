using System.Collections;
using System.Net;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public bool allowCarrierDataNetwork = false;
    public string pingAddress = "8.8.8.8"; // Google Public DNS server
    public float waitingTime = 2.0f;

    private Ping ping;
    private float pingStartTime;

    public bool isInternetOn;

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
                }
                instance = obj.GetComponent<NetworkManager>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }


    public void InitNetworkManager()
    {
        switch (Application.internetReachability)
        {
            //WiFi, LAN 연결 가능
            case NetworkReachability.ReachableViaLocalAreaNetwork:
                isInternetOn = true;
                break;
            //캐리어 이더넷 연결 가능
            case NetworkReachability.ReachableViaCarrierDataNetwork:
                isInternetOn = allowCarrierDataNetwork;
                break;
            //연결 안됨
            default:
                isInternetOn = false;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        InitNetworkManager();

        if (isInternetOn == false)
        {
            //Debug.Log("InterNet Not Conneted");
            return;
        }

        ping = new Ping(pingAddress);
        pingStartTime = Time.time;

        if (ping != null)
        {
            bool stop = true;

            if (ping.isDone == true)
            {
                if (ping.time >= 0)
                {
                    Debug.Log("Internet Connected!" + ping);
                }
                else
                {
                    Debug.Log("InterNet Not Conneted");
                }
            }
            else if (Time.time - pingStartTime < waitingTime)
            {
                stop = false;
            }
            else
            {
                Debug.Log("InterNet Not Conneted");
            }

            if (stop == true)
            {
                ping = null;
            }
        }
    }
}
