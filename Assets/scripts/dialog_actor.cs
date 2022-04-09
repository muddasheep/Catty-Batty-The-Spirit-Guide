using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialog_actor : MonoBehaviour
{
    public string name;
    public DialogSprite[] sprites;
    public SpriteRenderer sr;

    [System.Serializable]
    public class DialogSprite
    {
        public Sprite sprite;
        public Emotion emotion;
    }

    public void changeEmotion(Emotion new_emotion) {
        foreach (DialogSprite dialogsprite in sprites) {
            if (dialogsprite.emotion == new_emotion) {
                sr.sprite = dialogsprite.sprite;
            }
        }
    }
}
