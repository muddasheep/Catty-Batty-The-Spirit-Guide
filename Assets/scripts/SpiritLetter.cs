using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritLetter : MonoBehaviour
{
    public Sprite[] letters;
    public SpriteRenderer sr;
    public Animator ar;
    public int letter = 0;
    string allletters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    // Start is called before the first frame update
    void Start()
    {
        setLetter(letter);
    }

    public void setLetter(int letter_number) {
        letter = letter_number;
        sr.sprite = letters[letter];
    }

    public int findNumberForLetter(string given_letter) {
        int count = 0;
        foreach (char letter in allletters.ToCharArray()) {
            if (letter.ToString() == given_letter) {
                return count;
            }
            count++;
        }
        return 1;
    }
}
