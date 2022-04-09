using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetsGoBig : MonoBehaviour
{
    public Animator[] LettersBatty;
    public Animator[] LettersCatty;

    public Animator catty;
    public Animator batty;
    public Animator background;
    public bool is_catty = false;

    public void LetsGo(string who) {
        if (who == "catty") {
            is_catty = true;
            catty.StopPlayback();
            catty.enabled = true;
            catty.Play("letsgo_catty_in", 0, -1);
            StartCoroutine(FadeInLetters(LettersCatty));
        }
        else {
            is_catty = false;
            batty.StopPlayback();
            batty.enabled = true;
            batty.Play("letsgo_batty_in", 0, -1);
            StartCoroutine(FadeInLetters(LettersBatty));
        }

        background.StopPlayback();
        background.enabled = true;
        background.Play("letsgo_background_in", 0, -1);

        StartCoroutine(LetsGoOut(this));
    }

    IEnumerator LetsGoOut(LetsGoBig self) {
        yield return new WaitForSeconds(1f);

        if (self.is_catty) {
            catty.StopPlayback();
            catty.enabled = true;
            catty.Play("letsgo_catty_out", 0, -1);
        }
        else {
            batty.StopPlayback();
            batty.enabled = true;
            batty.Play("letsgo_batty_out", 0, -1);
        }

        background.StopPlayback();
        background.enabled = true;
        background.Play("letsgo_background_out", 0, -1);
    }

    IEnumerator FadeInLetters(Animator[] letters) {
        foreach (Animator ar in letters) {
            ar.StopPlayback();
            ar.enabled = true;
            ar.Play("letsgo_letter_in", 0, -1);
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1f);

        foreach (Animator ar in letters) {
            ar.StopPlayback();
            ar.Play("letsgo_letter_out", 0, -1);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
