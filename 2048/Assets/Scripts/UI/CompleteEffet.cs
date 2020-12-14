using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 타일 64 완성 시 이펙트 연출
/// </summary>
public class CompleteEffet : MonoBehaviour
{
    public Transform current;
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        current = transform;
        target = Game.instance.timeSlider.transform;

        current.localPosition = Vector3.MoveTowards(current.localPosition, target.localPosition, 1100 * Time.deltaTime);
    }
}
