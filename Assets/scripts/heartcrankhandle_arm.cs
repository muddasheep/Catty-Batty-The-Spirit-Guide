using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heartcrankhandle_arm : MonoBehaviour
{
    public Animator animator;
    public bool grown = false;
    public GameObject my_spirit;
    public float current_rotation_difference = 0f;

    private void FixedUpdate() {
        if (grown == true && animator.GetBool("grown") == false) {
            animator.SetBool("grown", true);
        }
        if (grown == false && animator.GetBool("grown") == true) {
            animator.SetBool("grown", false);
        }

        if (grown == true && my_spirit != null) {
            float current_z = transform.parent.localRotation.z;

            // rotate towards my_spirit
            Vector2 targetDirection = my_spirit.transform.position - transform.parent.position;

            // get angle that we will rotate towards
            float angle = Vector3.SignedAngle(targetDirection, transform.parent.forward, transform.parent.up);

            // rotate now
            transform.parent.up = targetDirection;

            // save direction + difference
            current_rotation_difference = angle > 0 ? 1f : -1f;
            float z_difference = current_z - transform.parent.localRotation.z;
            if (angle == 0 || z_difference == 0) {
                current_rotation_difference = 0;
            }

            // set scale of arm
            float distance = Vector3.Distance(my_spirit.transform.position, transform.parent.position);
            float max_distance = 2.2f;
            float percent = distance / max_distance;
            transform.parent.localScale = new Vector2(percent, percent);
        }
    }

    public void setMySpirit(GameObject spirit) {
        my_spirit = spirit;
        grown = true;
    }

    public void releaseMySpirit() {
        my_spirit = null;
        grown = false;
    }

    public bool isFree() {
        if (my_spirit == null) {
            return true;
        }

        return false;
    }
}
