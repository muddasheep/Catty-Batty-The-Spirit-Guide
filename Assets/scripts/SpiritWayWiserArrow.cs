using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritWayWiserArrow : MonoBehaviour
{
    public Animator animator;
    public bool on = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (on && animator.GetBool("on") != true) {
            animator.SetBool("on", true);
        }

        if (!on && animator.GetBool("on") == true) {
            animator.SetBool("on", false);
        }
    }

    public void switch_on() {
        on = true;
        animator.SetBool("on", true);
    }

    public void switch_off() {
        on = false;
        animator.SetBool("on", false);
    }
}
