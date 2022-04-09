using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditCloud : MonoBehaviour
{
    float speed;
    float new_x;
    float new_y;
    public SpriteRenderer sr;
    public Animator ar;
    float last_wobble;
    public GameObject creditstar;
    Gamemaster gamemaster;

    bool vertical = false;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        ar = GetComponentInChildren<Animator>();
        gamemaster = FindObjectOfType<Gamemaster>();

        vertical = gamemaster.level_number > 31 ? true : false;

        reset();
        last_wobble = Time.time;
    }

    void reset() {
        if (vertical) {
            new_x = Random.Range(-16.7f, 16.7f);
        }
        else {
            new_y = Random.Range(-6.7f, 6.7f);
        }

        speed = Random.Range(0.02f, 0.1f);
        transform.localScale = new Vector3(0.5f + speed * 5f, 0.5f + speed * 5f, 1f);
        sr.sortingOrder = Mathf.RoundToInt(speed * 100f);
        sr.color = new Color(0.8f + speed*2f, 0.8f + speed*2f, 0.8f + speed*2f, 1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (vertical) {
            new_y = transform.position.y + (speed / 2f);

            if (new_y > 9f) {
                new_y = -9f;
                reset();
            }
        }
        else {
            new_x = transform.position.x + speed;

            if (new_x > 15f) {
                new_x = -15f;
                reset();
            }
        }

        transform.position = new Vector3(
            new_x,
            new_y,
            transform.position.z
        );
    }

    public void wobble() {
        if (Time.time - last_wobble < 0.5f) {
            return;
        }
        last_wobble = Time.time;
        ar.enabled = true;
        ar.StopPlayback();
        ar.Play("creditcloud_wobble", -1, 0);
        GameObject newstar = (GameObject)Instantiate(creditstar);
        newstar.transform.position = transform.position;
        float pann = transform.position.x / 14f;
        float pitch = transform.position.y + 7f;
        pitch = Mathf.Clamp(Mathf.RoundToInt(pitch / 1.6f), 1, 8);
        gamemaster.music_instructor.soundman.play_sound("windchime" + pitch, volume: 0.2f, pan: pann);
    }
}
