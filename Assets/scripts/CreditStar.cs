using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditStar : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }
}
