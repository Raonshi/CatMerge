using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 게임 내의 제한시간이 흐르는 연출을 담당한다.
/// </summary>
public class Timer : MonoBehaviour
{
    Slider timer;
    Animator anim;

    public GameObject Icon;

    float time = 1.0f;

    private void Awake()
    {
        timer = gameObject.GetComponent<Slider>();
        anim = Icon.GetComponent<Animator>();
    }


    void Update()
    {
        time -= Time.deltaTime;

        if(timer.value > 10)
        {
            anim.SetBool("Hurry", false);
        }
        else if(timer.value <= 10)
        {
            if(time <= 0 && timer.value > 0)
            {
                SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_10Seconds"));
                time = 1.0f;
            }

            anim.SetBool("Hurry", true);
        }
    }
}
