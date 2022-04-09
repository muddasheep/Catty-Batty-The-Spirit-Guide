using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMovement : MonoBehaviour
{
    Vector3 startpos;
    Vector3 targetpos;
    public OptionsScreen optionsscreen;

    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.parent.transform.position;
        targetpos = transform.position;
        optionsscreen = FindObjectOfType<OptionsScreen>();
    }

    private void FixedUpdate() {
        startpos = transform.parent.transform.position;
        if (optionsscreen.in_submenu) {
            if (optionsscreen.active_submenu_item_object != null) {
                targetpos = optionsscreen.active_submenu_item_object.getActiveItemPos();
            }
        }
        else {
            if (optionsscreen.active_pause_menu_item_object != null) {
                targetpos = optionsscreen.active_pause_menu_item_object.transform.position;
            }
        }

        LookAtTarget();
    }

    void LookAtTarget() {
        Debug.DrawLine(startpos, targetpos, Color.green);
        transform.position = Vector3.MoveTowards(startpos, targetpos, 0.5f);
    }
}
