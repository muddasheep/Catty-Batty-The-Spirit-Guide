using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionsSubSliderSelect : MonoBehaviour
{
    public GameObject arrow_left;
    public GameObject arrow_right;
    public string[] available_options;
    public string current_option;
    public int current_option_index;
    public TextMeshPro textmeshpro;

    float last_change_time;

    // Start is called before the first frame update
    void Start() {
        current_option_index = find_current_option_index();
        last_change_time = Time.time;
    }

    public int find_current_option_index() {
        for (int i = 0; i < available_options.Length; i++) {
            if (available_options[i] == current_option) {
                return i;
            }
        }

        return 0;
    }
    public void display_current_option() {
        current_option = available_options[current_option_index];
        transform.parent.BroadcastMessage("ValueChange");
        display_current_option_text();
    }

    public void display_current_option_text() {
        string newtext = I2.Loc.LocalizationManager.GetTranslation(current_option);
        if (newtext == null) {
            newtext = current_option;
        }
        textmeshpro.text = newtext;
    }

    public void Increase() {
        if (Time.time - last_change_time < 0.1f) {
            return;
        }
        last_change_time = Time.time;

        current_option_index++;

        if (current_option_index >= available_options.Length) {
            current_option_index = 0;
        }

        display_current_option();
    }

    public void Decrease(float amount = 20f) {
        last_change_time = Time.time;

        current_option_index -= 1;

        if (current_option_index < 0) {
            current_option_index = available_options.Length - 1;
        }

        display_current_option();
    }
}
