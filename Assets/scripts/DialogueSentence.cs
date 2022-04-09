using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Emotion
{
    angry, confused, excited, happy, sad, surprised
}

[System.Serializable]
public class DialogueSentence
{
    public string KeyID;
    [TextArea(3, 10)]
    public string text;
    public GameObject target_object;
    public Vector3 target_position;
    public string function_name;
    public Emotion emotion;
    public bool hide_last_target_object;
    public string hide_sprite_with_name;
}
