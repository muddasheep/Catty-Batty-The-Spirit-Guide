using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsSubItem : MonoBehaviour
{
    public Sprite sprite_active;
    public Sprite sprite_inactive;
    public bool is_active = false;
    public SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
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

    public void setActive() {
        is_active = true;
        showState();
    }
    public void setInactive() {
        is_active = false;
        showState();
    }
}
