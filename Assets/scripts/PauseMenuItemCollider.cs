using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuItemCollider : MonoBehaviour
{
    public PauseMenuItem item;

    private void OnMouseEnter() {
        if (item.pausescreen != null) {
            item.pausescreen.set_active(item.transform.GetSiblingIndex());
        }
        if (item.optionsscreen != null) {
            item.optionsscreen.set_active(item.transform.GetSiblingIndex());
        }
    }

    private void OnMouseExit() {
        item.hover_off();
    }
}
