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
                Color color2;
                ColorUtility.TryParseHtmlString("#FFEA99", out color2);
                bg.color = color2;
                numText.text = string.Format("<color=#FFEA99> {0} </color>", num);
                break;

            case 4:
                Color color3;
                ColorUtility.TryParseHtmlString("#FFCD3F", out color3);
                bg.color = color3;
                numText.text = string.Format("<color=#FFCD3F> {0} </color>", num);
                break;

            case 8:
                Color color4;
                ColorUtility.TryParseHtmlString("#DB9600", out color4);
                bg.color = color4;
                numText.text = string.Format("<color=#DB9600> {0} </color>", num);
                break;

            case 16:
                Color color5;
                ColorUtility.TryParseHtmlString("#CAF562", out color5);
                bg.color = color5;
                numText.text = string.Format("<color=#CAF562> {0} </color>", num);
                break;

            case 32:
                Color color6;
                ColorUtility.TryParseHtmlString("#8ddf01", out color6);
                bg.color = color6;
                numText.text = string.Format("<color=#8ddf01> {0} </color>", num);
                break;
        }

        _xPos = GameObject.Find("Game").GetComponent<Game>().xPos;
        _yPos = GameObject.Find("Game").GetComponent<Game>().yPos;

        //생성 애니메이션 재생
    }

    // Update is called once per frame
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

        Vector3 target = new Vector3((320 * x) - _xPos, (320 * y) - _yPos, 0);
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, 60);
        
        
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
