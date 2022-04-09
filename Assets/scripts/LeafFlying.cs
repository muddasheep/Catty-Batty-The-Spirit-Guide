using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafFlying : MonoBehaviour
{
    public GameObject leftright;
    public GameObject leafsprite;
    public SpriteRenderer sr;
    public Vector3 startpos;
    public Vector3 leftright_startpos;
    float last_x = 0;
    float timealive = 1f;
    float myoffset = 0;
    float timeForSin = 5f;

    private void Start() {
        startpos = transform.position;
        leftright_startpos = leftright.transform.localPosition;
        myoffset = Random.Range(0, 100f);
        if (myoffset > 50f) {
            leafsprite.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        timeForSin = 3f + Random.Range(0, 2f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float mysin = Mathf.Sin((Time.time + myoffset) * timeForSin);
        float mysinupdown = Mathf.Abs(mysin) / 5f;

        // move left right & up and down
        leftright.transform.localPosition = new Vector3(
            leftright_startpos.x + mysin / 3f,
            leftright_startpos.y + mysinupdown / 3f,
            leftright_startpos.z
        );

        // rotate
        leftright.transform.eulerAngles = new Vector3(0, 0, 0 + mysin*20f);

        // move everything down slowly
        float downspeed = 0.01f;
        transform.position = new Vector3(transform.position.x, transform.position.y - downspeed, transform.position.z);

        timealive -= 0.01f;

        sr.color = new Color(255f, 255f, 255f, timealive);

        if (timealive <= 0) {
            Destroy(this.gameObject);
        }
    }
}
