using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreditSentence : MonoBehaviour
{
    public TextMeshPro dialogueText;
    public CreditsDisplayText header;
    public CreditsDisplayText content;
    public CreditsDisplayText content_patrons;
    public CreditsDisplayText the_end;
    public Animator whitebeforeend;

    // Start is called before the first frame update
    void Start()
    {
        dialogueText = GetComponent<TextMeshPro>();
        StartCoroutine(TypeSentence(dialogueText.text, this));
    }

    IEnumerator TypeSentence(string sentence, CreditSentence self) {
        dialogueText.text = "";
        yield return new WaitForSeconds(1f);
        CreditsDisplay credits_display = FindObjectOfType<CreditsDisplay>();

        credits_display.ShowDisplay();
        yield return new WaitForSeconds(1f);

        string musicartcode = I2.Loc.LocalizationManager.GetTranslation("credits_musicartcode");

        header.DisplayText(musicartcode);
        content.DisplayText("Philipp \"muddasheep\" Lehner");
        yield return new WaitForSeconds(5f);
        header.HideText();
        content.HideText();
        yield return new WaitForSeconds(2f);

        string executiveproducers = I2.Loc.LocalizationManager.GetTranslation("credits_executeive");
        header.DisplayText(executiveproducers);
        content.DisplayText("Auri, Yuki & Yaki");
        yield return new WaitForSeconds(5f);
        header.HideText();
        content.HideText();
        yield return new WaitForSeconds(2f);

        string betatesters = I2.Loc.LocalizationManager.GetTranslation("credits_betatesters");
        header.DisplayText(betatesters);
        content.DisplayText("Will Lewis, Nungy, KeshaFilm, Emily Rose, H4ndy");
        yield return new WaitForSeconds(6f);
        content.HideText();
        yield return new WaitForSeconds(3f);
        content.DisplayText("Alexis Clay, Barion, SaimizZ, s. Jane Mills");
        yield return new WaitForSeconds(6f);
        header.HideText();
        content.HideText();
        yield return new WaitForSeconds(2f);

        string additionalthanks = I2.Loc.LocalizationManager.GetTranslation("credits_additionalthanks");
        header.DisplayText(additionalthanks);
        content.DisplayText("Djigallag, Jancias, Michael Grankin, Denis Osipov, Brackeys, Zen");
        yield return new WaitForSeconds(6f);
        content.HideText();
        yield return new WaitForSeconds(3f);
        content.DisplayText("MGC, Mr. E, Nana, Nome, Toyoch, Gabriel Pereira, Carolina Actis");
        yield return new WaitForSeconds(6f);
        content.HideText();
        yield return new WaitForSeconds(3f);
        string mypatrons = I2.Loc.LocalizationManager.GetTranslation("credits_mypatrons");
        content_patrons.DisplayText(mypatrons);
        yield return new WaitForSeconds(6f);
        header.HideText();
        content_patrons.HideText();
        yield return new WaitForSeconds(3f);

        credits_display.HideDisplay();

        int letter_count = 1;
        foreach (char letter in sentence.ToCharArray()) {
            string oldtext = sentence.Substring(0, letter_count);
            string newtext = sentence.Substring(letter_count);
            dialogueText.text = oldtext + "<color=#FFFFFF>" + newtext + "</color>";
            letter_count++;
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(5f);

        whitebeforeend.enabled = true;

        yield return new WaitForSeconds(1f);

        Gamemaster gm = FindObjectOfType<Gamemaster>();
        if (gm.is_paused) {
            if (gm.is_in_options) {
                if (gm.optionsscreen.in_submenu) {
                    gm.optionsscreen.fireCancel();
                }
                gm.closeOptions();
            }
        }

        player_movement[] players = FindObjectsOfType<player_movement>();
        foreach (player_movement player in players) {
            player.destroyInputEvents();
        }

        whitebeforeend.enabled = true;

        yield return new WaitForSeconds(1f);

        if (gm.is_paused) {
            gm.unpauseGame();
        }

        AchievementsScreen acheevos = FindObjectOfType<AchievementsScreen>();
        acheevos.ShowAchievements();

        yield return new WaitForSeconds(4f);
        string theend = I2.Loc.LocalizationManager.GetTranslation("credits_theend");
        the_end.DisplayText(theend);
        yield return new WaitForSeconds(2f);

        while (!Input.anyKeyDown) {
            yield return null;
        }
        the_end.DisplayText("");
        yield return new WaitForFixedUpdate();

        acheevos.HideAchievements();

        gm.set_high_score();
        gm.return_to_map();
    }
}
