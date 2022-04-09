using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sparkles : MonoBehaviour
{
    public Vector3 startpos;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.localPosition;
        StartCoroutine(Sparkle(animator, startpos, transform));
    }


    private static IEnumerator Sparkle(Animator animator, Vector3 startpos, Transform transform) {
        while (true) {
            yield return new WaitForSeconds(0.8f + Random.Range(0, 0.5f));

            transform.localPosition = new Vector3(
                startpos.x - Random.Range(0, 0.5f) + Random.Range(0, 0.5f),
                startpos.y - Random.Range(0, 0.5f) + Random.Range(0, 0.5f),
                startpos.z
            );

            animator.StopPlayback();
            animator.Play("sparkles", -1, 0f);
        }
    }
}
