using UnityEngine;

/// <summary>
/// 남은 시간이 10초 이하일 경우 타일의 테두리를 붉은색으로 점멸한다.
/// </summary>
public class TileImage : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Time", Game.instance.time);
    }
}
