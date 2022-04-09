using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speck_of_dust : MonoBehaviour
{
    public void rotate() {
        gameObject.transform.parent.Rotate(0, 0, Random.Range(0, 360f));
        float scaley = 0.5f + Random.Range(0f, 0.5f);
        gameObject.transform.localScale = new Vector3(scaley, scaley, 1f);
        gameObject.GetComponent<Animator>().enabled = false;
        StartCoroutine(restart_with_delay());
    }

    public void restart() {
    }

    IEnumerator restart_with_delay() {
        yield return new WaitForSeconds(Random.Range(0f, 2.5f));
        gameObject.GetComponent<Animator>().enabled = true;
    }
}
