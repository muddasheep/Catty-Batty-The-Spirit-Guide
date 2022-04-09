using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursorScript : MonoBehaviour {
    private Animator[] animators;
    private SpriteRenderer[] spriterenderers;

    public Sprite[] styles;

    public int current_style = 0;

    // Start is called before the first frame update
    void Awake() {
        animators = gameObject.GetComponentsInChildren<Animator>();
        spriterenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
        applyStyle();
    }

    public void playCursorAnimation() {
        foreach (Animator cursor in animators) {
            cursor.StopPlayback();
            cursor.Play("cursor_animation", -1, 0f);
        }
    }

    public void applyStyle() {
        foreach (SpriteRenderer spriterenderer in spriterenderers) {
            spriterenderer.sprite = styles[current_style];
        }
    }

    public void toggleStyle() {
        current_style++;

        if(current_style >= styles.Length) {
            current_style = 0;
        }

        applyStyle();
    }
}
