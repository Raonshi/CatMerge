using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager instance;

    public static SaveManager Singleton
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = GameObject.Find("SaveManager");

                if (obj == null)
                {
                    obj = new GameObject("SaveManager");
                    obj.AddComponent<SaveManager>();
                }

                instance = obj.GetComponent<SaveManager>();
            }
            return instance;
        }
    }


    public void InitSaveManager()
    {
        Debug.Log("======================SaveManger loaded======================");
        DontDestroyOnLoad(gameObject);

        if(GameManager.Singleton.isNew == true)
        {

        }
        LoadData("user");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveUserJson()
    {

    }

    public void LoadUserJson()
    {
        UserJson userJson;
    }


    #region 파일 저장 및 불러오기
    public void SaveData()
    {

    }


    public void LoadData(string fileName)
    {
        string fullPath = "Assets/Json/" + fileName + ".json";

        Debug.Log("데이터를 불러옵니다");
    }
    #endregion
}

public class UserJson
{
    int townLevel;
    float scoreRate;

    int best;
    int point;
}
