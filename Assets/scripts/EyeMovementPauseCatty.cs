using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMovementPauseCatty : MonoBehaviour
{
    Vector3 startpos;
    Vector3 targetpos;

    PauseScreen pause;
    public float x_limit;
    public float y_limit;

    // Start is called before the first frame update
    void Start() {
        pause = GetComponentInParent<PauseScreen>();
        startpos = transform.parent.transform.position;
        targetpos = pause.look_at_target_pos;
    }

    private void FixedUpdate() {
        startpos = transform.parent.transform.position;
        targetpos = pause.look_at_target_pos;
        LookAtTarget();
    }

    void LookAtTarget() {
        Vector3 newpos = Vector3.MoveTowards(startpos, targetpos, 0.5f);
        newpos = new Vector3(
            Mathf.Clamp(newpos.x, startpos.x - x_limit, startpos.x + x_limit),
            Mathf.Clamp(newpos.y, startpos.y - y_limit, startpos.y + y_limit),
            newpos.z
        );
        transform.position = newpos;
    }
}
