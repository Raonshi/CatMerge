using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfTime : MonoBehaviour
{
    Rigidbody2D rigidbody = new Rigidbody2D();
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_10Seconds"));
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rigidbody.position.y <= 1280)
        {
            rigidbody.velocity = Vector3.zero;
            return;
        }
        rigidbody.AddForce(new Vector2(0, -8000));

        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
