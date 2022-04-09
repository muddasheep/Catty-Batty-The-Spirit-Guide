using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leaf : MonoBehaviour
{
    public Sprite leaf_single;
    public Sprite leaf_pile;

    public bool is_pile = false;

    public void grow_pile() {
        is_pile = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = leaf_pile;
        gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public void remove_pile() {
        is_pile = false;
        gameObject.GetComponent<SpriteRenderer>().sprite = leaf_single;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, -90f);
    }
}
