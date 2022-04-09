using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashTransitionHearts : MonoBehaviour
{
    void Awake() {
        SplashTransitionHearts[] hearts = GameObject.FindObjectsOfType<SplashTransitionHearts>();

        if (hearts.Length > 1) {
            Destroy(this.gameObject);
        }

    }
}
