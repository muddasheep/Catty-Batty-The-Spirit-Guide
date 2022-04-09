using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CompletePopup : MonoBehaviour
{
    public Animator vine_left;
    public Animator vine_right;
    public Animator heart;
    public Animator batty;
    public Animator catty;
    public Animator vine_sideleft;
    public Animator vine_sideright;
    public Animator hedgar;
    public Animator corner_left;
    public Animator corner_right;
    public Animator plant_left;
    public Animator plant_right;
    public TextMeshPro text;
    public Animator continue_button;
    public Animator white;

    public Animator stripe1;
    public Animator stripe2;
    public Animator stripe3;
    public Animator stripe4;
    public Animator stripe5;

    public Animator popup;

    // Start is called before the first frame update
    void Start()
    {
        vine_left.speed = 2f;
        vine_right.speed = 2f;
        vine_sideleft.speed = 2f;
        vine_sideright.speed = 2f;
    }

    public void showPopup(int total_towers_placed, float total_playtime) {
        TimeSpan time = TimeSpan.FromSeconds(total_playtime);
        string str = time.ToString(@"mm\:ss");

        StartCoroutine(ActivateAnimatorDelayed(heart, 0f));
        StartCoroutine(ActivateAnimatorDelayed(vine_left, 0.1f));
        StartCoroutine(ActivateAnimatorDelayed(vine_right, 0.1f));
        StartCoroutine(ActivateAnimatorDelayed(batty, 0.3f));
        StartCoroutine(ActivateAnimatorDelayed(catty, 0.3f));
        StartCoroutine(ActivateAnimatorDelayed(popup, 1f));
        StartCoroutine(ActivateAnimatorDelayed(white, 1f));
        StartCoroutine(ActivateAnimatorDelayed(vine_sideleft, 1f));
        StartCoroutine(ActivateAnimatorDelayed(vine_sideright, 1f));
        StartCoroutine(ActivateAnimatorDelayed(corner_left, 1.4f));
        StartCoroutine(ActivateAnimatorDelayed(corner_right, 1.4f));
        StartCoroutine(ActivateAnimatorDelayed(plant_left, 1.4f));
        StartCoroutine(ActivateAnimatorDelayed(plant_right, 1.4f));
        StartCoroutine(ActivateAnimatorDelayed(hedgar, 1.7f));
        StartCoroutine(ActivateAnimatorDelayed(stripe1, 1.3f));
        StartCoroutine(ActivateAnimatorDelayed(stripe2, 1.4f));
        StartCoroutine(ActivateAnimatorDelayed(stripe3, 1.5f));
        StartCoroutine(ActivateAnimatorDelayed(stripe4, 1.6f));
        StartCoroutine(ActivateAnimatorDelayed(stripe5, 1.7f));
        StartCoroutine(ActivateAnimatorDelayed(continue_button, 1.4f));
        string levelcomplete = I2.Loc.LocalizationManager.GetTranslation("levelcomplete");
        string boxesplaced = I2.Loc.LocalizationManager.GetTranslation("boxesplaced");
        string localizedtime = I2.Loc.LocalizationManager.GetTranslation("localizedtime");

        StartCoroutine(TypeSentence(
            levelcomplete + "\n\n" + boxesplaced + " " +
            total_towers_placed.ToString() +
            "\n" + localizedtime + " " + str, 1.4f
        ));
    }

    private static IEnumerator ActivateAnimatorDelayed(Animator animator, float delay) {

        yield return new WaitForSeconds(delay);

        animator.gameObject.SetActive(true);

        animator.enabled = true;
    }

    IEnumerator TypeSentence(string sentence, float delay) {
        yield return new WaitForSeconds(delay);

        text.gameObject.SetActive(true);
        int letter_count = 1;
        foreach (char letter in sentence.ToCharArray()) {
            string oldtext = sentence.Substring(0, letter_count);
            string newtext = sentence.Substring(letter_count);
            text.text = oldtext + "<color=#FFFFFF>" + newtext + "</color>";
            letter_count++;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
