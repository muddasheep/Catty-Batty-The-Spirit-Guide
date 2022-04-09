using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashTransitionHeartSmall : MonoBehaviour
{
    public SpriteRenderer sr;
    public bool chosen = false;
    public Rigidbody2D rb;
    public Animator ar;

    public void moveit() {
        rb.AddForce(transform.up * Random.Range(9f,12f) * 7f);
        ar.enabled = true;
    }
}
