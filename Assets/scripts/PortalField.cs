using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalField : MonoBehaviour
{
    public Animator ar;
    private void Start() {
        ar = GetComponentInChildren<Animator>();
        ar.StopPlayback();
        ar.Play("portal_big_rotation", 0, Random.Range(0, 1f));
    }
}
