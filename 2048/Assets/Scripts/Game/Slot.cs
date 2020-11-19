using System;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image bg;
    public Image image;
    public GameObject numImage;
    public Text numText;
    
    public int num;

    public bool isCombine, isNew, isBomb, isNum;
    public Animator anim;

    public int _x;
    public int _y;

    float _xPos, _yPos;

    public bool move, _combine;

    private void Awake()
    {
        isCombine = false;
        if(Game.instance.numEnable.GetComponent<Toggle>().isOn == true)
        {
            isNum = false;
        }
        else
        {
            isNum = true;
        }
        anim = gameObject.GetComponent<Animator>();
    }
    void Start()
    {
        switch (num)
        {
            case 1:
                Color color1;
                if (gameObject.name == "Multiple")
                {
                    ColorUtility.TryParseHtmlString("#9CF6FF", out color1);
                    bg.color = color1;

                    numText.text = string.Format("<color=#9CF6FF>×2</color>");
                }
                else if(gameObject.name == "Division")
                {
                    ColorUtility.TryParseHtmlString("#D43726", out color1);
                    bg.color = color1;

                    numText.text = string.Format("<color=#D43726>÷2</color>");
                }
                break;

            case 2:
                numText.text = string.Format("<color=#C0C2FE> {0} </color>", num);
                break;

            case 4:
                numText.text = string.Format("<color=#888BFA> {0} </color>", num);
                break;

            case 8:
                numText.text = string.Format("<color=#464AD4> {0} </color>", num);
                break;

            case 16:
                numText.text = string.Format("<color=#CAF562> {0} </color>", num);
                break;

            case 32:
                numText.text = string.Format("<color=#8ddf01> {0} </color>", num);
                break;
        }

        _xPos = GameObject.Find("Game").GetComponent<Game>().xPos;
        _yPos = GameObject.Find("Game").GetComponent<Game>().yPos;
    }

    void Update()
    {
        if(isNum == true)
        {
            numImage.SetActive(true);
        }
        else
        {
            numImage.SetActive(false);
        }


        if (move)
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

        Vector3 target = new Vector3((270 * x) - _xPos, (270 * y) - _yPos, 0);
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, 5000 * Time.deltaTime);
        
        
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
