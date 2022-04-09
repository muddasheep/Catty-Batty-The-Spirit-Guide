using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashTitle : MonoBehaviour
{
    public float delay;
    public SpriteRenderer sr;
    public Animator ar;

    public void ActivateMe() {
        StartCoroutine(Activator(sr, ar, delay / 10f));
    }

    private static IEnumerator Activator(SpriteRenderer sr, Animator ar, float delay) {
        float start = Time.time;
        while (Time.time <= start + delay) {
            yield return null;
        }
        sr.enabled = true;
        ar.enabled = true;

        while (true) {
            yield return new WaitForSeconds(2f);
            ar.StopPlayback();
            ar.Play("splash_title_hover", -1, 0);
        }
    }
}
