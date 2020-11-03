using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public List<Transform> targetList = new List<Transform>();
    public Transform target;
    public GameObject[] array;

    // Start is called before the first frame update
    void Start()
    {
        array = GameObject.FindGameObjectsWithTag("Target");
        for (int i = 0; i < array.Length; i++)
        {
            targetList.Add(array[i].transform);
        }
    }




    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            target = targetList[Random.Range(0, targetList.Count)];
        }
        else
        {
            if(Vector3.Distance(transform.localPosition, target.localPosition) <= 1)
            {
                target = targetList[Random.Range(0, targetList.Count)];
            }
            else
            {
                Move();
            }
        }
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, 0.5f);
    }

}
