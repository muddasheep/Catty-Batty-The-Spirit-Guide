using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritWaveyScript : MonoBehaviour
{
    Material mat;
    SpriteRenderer sr;
    Vector3 last_pos;
    float total_distance = 0;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        mat = sr.material;
        last_pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float distance_x = transform.position.x - last_pos.x;

        distance_x = Mathf.Clamp(distance_x, -0.1f, 0.1f);

        if (distance_x < 0 && distance_x < total_distance) {
            total_distance = distance_x * 5f;
        }
        if (distance_x > 0 && distance_x > total_distance) {
            total_distance = distance_x * 5f;
        }

        total_distance = Mathf.Lerp(total_distance, 0, Time.deltaTime);
        mat.SetFloat("_DistanceX", total_distance);
        mat.SetVector("_LastPosition", last_pos);
        mat.SetVector("_CurPosition", transform.position);
        last_pos = transform.position;
    }

}
