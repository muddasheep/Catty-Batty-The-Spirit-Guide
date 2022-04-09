using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beatboxkicker : MonoBehaviour
{
    public beatbox beatbox;

    public void kick() {
        beatbox.shake();
    }
}
