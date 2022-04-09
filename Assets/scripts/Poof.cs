using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poof : MonoBehaviour
{
    public SpriteRenderer sr;
    public Animator ar;
    public bool done = false;
    public Gamemaster gm;

    public void GoPoof(int sort_order) {
        done = false;
        sr.enabled = true;
        ar.enabled = true;
        ar.StopPlayback();
        ar.Play("poof", -1, 0);
        sr.sortingOrder = sort_order;

        StartCoroutine(DoneSoon(this));
    }

    IEnumerator DoneSoon(Poof me) {
        yield return new WaitForSeconds(0.5f);
        me.done = true;
        me.gm.poofpool.Add(this);
    }
}
