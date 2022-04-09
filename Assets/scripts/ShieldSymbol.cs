using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSymbol : MonoBehaviour
{
    public int sequence_number;
    public SpriteRenderer box_sign_sr;
    public Sprite box_sign;
    public Sprite box_sign_checked;

    public GameObject boxfox_wood1;
    public GameObject boxfox_wood2;
    public GameObject boxfox_wood3;
    public GameObject boxfox_wood4;

    float last_wood_spawn = 0;

    public void set_checked() {
        box_sign_sr.sprite = box_sign_checked;
    }

    public void set_unchecked() {
        box_sign_sr.sprite = box_sign;
    }

    public void spawnWood() {
        float leave_count = 0;

        if (Time.time - last_wood_spawn < 0.2f) {
            return;
        }

        last_wood_spawn = Time.time;

        GameObject base_wood = new GameObject();

        while (leave_count < 5f) {
            Vector3 rotation = new Vector3(0, 0, Random.Range(0, 360f));
            GameObject wood = boxfox_wood1;
            if (leave_count > 1f) {
                wood = boxfox_wood2;
            }
            if (leave_count > 2f) {
                wood = boxfox_wood3;
            }
            if (leave_count > 3f) {
                wood = boxfox_wood4;
            }

            GameObject parent_wood = Instantiate(base_wood, transform.position, Quaternion.Euler(rotation));

            GameObject new_leave = (GameObject)Instantiate(
                wood, transform.position, Quaternion.Euler(rotation), parent_wood.transform
            );
            leave_count += 1f;
            Destroy(parent_wood, 0.5f);
        }
    }
}
