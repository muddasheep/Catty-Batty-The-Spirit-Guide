using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadgarTreeWaiting : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer sr;
    public Sprite happysprite;

    public void happy() {
        animator.enabled = false;
        sr.sprite = happysprite;
        sr.sortingOrder = 24;
        HedgarGlasses glasses = FindObjectOfType<HedgarGlasses>();
        glasses.GetComponent<SpriteRenderer>().enabled = false;
        glasses.done = true;
    }
}
