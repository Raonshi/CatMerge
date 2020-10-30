using System;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Text Text;
    public int num;
    public bool isCombine;

    public int _x;
    public int _y;

    float _xPos, _yPos;

    public bool move, _combine;

    void Start()
    {
        isCombine = false;

        if(gameObject.name == "Joker")
        {
            Text.text = "J";
        }
        else if(gameObject.name == "Multiple")
        {
            Text.text = "M";
        }
        else if(gameObject.name == "Division")
        {
            Text.text = "D";
        }
        else
        {
            Text.text = num.ToString();
        }

        _xPos = GameObject.Find("Game").GetComponent<Game>().xPos;
        _yPos = GameObject.Find("Game").GetComponent<Game>().yPos;

    }

    // Update is called once per frame
    void Update()
    {
        if(move)
        {
            Move(_x, _y, _combine);
        }
    }


    public void Move(int x, int y, bool combine)
    {
        move = true;
        _x = x;
        _y = y;
        _combine = combine;

        Vector3 target = new Vector3((320 * x) - _xPos, (320 * y) - _yPos, 0);
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, 100);
        
        
        if (transform.localPosition == target)
        {
            move = false;

            if (combine == true)
            {
                _combine = false;
                Destroy(gameObject);
            }
        }
    }
}
