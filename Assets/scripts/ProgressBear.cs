using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBear : MonoBehaviour
{
    public Sprite[] sprites;
    public SpriteRenderer sr;
    public Animator appear_ar;
    public Animator ar;
    public int current_sprite = 0;
    bool hidden = false;

    public void Start() {
        Hide();
    }

    public void DisplayPercent(float percent) {
        if (hidden == true) {
            hidden = false;
            appear_ar.enabled = true;
            appear_ar.StopPlayback();
            appear_ar.Play("progressappear", -1, 0);
        }
        sr.enabled = true;
        int current_frame = Mathf.RoundToInt(sprites.Length * percent / 100f);
        current_frame = Mathf.Clamp(current_frame, 0, sprites.Length - 1);
        sr.sprite = sprites[current_frame];
    }

    public void Done() {
        ar.enabled = true;
    }

    public void Hide() {
        if (hidden == false) {
            hidden = true;
            appear_ar.enabled = true;
            appear_ar.StopPlayback();
            appear_ar.Play("progressdisappear", -1, 0);
        }
    }
}
