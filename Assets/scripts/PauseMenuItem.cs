using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenuItem : MonoBehaviour
{
    public Animator animator;
    public bool hover = false;
    public TextMeshPro item_text;
    public string item_display_text;
    public string item_action_name;
    public PauseScreen pausescreen;
    public OptionsScreen optionsscreen;
    public bool moving_to_top = false;
    public bool moving_to_left = false;
    public bool moving_back = false;
    public Vector3 start_pos;
    Vector3 left_pos;
    Vector3 top_pos;
    Vector3 start_moving_pos;
    float start_moving;
    float moving_distance;

    private void Start() {
        start_pos = transform.localPosition;
        left_pos = new Vector3(
            start_pos.x - 5f,
            start_pos.y,
            start_pos.z
        );
        top_pos = new Vector3(
            start_pos.x,
            2f,
            start_pos.z
        );
    }

    public void UpdateText() {
        item_text.SetText(item_display_text);
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (hover && animator.GetBool("hover") != true) {
            hover_on();
        }

        if (!hover && animator.GetBool("hover") == true) {
            hover_off();
        }

        if (moving_to_left) {
            float distanceCovered = (Time.time - start_moving) * 10f;
            float fractionOfJourney = distanceCovered / moving_distance;
            transform.localPosition = Vector3.Lerp(
                start_moving_pos, left_pos, fractionOfJourney
            );
        }

        if (moving_to_top) {
            float distanceCovered = (Time.time - start_moving) * 10f;
            float fractionOfJourney = distanceCovered / moving_distance;
            transform.localPosition = Vector3.Lerp(
                start_moving_pos, top_pos, fractionOfJourney
            );
        }

        if (moving_back) {
            float distanceCovered = (Time.time - start_moving) * 10f;
            float fractionOfJourney = distanceCovered / moving_distance;
            transform.localPosition = Vector3.Lerp(
                start_moving_pos, start_pos, fractionOfJourney
            );
        }
    }

    public void hover_on() {
        hover = true;
        animator.SetBool("hover", true);
        item_text.color = new Color(255, 255, 255);
    }

    public void hover_off() {
        hover = false;
        animator.SetBool("hover", false);
        item_text.color = new Color(0, 0, 0);
    }

    public void call_action() {
        if (pausescreen != null) {
            pausescreen.BroadcastMessage("action_" + item_action_name);
        }

        if (optionsscreen != null) {
            optionsscreen.BroadcastMessage("action_" + item_action_name);
        }
    }

    public void move_left() {
        moving_to_left = true;
        moving_to_top = false;
        moving_back = false;
        start_moving = Time.time;
        start_moving_pos = transform.localPosition;
        moving_distance = Vector3.Distance(start_moving_pos, left_pos);
    }

    public void move_top() {
        moving_to_left = false;
        moving_to_top = true;
        moving_back = false;
        start_moving = Time.time;
        start_moving_pos = transform.localPosition;
        moving_distance = Vector3.Distance(start_moving_pos, top_pos);
    }

    public void move_back() {
        moving_to_left = false;
        moving_to_top = false;
        moving_back = true;
        start_moving = Time.time;
        start_moving_pos = transform.localPosition;
        moving_distance = Vector3.Distance(start_moving_pos, start_pos);
    }
}
