using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationHover : MonoBehaviour
{
    public Animator[] dots;

    // Start is called before the first frame update
    void Start()
    {
        dots = GetComponentsInChildren<Animator>();

        float delay = 0;
        foreach (Animator ar in dots) {
            ar.StopPlayback();
            ar.Play("overworld_location_dot_rotation", -1, delay);
            delay += 0.16f;
        }
    }
}
