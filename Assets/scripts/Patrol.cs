using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float x;
    public float y;
    public float x2;
    public float y2;
    public float targetX;
    public float targetY;
    public float initial_x;
    public float initial_y;
    public float speed = 0.07f;

    // Start is called before the first frame update
    void Start()
    {
        init_movement_patrol();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        check_movement();
        movement_patrol();
    }

    void init_movement_patrol() {
        targetX = x2;
        targetY = y2;
        initial_x = transform.position.x;
        initial_y = transform.position.y;
    }

    void check_movement() {
        float current_x = transform.position.x;
        float current_y = transform.position.y;

        if (current_x < targetX) {
            current_x += speed;
        }
        if (current_x > targetX) {
            current_x -= speed;
        }
        if (Mathf.Abs(current_x - targetX) < speed) {
            current_x = targetX;
        }

        if (current_y < targetY) {
            current_y += speed;
        }
        if (current_y > targetY) {
            current_y -= speed;
        }
        if (Mathf.Abs(current_y - targetY) < speed) {
            current_y = targetY;
        }

        transform.position = new Vector3(current_x, current_y, transform.position.z);

        float distance = get_distance();
        if (distance <= 0.1f) {
            float percent_done = (0.1f - distance) / 0.1f;
            transform.position = Vector3.Lerp(
                transform.position,
                new Vector3(targetX, targetY, transform.position.z),
                percent_done
            );
        }

        if (distance <= 0.05f) {
            transform.position = new Vector3(targetX, targetY, transform.position.z);
            speed = 0;
        }
    }

    void movement_patrol() {
        float current_x = transform.position.x;
        float current_y = transform.position.y;

        float distance = get_distance();

        if (distance < 0.1f) {
            if (Mathf.Round(current_x) == x2 && Mathf.Round(current_y) == y2) {
                SetTarget(initial_x, initial_y);
            }
            else {
                SetTarget(x2, y2);
            }
        }
    }

    float get_distance() {
        float distance = Vector2.Distance(
            transform.position, new Vector2(targetX, targetY)
        );
        return distance;
    }

    public void SetTarget(float x, float y) {
        targetX = x;
        targetY = y;
        speed = 0.07f;
    }
}
