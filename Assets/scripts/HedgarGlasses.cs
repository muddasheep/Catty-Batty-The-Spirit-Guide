using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HedgarGlasses : MonoBehaviour
{
    spirit_ai my_spirit;
    public bool done = false;

    private void Update() {
        if (done) {
            transform.position = new Vector3(
                -9f, -1.5f, 0
            );
            return;
        }
        if (my_spirit != null) {
            Vector3 newpos = new Vector3(
                my_spirit.transform.position.x,
                my_spirit.transform.position.y + 1f,
                my_spirit.transform.position.z
            );
            transform.position = Vector3.Lerp(
                transform.position, newpos, 0.1f
            );
        }
    }

    public void attachToSpirit(spirit_ai spiritscript) {
        if (my_spirit != null) {
            return;
        }

        my_spirit = spiritscript;
        spiritscript.has_hedgar_glasses = true;
    }
}
