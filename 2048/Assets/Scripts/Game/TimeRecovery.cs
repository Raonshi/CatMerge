using UnityEngine;


/// <summary>
/// 64완성 시 시간이 회복되는 연출을 담당한다.
/// </summary>
public class TimeRecovery : MonoBehaviour
{
    public float destroyTime;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        destroyTime -= Time.deltaTime;

        if(destroyTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
