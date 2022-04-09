using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionGradient : MonoBehaviour
{
    public Animator ar;

    void Awake() {
        TransitionGradient[] masters = GameObject.FindObjectsOfType<TransitionGradient>();

        if (masters.Length > 1) {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void FadeIn() {
        ar.enabled = true;
        ar.StopPlayback();
        ar.Play("whitegradient_in", -1, 0);
    }
    public void FadeOut() {
        ar.enabled = true;
        ar.StopPlayback();
        ar.Play("whitegradient_out", -1, 0);
    }
}
