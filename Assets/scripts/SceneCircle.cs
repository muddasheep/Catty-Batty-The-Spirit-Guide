using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCircle : MonoBehaviour
{
    public float delay = 0;
    public Animator animator;
    public bool left = false;
    float rotateamount = 0;

    bool fading_in = false;

    public void FallIn() {
        StartCoroutine(Fall(animator, this, delay));
    }

    public void FallOut() {
        StartCoroutine(FallBack(animator, this, delay));
    }

    private static IEnumerator Fall(Animator animator, SceneCircle self, float delay = 0F) {
        float start = Time.time;
        while (Time.time <= start + delay) {
            yield return null;
        }
        self.fading_in = true;
        animator.enabled = true;
        animator.StopPlayback();
        animator.Play("scenecircle_fallin", -1, 0.7f - delay);

        start = Time.time;
        while (Time.time <= start + delay + 1f) {
            yield return null;
        }
    }

    private static IEnumerator FallBack(Animator animator, SceneCircle self, float delay = 0F) {
        float start = Time.time;
        self.rotateamount = 0;

        while (Time.time <= start + delay) {
            yield return null;
        }

        animator.StopPlayback();
        animator.Play("scenecircle_fallout", -1, 0);

        self.fading_in = false;
    }


    public void ScaleIn() {
        StartCoroutine(Inflate(animator, this, delay));
        rotateamount = 0;
        rotateamount = 0.1f + delay / 10f;
        if (left) {
            rotateamount = 0 - rotateamount;
        }

        transform.Rotate(0, 0, Random.Range(0, 90f), Space.Self);
    }

    public void ScaleOut() {
        StartCoroutine(Deflate(animator, this, delay));
    }

    private static IEnumerator Inflate(Animator animator, SceneCircle self, float delay = 0F) {
        float start = Time.time;
        while (Time.time <= start + delay) {
            yield return null;
        }
        self.fading_in = true;
        animator.enabled = true;
        animator.StopPlayback();
        animator.Play("scenecircle_scalein", -1, 0);

        start = Time.time;
        while (Time.time <= start + delay + 1f) {
            yield return null;
        }
    }

    private static IEnumerator Deflate(Animator animator, SceneCircle self, float delay = 0F) {
        float start = Time.time;
        self.rotateamount = 0;

        while (Time.time <= start + delay) {
            yield return null;
        }

        animator.StopPlayback();
        animator.Play("scenecircle_scaleout", -1, 0);

        self.fading_in = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0, 0, rotateamount, Space.Self);

        if (fading_in == true) {
            if (left) {
                if (rotateamount < 0) {
                    rotateamount += 0.002f;
                    if (rotateamount > 0) {
                        rotateamount = 0;
                    }
                }
            }
            else {
                if (rotateamount > 0) {
                    rotateamount -= 0.002f;
                    if (rotateamount < 0) {
                        rotateamount = 0;
                    }
                }
            }
        }
    }
}
