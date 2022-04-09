using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heartcrank : MonoBehaviour {

    heartcrankhandle_arm[] arms;
    public GameObject heartcrank_sprite;
    public GameObject target_heartvine;
    public float max_movement;
    public float movement_modifier = 1f;
    public bool move_vertical = true;
    public bool move_opposite_direction = false;
    Vector3 init_heartvine_position;
    float current_movement_speed = 0f;

    private void Start() {
        arms = GetComponentsInChildren<heartcrankhandle_arm>();
        init_heartvine_position = target_heartvine.transform.position;

        if (move_opposite_direction) {
            if (move_vertical) {
                init_heartvine_position = new Vector3(
                    init_heartvine_position.x,
                    init_heartvine_position.y + max_movement,
                    init_heartvine_position.z
                );
            }
            else {
                init_heartvine_position = new Vector3(
                    init_heartvine_position.x + max_movement,
                    init_heartvine_position.y,
                    init_heartvine_position.z
                );
            }
        }
    }

    private void FixedUpdate() {
        float current_rotation = get_current_rotation();
        heartcrank_sprite.transform.Rotate(0, 0, current_rotation);

        current_movement_speed += current_rotation / 1000f;
        current_movement_speed = Mathf.Clamp(current_movement_speed, -0.1f, 0.1f);
        current_movement_speed *= movement_modifier;
        if (current_rotation == 0) {
            if (current_movement_speed > 0) {
                current_movement_speed -= 0.01f;
                if (current_movement_speed < 0.01f)
                    current_movement_speed = 0;
            }
            if (current_movement_speed < 0) {
                current_movement_speed += 0.01f;
                if (current_movement_speed > -0.01f)
                    current_movement_speed = 0;
            }
        }

        if (find_occupied_arm() == null) {
            current_rotation = 0;
        }

        if (move_vertical) {
            float check_new_y = target_heartvine.transform.position.y;
            float new_y = target_heartvine.transform.position.y + current_movement_speed;

            new_y = Mathf.Clamp(
                new_y,
                init_heartvine_position.y - max_movement,
                init_heartvine_position.y
            );
            if (check_new_y == new_y) {
                current_movement_speed = 0;
            }

            target_heartvine.transform.position = new Vector3(
                target_heartvine.transform.position.x,
                new_y,
                target_heartvine.transform.position.z
            );
        }
        else {
            float check_new_x = target_heartvine.transform.position.x;
            float new_x = target_heartvine.transform.position.x + current_movement_speed;

            new_x = Mathf.Clamp(
                new_x,
                init_heartvine_position.x - max_movement,
                init_heartvine_position.x
            );
            if (check_new_x == new_x) {
                current_movement_speed = 0;
            }

            target_heartvine.transform.position = new Vector3(
                new_x,
                target_heartvine.transform.position.y,
                target_heartvine.transform.position.z
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "Spirit(Clone)") {
            heartcrankhandle_arm found_arm = find_free_arm();
            if (found_arm) {
                spirit_ai spirit_brain = other.GetComponent<spirit_ai>();
                if (!spirit_brain.preview_mode) {
                    found_arm.setMySpirit(other.gameObject);
                    spirit_brain.heartvine_crank_handle = found_arm.gameObject;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.name == "Spirit(Clone)") {
            spirit_ai spirit_brain = other.GetComponent<spirit_ai>();
            if (spirit_brain.heartvine_crank_handle != null) {
                heartcrankhandle_arm arm = spirit_brain.heartvine_crank_handle.GetComponent<heartcrankhandle_arm>();
                arm.releaseMySpirit();
                spirit_brain.heartvine_crank_handle = null;
            }
        }
    }

    heartcrankhandle_arm find_free_arm() {
        foreach (heartcrankhandle_arm arm in arms) {
            if (arm.isFree()) {
                return arm;
            }
        }
        return null;
    }

    heartcrankhandle_arm find_occupied_arm() {
        foreach (heartcrankhandle_arm arm in arms) {
            if (!arm.isFree()) {
                return arm;
            }
        }
        return null;
    }

    float get_current_rotation() {
        float rotation = 0;
        foreach (heartcrankhandle_arm arm in arms) {
            if (!arm.isFree()) {
                rotation += arm.current_rotation_difference;
            }
        }
        return rotation;

    }
}