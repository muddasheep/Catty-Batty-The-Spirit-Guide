using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetsgoSign : MonoBehaviour
{
    Vector3 startpos;
    public Animator ar;
    public SpriteRenderer sr;

    private void Start() {
        startpos = transform.position;
    }

    private void FixedUpdate() {
        transform.position = new Vector3(
            transform.position.x,
            startpos.y + Mathf.Sin(Time.time * 4f) / 10f,
            transform.position.z
        );

        sr.color = new Color(255f, 255f, 255f, 1f - (1f + Mathf.Sin(Time.time)));
    }

    public void Begone() {
        ar.SetBool("begoned", true);
    }
}
