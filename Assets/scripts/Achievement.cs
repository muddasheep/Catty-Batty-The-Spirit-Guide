using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Achievement
{
    public string ID;
    public bool achieved = false;

    public Achievement(string new_ID) {
        ID = new_ID;
    }
}
