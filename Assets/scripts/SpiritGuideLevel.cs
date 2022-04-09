using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpiritGuideLevel
{
    public int level_number = 0;
    public int total_times_finished = 0;
    public float best_finished_time_seconds = 0;
    public int best_finished_boxes_used = 0;
    public bool revealed = false;
}
