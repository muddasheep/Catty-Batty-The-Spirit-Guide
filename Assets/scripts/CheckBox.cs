using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBox : MonoBehaviour
{
    public Sprite sprite_checked;
    public Sprite sprite_unchecked;
    public bool is_checked = false;
    public SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        showState();
    }

    public void showState() {
        if (is_checked) {
            sr.sprite = sprite_checked;
        }
        else {
            sr.sprite = sprite_unchecked;
        }
    }

    public void toggleActive() {
        is_checked = !is_checked;
        showState();
    }

    public void setActive() {
        is_checked = true;
        showState();
    }
    
    public void setInactive() {
        is_checked = false;
        showState();
    }
}
