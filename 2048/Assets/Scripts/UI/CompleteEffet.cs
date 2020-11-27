using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
