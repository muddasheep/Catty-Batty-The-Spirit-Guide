using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newgameplussign : MonoBehaviour
{
    public bool hover = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        float new_x = transform.parent.transform.position.x;

        if (hover) {
            new_x = new_x - 0.1f;
            if (new_x < -0.8f) {
                new_x = -0.8f;
            }
        }
        else {
            new_x = new_x + 0.1f;
            if (new_x > 0) {
                new_x = 0;
            }
        }

        transform.parent.transform.position = new Vector3(
            new_x, transform.parent.transform.position.y, transform.parent.transform.position.z
        );
    }

    public void Show() {
        transform.parent.localScale = new Vector3(1f, 1f, 1f);
    }

    private void OnMouseEnter() {
        hover = true;
    }

    private void OnMouseExit() {
        hover = false;
    }

    private void OnMouseDown() {
        transform.parent.localScale = new Vector3(1.01f, 1.01f, 1.01f);
    }
    private void OnMouseUp() {
        transform.parent.localScale = new Vector3(1f, 1f, 1f);
    }
}
