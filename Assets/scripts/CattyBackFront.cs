using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CattyBackFront : MonoBehaviour
{
    public SpriteRenderer sprite_renderer;
    public Sprite sprite_front;
    public Sprite sprite_back;
    public GameObject dust_cloud;
    float last_y;

    int dust_cloud_delay = 0;
    int dust_cloud_sort_number;

    public bool facing_front = true;
    GameObject my_dust_cloud;

    // Start is called before the first frame update
    void Start()
    {
        last_y = transform.position.y;
        sprite_renderer.sprite = sprite_front;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y > last_y) {
            sprite_renderer.sprite = sprite_back;
            dust_cloud_sort_number = 44;
            summon_dust_cloud();
            facing_front = false;
        }
        else {
            if (transform.position.y < last_y) {
                sprite_renderer.sprite = sprite_front;
                dust_cloud_sort_number = 40;
                summon_dust_cloud();
                facing_front = true;
            }
        }
        last_y = transform.position.y;
    }

    void summon_dust_cloud() {
        dust_cloud_delay--;

        if (dust_cloud_delay <= 0) {
            float spirit_smoke_y = transform.position.y - 0.6f;

            if (my_dust_cloud == null) {
                my_dust_cloud = (GameObject)Instantiate(dust_cloud);
            }
            my_dust_cloud.transform.position = new Vector3(transform.position.x, spirit_smoke_y, 0);
            my_dust_cloud.GetComponent<SpriteRenderer>().sortingOrder = dust_cloud_sort_number;
            Animator ar = my_dust_cloud.GetComponent<Animator>();
            ar.StopPlayback();
            ar.Play("dustcloud", -1, 0);
            dust_cloud_delay = 9;
        }
    }
}
