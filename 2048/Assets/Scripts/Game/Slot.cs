using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 게임 애네서 생성되는 타일마다 개별적으로 연결된다.
/// </summary>
public class Slot : MonoBehaviour
{
    public Image bg;
    public Image image;
    public GameObject numImage;
    public Text numText;
    
    public int num;

    public bool isCombine, isNew, isNum;
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

        _xPos = Game.instance.xPos;
        _yPos = Game.instance.yPos;
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

    /// <summary>
    /// 타일을 목표지점까지 이동시킨다. 결합일 경우 목표지점에 도달하면 타일을 제거한다.
    /// </summary>
    /// <param name="x">목표지점의 x좌표</param>
    /// <param name="y">목표지점의 y좌표</param>
    /// <param name="combine">결합 여부</param>
    public void Move(int x, int y, bool combine)
    {
        move = true;
        _x = x;
        _y = y;
        _combine = combine;

        Vector3 target = new Vector3((Game.instance.slotGap * x) - _xPos, (Game.instance.slotGap * y) - _yPos, 0);
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
