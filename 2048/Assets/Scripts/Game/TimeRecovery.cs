using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRecovery : MonoBehaviour
{
    public float destroyTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        destroyTime -= Time.deltaTime;

        if(destroyTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
