using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BurgundyHelp : MonoBehaviour
{
    public TextMeshPro text;
    public Animator speechbub;
    public SpriteRenderer speechbubbly_burgundy;
    string old_text;
    float start_y;

    void Start() {
        old_text = text.text;
        start_y = speechbubbly_burgundy.transform.localPosition.y;
    }

    void FixedUpdate() {
        speechbubbly_burgundy.transform.localPosition = new Vector3(
            speechbubbly_burgundy.transform.localPosition.x,
            start_y + Mathf.Sin(Time.time * 8f) / 5f,
            speechbubbly_burgundy.transform.localPosition.z
        );
    }

    public void DisplayHelp(Gamemaster gm) {
        StartCoroutine(CheckIfHelpNeeded(this, gm));
    }

    IEnumerator CheckIfHelpNeeded(BurgundyHelp self, Gamemaster gm) {
        yield return new WaitForSeconds(0.5f);
        if (gm.spirits_active) {
            yield break;
        }

        text.enabled = true;
        speechbub.enabled = true;
        self.speechbub.StopPlayback();
        self.speechbub.Play("burgundyhelp_in", -1, 0);
        StartCoroutine(TypeSentence(old_text));
    }

    IEnumerator TypeSentence(string sentence) {
        text.text = "";
        yield return new WaitForSeconds(0.5f);
        int letter_count = 1;
        bool in_brackets = false;
        foreach (char letter in sentence.ToCharArray()) {
            if (letter.ToString() == "<") {
                in_brackets = true;
            }
            if (letter.ToString() == ">") {
                in_brackets = false;
            }
            text.text = text.text + letter;
            letter_count++;
            if (in_brackets) {
                continue;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void GoodbyeBurgundy() {
        StopAllCoroutines();
        text.text = "";
        speechbub.StopPlayback();
        speechbub.Play("burgundyhelp_out", -1, 0);
        text.enabled = false;
    }

    void OnDestroy() {
        StopAllCoroutines();
    }

}
