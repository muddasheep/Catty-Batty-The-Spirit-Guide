using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritWayWiser : MonoBehaviour
{
    public SpiritWayWiserArrow arrow_up;
    public SpiritWayWiserArrow arrow_right;
    public SpiritWayWiserArrow arrow_down;
    public SpiritWayWiserArrow arrow_left;

    public Animator animator;

    int current_direction = 0;
    public int override_start_direction = -1;

    private void Start() {
        // start with random direction
        current_direction = Random.Range(0, 3);
        if (override_start_direction != -1) {
            current_direction = override_start_direction;
        }
        change_direction();
    }

    public void change_direction() {
        current_direction++;

        if (current_direction >= 4) {
            current_direction = 0;
        }

        arrow_up.switch_off();
        arrow_right.switch_off();
        arrow_down.switch_off();
        arrow_left.switch_off();

        if (current_direction == 0)
            arrow_up.switch_on();
        if (current_direction == 1)
            arrow_right.switch_on();
        if (current_direction == 2)
            arrow_down.switch_on();
        if (current_direction == 3)
            arrow_left.switch_on();

        animator.StopPlayback();
        animator.Play("spiritwaywiser_wobble", -1, 0f);
    }
}
