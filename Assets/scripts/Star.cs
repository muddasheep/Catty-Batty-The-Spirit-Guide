using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public Animator starnimator;

    // Start is called before the first frame update
    void Start()
    {
        Reposition();
        starnimator.Play("starfall", 0, Random.Range(0, 1f));
    }

    public void Reposition() {
        gameObject.transform.parent.position = new Vector3(
            Random.Range(-11f, 11f),
            Random.Range(-6f, 6f),
            1
        );
    }
}
