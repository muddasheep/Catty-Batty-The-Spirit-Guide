using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsDisplay : MonoBehaviour
{
    public Animator[] lines;
    public Animator whitebg;

    public void ShowDisplay() {
        whitebg.GetComponent<SpriteRenderer>().enabled = true;
        whitebg.enabled = true;
        whitebg.StopPlayback();
        whitebg.Play("fade_in", -1, 0);

        foreach (Animator ar in lines) {
            ar.GetComponent<SpriteRenderer>().enabled = true;
            ar.enabled = true;
            ar.StopPlayback();
            ar.Play("spiritwaitingroomdisplayline", -1, 0);
        }
    }

    public void HideDisplay() {
        whitebg.enabled = true;
        whitebg.StopPlayback();
        whitebg.Play("fade_out", -1, 0);

        foreach (Animator ar in lines) {
            ar.enabled = true;
            ar.StopPlayback();
            ar.Play("spiritwaitingroomdisplayline_reverse", -1, 0);
        }
    }
}
