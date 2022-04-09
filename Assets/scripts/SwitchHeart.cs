using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchHeart : MonoBehaviour
{
    public Transform target_transform;
    public Vector3 target_vector;
    public GameObject sinheart;
    public SpriteRenderer sr;

    bool moving = false;
    float speed = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!moving) {
            return;
        }

        Vector3 target = transform.position;
        Vector3 target_pos;
        float distance = 0;

        if (target_transform != null) {
            target = Vector3.MoveTowards(gameObject.transform.position, target_transform.position, speed);
            target_pos = target_transform.position;
            distance = Vector3.Distance(gameObject.transform.position, target_transform.position);
        }
        else {
            target = Vector3.MoveTowards(gameObject.transform.position, target_vector, speed);
            target_pos = target_vector;
            distance = Vector3.Distance(gameObject.transform.position, target_vector);
            sr.color = new Color(0.4f, 0.4f, 0.4f, Mathf.Clamp01(distance));
        }

        transform.position = target;

        // rotate towards target
        Vector2 targetDirection = target_pos - transform.position;

        // get angle that we will rotate towards
        float angle = Vector3.SignedAngle(
            targetDirection, transform.forward, transform.up
        );

        // rotate now
        sinheart.transform.up = targetDirection;

        // sinus move child
        float mysin = Mathf.Sin((Time.time * 2f) * 5f);
        float mysinupdown = Mathf.Abs(mysin) / 5f;

        // move left right & up and down
        sinheart.transform.GetChild(0).localPosition = new Vector3(
            0 + mysin / 3f,
            0 + mysinupdown / 3f,
            0
        );
        transform.GetChild(0).transform.position = sinheart.transform.GetChild(0).transform.position;

        if (distance < 0.05f && target_transform != null) {
            explode();
            Destroy(gameObject);
        }

        if (distance < 0.05f && target_transform == null) {
            Destroy(gameObject);
        }
    }

    public void FlyTowards(Transform newtarget) {
        transform.localScale = new Vector3(1f, 1f, 1f);
        target_transform = newtarget;
        moving = true;
        speed = 0.2f;
    }

    public void FlyTowardsVector(Vector3 newtarget) {
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        target_vector = newtarget;
        target_transform = null;
        moving = true;
        speed = 0.1f;
    }

    void explode() {
        moving = false;

        Gamemaster gm = FindObjectOfType<Gamemaster>();
        gm.music_instructor.soundman.play_sound("switchplayers2_real");

        // 4 clones shoot out in 4 directions
        GameObject cloneheart = (GameObject)Instantiate(gameObject);
        SwitchHeart my_heart_switch = cloneheart.GetComponent<SwitchHeart>();
        my_heart_switch.FlyTowardsVector(
            new Vector3(
                transform.position.x - 2f,
                transform.position.y - 2f,
                transform.position.z
            )
        );
        cloneheart = (GameObject)Instantiate(gameObject);
        my_heart_switch = cloneheart.GetComponent<SwitchHeart>();
        my_heart_switch.FlyTowardsVector(
            new Vector3(
                transform.position.x + 2f,
                transform.position.y - 2f,
                transform.position.z
            )
        );
        cloneheart = (GameObject)Instantiate(gameObject);
        my_heart_switch = cloneheart.GetComponent<SwitchHeart>();
        my_heart_switch.FlyTowardsVector(
            new Vector3(
                transform.position.x + 2f,
                transform.position.y + 2f,
                transform.position.z
            )
        );
        cloneheart = (GameObject)Instantiate(gameObject);
        my_heart_switch = cloneheart.GetComponent<SwitchHeart>();
        my_heart_switch.FlyTowardsVector(
            new Vector3(
                transform.position.x - 2f,
                transform.position.y + 2f,
                transform.position.z
            )
        );
    }
}
