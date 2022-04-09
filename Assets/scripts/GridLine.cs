using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLine : MonoBehaviour
{
    public Animator ar;
    public Gamemaster gm;
    public bool done = false;

    public void MoveIn() {
        ar.enabled = true;
        ar.StopPlayback();
        ar.Play("gridline_in", -1, 0);
        StartCoroutine(DoneSoon(this));
    }

    IEnumerator DoneSoon(GridLine me) {
        yield return new WaitForSeconds(0.5f);
        me.done = true;
        me.gm.gridlinepool.Add(this);
    }
}
