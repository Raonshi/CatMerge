using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSpawn : MonoBehaviour
{
    public List<GameObject> cat = new List<GameObject>();

    public float timer;
    float time;

    int count;

    void Start()
    {
        time = timer;
        count = transform.childCount;

        for(int i = 0; i < count; i++)
        {
            cat.Add(transform.GetChild(i).gameObject);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;

        
        if(time <= 0)
        {
            int i = 0;

            while(i < count)
            {
                if(cat[i].activeSelf == true)
                {
                    i++;
                    continue;
                }
                else
                {
                    cat[i].SetActive(true);
                    break;
                }
            }

            time = timer;
        }
    }
}
