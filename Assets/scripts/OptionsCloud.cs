using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OptionsCloud : MonoBehaviour
{
    public float speed = 1f;
    public float x;

    // Start is called before the first frame update
    void Start()
    {
        x = transform.localPosition.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (x > 14f) {
            x = -18f;
        }

        x += speed;

        transform.localPosition = new Vector3(
            x,
            transform.localPosition.y,
            transform.localPosition.z
        );
    }
}
