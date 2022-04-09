using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsSubMenu : MonoBehaviour
{
    public Animator[] childanimators;
    public OptionsSubMenuItem[] items;
    public OptionsScreen optionsscreen;
    public int active_item = 0;

    // Start is called before the first frame update
    void Start()
    {
        childanimators = transform.GetComponentsInChildren<Animator>();
        items = transform.GetComponentsInChildren<OptionsSubMenuItem>();
        optionsscreen = transform.GetComponentInParent<OptionsScreen>();
    }

    public void show() {
        foreach (Animator animator in childanimators) {
            animator.enabled = true;
            animator.StopPlayback();
            animator.Play("optionssubmenuitem_fadein", -1, 0f);
        }
        active_item = 0;
        highlight_active_item();
    }

    public Vector3 getActiveItemPos() {
        return items[active_item].transform.position;
    }

    public void SaveAndReturn() {
        foreach (Animator animator in childanimators) {
            animator.enabled = true;
            animator.StopPlayback();
            animator.Play("optionssubmenuitem_fadeout", -1, 0f);
        }
        optionsscreen.exitSubmenu();
        optionsscreen.SaveToObserver();
        optionsscreen.highlight_active_menu_item();

        foreach (OptionsSubMenuItem item in items) {
            item.setInactive();
        }
    }

    public void call_action_of_active_submenu_item() {
        if (items[active_item].sub_item.is_active) {
            items[active_item].action();
        }
    }

    public void next_item() {
        active_item++;
        if (active_item >= items.Length) {
            active_item = 0;
        }

        highlight_active_item();
    }

    public void prev_item() {
        active_item--;
        if (active_item < 0) {
            active_item = items.Length - 1;
        }

        highlight_active_item();
    }

    public void right_item() {
        if (items[active_item].item_type == "button") {
            next_item();
            return;
        }
        items[active_item].actionRight();
    }

    public void left_item() {
        if (items[active_item].item_type == "button") {
            prev_item();
            return;
        }
        items[active_item].actionLeft();
    }

    public void highlight_active_item() {
        foreach (OptionsSubMenuItem item in items) {
            item.setInactive();
        }

        items[active_item].setActive();
    }

    public void set_active_item(OptionsSubMenuItem given_item) {
        int item_index = 0;
        foreach (OptionsSubMenuItem item in items) {
            if (item == given_item) {
                active_item = item_index;
            }
            else {
                item.setInactive();
            }
            item_index++;
        }
    }
}
