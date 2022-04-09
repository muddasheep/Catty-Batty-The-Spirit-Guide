using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocationInfo : MonoBehaviour
{
    public TextMeshPro clocktext;
    public TextMeshPro boxtext;
    public TextMeshPro leveltext;
    public TextMeshPro levelnumber;
    public Animator ar;

    IEnumerator Start() {
        float x_scale = 0;
        transform.localScale = new Vector3(x_scale, 1f, 1f);
        yield return new WaitForSeconds(2f);
        while (x_scale < 1f) {
            yield return new WaitForSeconds(0.01f);
            x_scale += 0.1f;
            transform.localScale = new Vector3(x_scale, 1f, 1f);
        }
    }

    public void SetInfo(string box, float totaltime, string level_name, int level_number) {
        TimeSpan time = TimeSpan.FromSeconds(totaltime);
        string str = time.ToString(@"mm\:ss");

        leveltext.text = "<voffset=0em><size=160%>" + level_number + "<voffset=0.5>:<size=100%> " + level_name.ToUpper();
        clocktext.text = totaltime == 0 ? "--:--" : str;
        boxtext.text = box;
        ar.StopPlayback();
        ar.Play("overworld_info_wobble", -1, 0);
        //levelnumber.text = level_number.ToString();
    }
}
