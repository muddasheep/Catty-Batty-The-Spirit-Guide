using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionsSubBox : MonoBehaviour
{
    public Sprite sprite_active;
    public Sprite sprite_inactive;
    public bool is_active = false;
    public SpriteRenderer sr;
    public TextMeshPro tmpro;

    // Start is called before the first frame update
    void Start() {
        showState();
    }

    public void showState() {
        if (is_active) {
            sr.sprite = sprite_active;
            tmpro.color = new Color(255, 255, 255);
        }
        else {
            sr.sprite = sprite_inactive;
            tmpro.color = new Color(0, 0, 0);
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

    private void OnMouseEnter() {
        setActive();
    }

    private void OnMouseExit() {
        setInactive();
    }
}
