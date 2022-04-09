using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shromp : MonoBehaviour
{
    public Animator animator;
    public Gamemaster gm;
    public float last_jomp = 0;
    public Animator shrompwave;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<Gamemaster>();
        last_jomp = Time.time;
    }

    public bool jomp() {
        if (Time.time - last_jomp < 0.5f) {
            return false;
        }
        animator.StopPlayback();
        animator.Play("shromp", -1, 0);
        if (!gm.has_beatbox) {
            gm.boost_spirits(2f);
        }

        last_jomp = Time.time;

        shrompwave.enabled = true;
        shrompwave.transform.Rotate(0, 0, Random.Range(0, 90f), Space.Self);
        shrompwave.StopPlayback();
        shrompwave.Play("shrompwave", -1, 0);

        gm.music_instructor.soundman.play_sound("squeek_v2c", Random.Range(-5, 5));

        return true;
    }
}
