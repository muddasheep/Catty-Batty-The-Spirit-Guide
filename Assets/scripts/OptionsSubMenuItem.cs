using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionsSubMenuItem : MonoBehaviour
{
    public TextMeshPro textmeshpro;
    public string menu_text;
    public string property_name;
    public int property_value;
    public string item_type;
    public OptionsSubItem sub_item;
    public OptionsSubMenu submenu;
    public string update_function;
    public UniverseMaster universemaster;

    // types
    public CheckBox checkbox;
    public OptionsSubSlider slider;
    public OptionsSubSliderSelect sliderselect;
    public OptionsSubBox button;

    public HoverItem hover_item;

    public void Initialize() {
        submenu = transform.GetComponentInParent<OptionsSubMenu>();

        if (item_type == "checkbox") {
            prepare_checkbox();
        }

        if (item_type == "slider") {
            prepare_slider();
        }

        if (item_type == "sliderselect") {
            prepare_sliderselect();
        }

        if (item_type == "button") {
            prepare_button();
        }

        hover_item = GetComponentInChildren<HoverItem>();
    }

    void prepare_checkbox() {
        Destroy(slider.gameObject);
        Destroy(sliderselect.gameObject);
        Destroy(button.gameObject);
        checkbox.is_checked = property_value == 1;
        checkbox.showState();
    }

    void prepare_slider() {
        Destroy(checkbox.gameObject);
        Destroy(sliderselect.gameObject);
        Destroy(button.gameObject);
        slider.percent_complete = property_value;
        slider.displayPercent();
    }
    void prepare_sliderselect() {
        Destroy(checkbox.gameObject);
        Destroy(slider.gameObject);
        Destroy(button.gameObject);
        sliderselect.current_option_index = property_value;
        sliderselect.display_current_option();
    }
    void prepare_button() {
        Destroy(checkbox.gameObject);
        Destroy(slider.gameObject);
        Destroy(sliderselect.gameObject);
        textmeshpro.enabled = false;
    }

    public void setActive() {
        submenu.set_active_item(this);
        sub_item.setActive();

        MusicInstructor instructor = FindObjectOfType<MusicInstructor>();
        instructor.soundman.play_sound("select");

        if (item_type == "button") {
            button.setActive();
        }

        if (hover_item != null) {
            hover_item.Show();
        }
    }

    public void setInactive() {
        sub_item.setInactive();

        if (item_type == "button") {
            button.setInactive();
        }

        if (hover_item != null) {
            hover_item.Hide();
        }
    }

    public void action() {
        if (item_type == "checkbox") {
            checkbox_action();
        }
        if (item_type == "sliderselect") {
            sliderselect_action();
        }
        if (item_type == "slider") {
            slider_action();
        }
        if (item_type == "button") {
            button_action();
        }
    }

    void checkbox_action() {
        checkbox.toggleActive();
        if (checkbox.is_checked) {
            property_value = 1;
        }
        else {
            property_value = 0;
        }
        ValueChange();
    }

    void sliderselect_action() {
        sliderselect.Increase();
        property_value = sliderselect.current_option_index;
    }

    void slider_action() {
        slider.Increase();
        property_value = Mathf.RoundToInt(slider.percent_complete);
    }

    void button_action() {
        submenu.Invoke(property_name, 0f);
    }

    public void actionRight() {
        if (item_type == "checkbox") {
            action();
        }
        if (item_type == "slider") {
            action();
            property_value = Mathf.RoundToInt(slider.percent_complete);
        }
        if (item_type == "sliderselect") {
            sliderselect.Increase();
            property_value = sliderselect.current_option_index;
        }
    }

    public void actionLeft() {
        if (item_type == "checkbox") {
            action();
        }
        if (item_type == "slider") {
            slider.Decrease();
            property_value = Mathf.RoundToInt(slider.percent_complete);
        }
        if (item_type == "sliderselect") {
            sliderselect.Decrease();
            property_value = sliderselect.current_option_index;
        }
    }

    public void ValueChange() {
        if (item_type == "checkbox") {
        }
        if (item_type == "slider") {
            property_value = Mathf.RoundToInt(slider.percent_complete);
        }
        if (item_type == "sliderselect") {
            property_value = sliderselect.current_option_index;
        }
        if (universemaster != null) {
            universemaster.observer.AddOption(new OptionSetting(property_name, property_value));
        }
        if (update_function != "" && update_function != null) {
            transform.parent.parent.parent.BroadcastMessage(update_function);
        }
    }

    private void OnMouseEnter() {
        setActive();
    }

    private void OnMouseExit() {
        setInactive();
    }

    private void OnMouseDown() {
        //action();
    }
}
