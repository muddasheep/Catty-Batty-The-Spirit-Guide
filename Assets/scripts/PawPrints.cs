using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawPrints : MonoBehaviour
{
    public Sprite[] sprites;
    SpriteRenderer sr;
    public float delay = 0;

    private void Start() {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprites[Random.Range(0, sprites.Length)];

        StartCoroutine(FadeIn(sr, delay));
    }

    private static IEnumerator FadeIn(SpriteRenderer sr, float delay) {
        yield return new WaitForSeconds(delay);

        float alpha = 0;

        while (alpha < 0.5f) {
            sr.color = new Color(255, 255, 255, alpha);
            alpha += 0.1f;
            yield return new WaitForFixedUpdate();
        }
    }
}
