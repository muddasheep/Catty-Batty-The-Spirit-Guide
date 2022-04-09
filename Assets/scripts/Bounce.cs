using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public float myoffset = 0;
    public float start_y;
    public float divider = 3f;

    private void Start() {
        start_y = transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(
            transform.position.x,
            start_y + Mathf.Sin(Time.time + myoffset) / divider,
            transform.position.z
        );
    }
}
