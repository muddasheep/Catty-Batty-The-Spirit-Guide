using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreditsDisplayText : MonoBehaviour
{
    public TextMeshPro text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
    }

    public void DisplayText(string text) {
        StartCoroutine(TypeSentence(text));
    }

    IEnumerator TypeSentence(string sentence) {
        text.text = "";

        int letter_count = 1;
        foreach (char letter in sentence.ToCharArray()) {
            string oldtext = sentence.Substring(0, letter_count);
            string newtext = sentence.Substring(letter_count);
            text.text = oldtext;
            if (newtext.Length > 0) {
                text.text = text.text + "<color=#FFFFFF>" + newtext + "</color>";
            }
            letter_count++;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void HideText() {
        StartCoroutine(RemoveSentence());
    }

    IEnumerator RemoveSentence() {
        string sentence = text.text;

        int letter_count = 1;
        foreach (char letter in sentence.ToCharArray()) {
            string oldtext = sentence.Substring(0, letter_count);
            string newtext = sentence.Substring(letter_count);
            text.text = "<color=#FFFFFF>" + oldtext + "</color>" + newtext;
            letter_count++;
            yield return new WaitForSeconds(0.03f);
        }
        text.text = "";
    }

}
