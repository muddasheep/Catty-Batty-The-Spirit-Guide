using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneTransitioner : MonoBehaviour
{
    public SceneCircle[] circles;

    void Awake() {
        SceneTransitioner[] objs = GameObject.FindObjectsOfType<SceneTransitioner>();

        if (objs.Length > 1) {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void StartTransitionV3() {
        float delay = 0;
        foreach (SceneCircle circle in circles) {
            delay += 0.05f;
            circle.delay = delay;
            circle.FallIn();
        }
    }

    public void EndTransitionV3() {
        float delay = 1.2f;
        foreach (SceneCircle circle in circles) {
            delay -= 0.06f;
            circle.delay = delay;
            circle.FallOut();
        }
    }

    public void StartTransitionV2() {
        float delay = 1.2f;
        foreach(SceneCircle circle in circles) {
            delay -= 0.1f;
            circle.delay = delay;
            circle.ScaleIn();
        }
    }

    public void EndTransitionV2() {
        float delay = 1.2f;
        foreach (SceneCircle circle in circles) {
            delay -= 0.06f;
            circle.delay = delay;
            circle.ScaleOut();
        }
    }

    public void StartTransition() {
        float delay = 0;
        foreach (SceneCircle circle in circles) {
            delay += 0.05f;
            circle.delay = delay;
            circle.ScaleIn();
        }
    }


    public void EndTransition() {
        float delay = 0;
        foreach (SceneCircle circle in circles) {
            delay += 0.1f;
            circle.delay = delay;
            circle.ScaleOut();
        }
    }
}
