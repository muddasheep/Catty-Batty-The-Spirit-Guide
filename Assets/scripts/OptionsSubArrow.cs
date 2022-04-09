using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsSubArrow : MonoBehaviour
{
    public Sprite sprite_active;
    public Sprite sprite_inactive;
    public bool is_active = false;
    public SpriteRenderer sr;
    public string BroadcastActionForParent;

    // Start is called before the first frame update
    void Start() {
        showState();
    }

    public void showState() {
        if (is_active) {
            sr.sprite = sprite_active;
        }
        else {
            sr.sprite = sprite_inactive;
        }
    }

    public void setArrowActive() {
        is_active = true;
        showState();
    }

    public void setArrowInactive() {
        is_active = false;
        showState();
    }

    private void OnMouseDown() {
        if (BroadcastActionForParent == "Decrease") {
            transform.parent.BroadcastMessage(BroadcastActionForParent, 40f);
        }
    }

    private void OnMouseEnter() {
        setArrowActive();
        transform.parent.parent.BroadcastMessage("setActive");
    }

    private void OnMouseExit() {
        setArrowInactive();
    }
}
