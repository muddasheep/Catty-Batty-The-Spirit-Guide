using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsSubSlider : MonoBehaviour
{
    public GameObject sliderbar;
    public GameObject arrow_left;
    public GameObject arrow_right;

    public float percent_complete = 50;
    int total_bars = 5;
    List<OptionsSubSliderBar> bars;

    // Start is called before the first frame update
    void Awake()
    {
        bars = new List<OptionsSubSliderBar>();

        build_bars();
        displayPercent();
    }

    public void displayPercent() {
        float percent_per_bar = 100f / total_bars;
        float current_percent = 0;

        foreach (OptionsSubSliderBar bar in bars) {
            if (current_percent < percent_complete) {
                bar.is_active = true;
                bar.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else {
                bar.is_active = false;
                bar.transform.localScale = new Vector3(1f, 1f, 1f);
                /*
                float next_bar = current_percent + percent_per_bar;
                float prev_bar = current_percent - percent_per_bar;
                if (next_bar > percent_complete && prev_bar <= percent_complete) {
                    float bar_difference = next_bar - percent_complete;
                    float scaley = percent_per_bar / bar_difference * 3f;

                    bar.transform.localScale = new Vector3(1f, scaley, 1f);
                }
                else {
                    bar.transform.localScale = new Vector3(1f, 1f, 1f);
                }*/
            }
            bar.showState();
            current_percent += percent_per_bar;
        }

        transform.parent.BroadcastMessage("ValueChange");
    }

    void build_bars() {
        for (int i = 0; i < total_bars; i++) {
            GameObject newbar = (GameObject)Instantiate(sliderbar);
            OptionsSubSliderBar bar = newbar.GetComponent<OptionsSubSliderBar>();
            bars.Add(bar);
            newbar.transform.parent = transform;
            newbar.transform.localPosition = new Vector3(0.5f + i / 2f, 0, 0);
        }

        arrow_right.transform.localPosition = new Vector3(
            total_bars / 2 + 1f,
            0, 0
        );
    }

    public void Increase(float amount = 20f) {
        percent_complete = Mathf.Clamp(percent_complete + amount, 0, 100);
        displayPercent();
    }
    public void Decrease(float amount = 20f) {
        percent_complete = Mathf.Clamp(percent_complete - amount, 0, 100);
        displayPercent();
    }
}
