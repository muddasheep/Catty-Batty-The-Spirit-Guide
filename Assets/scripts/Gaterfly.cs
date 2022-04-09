using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaterfly : MonoBehaviour
{
    public Animator animato;
    bool flying = false;
    int randomflying = 0;
    Vector3 target;
    float my_y = 0;

    // Update is called once per frame
    void FixedUpdate() {
        if (flying) {
            Vector3 curpos = transform.position;
            float distance = Vector3.Distance(curpos, target);

            if (randomflying == 0) {
                if (distance > 0.1f) {
                    Vector3 newpos = curpos;

                    if (newpos.x < target.x) {
                        newpos.x = Random.Range(curpos.x, curpos.x + 0.03f);
                    }
                    else {
                        newpos.x = Random.Range(curpos.x, curpos.x - 0.03f);
                    }

                    if (newpos.y < target.y) {
                        newpos.y = Random.Range(curpos.y, curpos.y + 0.03f);
                    }
                    else {
                        newpos.y = Random.Range(curpos.y, curpos.y - 0.03f);
                    }

                    if (newpos.z < target.z) {
                        newpos.z = curpos.z + 0.01f;
                    }
                    else {
                        newpos.z = curpos.z - 0.01f;
                    }

                    rotateTowardsTarget(target);

                    transform.position = newpos;
                }
                else {
                    stopflying();
                }

                if (Random.Range(0, 5f) > 4.5f) {
                    randomflying = 20;
                }
            }
            else {
                Vector3 newpos = curpos;
                newpos.x = Random.Range(curpos.x - 0.01f, curpos.x + 0.01f);
                newpos.y = Random.Range(curpos.y - 0.03f, curpos.y + 0.03f);
                if (newpos.z < target.z) {
                    newpos.z = curpos.z + 0.01f;
                }
                else {
                    newpos.z = curpos.z - 0.01f;
                }
                transform.position = newpos;
                randomflying--;
            }
        }
    }

    public void fly() {
        flying = true;
        my_y = transform.position.y;
        animato.Play("gatebutterfly", 0, -1f);
        rotateTowardsTarget(target);
    }

    public void stopflying() {
        flying = false;
        animato.StopPlayback();
        animato.Play("gatebutterfly_idle", 0, -1f);
    }

    public void setTarget(Vector3 newpos) {
        target = newpos;
    }

    void rotateTowardsTarget(Vector3 newpos) {

        // rotate towards target
        Vector2 targetDirection = newpos - transform.position;

        // get angle that we will rotate towards
        float angle = Vector3.SignedAngle(
            targetDirection, transform.forward, transform.up
        );

        // rotate now
        transform.up = targetDirection;
    }
}
