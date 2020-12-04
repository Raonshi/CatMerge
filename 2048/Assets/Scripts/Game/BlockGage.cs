using UnityEngine;
using UnityEngine.UI;


public class BlockGage : MonoBehaviour
{
    Animator anim;
    Image image;
    public float time;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        image = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Singleton.difficulty == GameManager.Difficulty.Easy || Game.instance.blockCount >= 4)
        {
            time = 0;
        }
        else
        {
            time = Game.instance.hardTime;
            anim.SetFloat("Time", time);
        }

        image.fillAmount = time * 0.05f;
    }
}
