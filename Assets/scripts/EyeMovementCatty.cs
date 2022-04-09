using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMovementCatty : MonoBehaviour
{
    Vector3 startpos;
    Vector3 targetpos;

    player_movement player_script;
    public float x_limit;
    public float y_limit;

    // Start is called before the first frame update
    void Start() {
        player_script = GetComponentInParent<player_movement>();
        startpos = transform.parent.transform.position;
        targetpos = FindObjectOfType<SpiritGate>().transform.position;
    }

    private void FixedUpdate() {
        startpos = transform.parent.transform.position;
        targetpos = player_script.look_at_target_pos;
        LookAtTarget();
    }

    void LookAtTarget() {

        Vector3 newpos = Vector3.MoveTowards(startpos, targetpos, 0.05f);
        newpos = new Vector3(
            Mathf.Clamp(newpos.x, startpos.x - x_limit, startpos.x + x_limit),
            Mathf.Clamp(newpos.y, startpos.y - y_limit, startpos.y + y_limit),
            newpos.z

        );
        transform.position = newpos;
    }
}
