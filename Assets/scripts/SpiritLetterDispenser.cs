using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritLetterDispenser : MonoBehaviour
{
    public GateLetter gate_letter;
    private GameObject spirit_letter;

    // Start is called before the first frame update
    void Start()
    {
        SpiritLetter spiritletter = GetComponentInChildren<SpiritLetter>();
        spiritletter.setLetter(spiritletter.findNumberForLetter(gate_letter.text));
    }
}
