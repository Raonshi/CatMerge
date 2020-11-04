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
        switch(num)
        {
            case 1:
                if(gameObject.name == "Multiple")
                {
                    bg.color = new Color(153 / 255f, 255 / 255f, 153 / 255f);
                    numText.text = string.Format("<color=purple>×2</color>");
                }
                else if(gameObject.name == "Division")
                {
                    bg.color = new Color(255 / 255f, 51 / 255f, 0 / 255f);
                    numText.text = string.Format("<color=red>÷2</color>");
                }
                break;

            case 2:
                bg.color = new Color(204 / 255f, 102 / 255f, 0 / 255f);
                numText.text = string.Format("<color=black> {0} </color>", num);
                break;

            case 4:
                bg.color = new Color(255 / 255f, 153 / 255f, 51 / 255f);
                numText.text = string.Format("<color=blue> {0} </color>", num);
                break;

            case 8:
                bg.color = new Color(255 / 255f, 153 / 255f, 102 / 255f);
                numText.text = string.Format("<color=green> {0} </color>", num);
                break;

            case 16:
                bg.color = new Color(255 / 255f, 153 / 255f, 153 / 255f);
                numText.text = string.Format("<color=orange> {0} </color>", num);
                break;

            case 32:
                bg.color = new Color(255 / 255f, 153 / 255f, 204 / 255f);
                numText.text = string.Format("<color=yellow> {0} </color>", num);
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
