using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritButton : MonoBehaviour
{
    public bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate() {
        active = true;
        foreach (Transform child in transform) {
            child.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    public void Deactivate() {
        active = false;
        foreach (Transform child in transform) {
            child.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
