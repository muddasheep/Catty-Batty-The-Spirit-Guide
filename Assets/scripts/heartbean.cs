using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heartbean : MonoBehaviour
{
    public bool blooming = false;

    public void stopAnimation() {
        gameObject.GetComponent<Animator>().enabled = false;
    }

    public void startAnimation() {
        gameObject.GetComponent<Animator>().enabled = true;
    }

}
