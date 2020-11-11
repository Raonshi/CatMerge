using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    Slider timer;

    public GameObject Icon;
    Animator anim;
    private void Awake()
    {
        timer = gameObject.GetComponent<Slider>();
        anim = Icon.GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timer.value > 10)
        {
            anim.SetBool("Hurry", false);
        }
        else if(timer.value <= 10)
        {
            anim.SetBool("Hurry", true);
        }
    }
}
