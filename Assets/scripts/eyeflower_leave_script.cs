using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eyeflower_leave_script : MonoBehaviour
{
    Gamemaster gm;
    private void Start() {
        gm = FindObjectOfType<Gamemaster>();
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.name == "tower_block") {
            gm.DestroyBoxWithEffects(collider.gameObject);
        }
    }
}
