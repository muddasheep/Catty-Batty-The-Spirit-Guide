using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cattypillar : MonoBehaviour
{
    public CattyPillarCoordinate[] coordinates_path;

    CattyPillarCoordinate current_target_coordinate;
    int current_coordinate_index = 0;
    float speed = 0.1f;

    public GameObject[] tails;

    List<Vector3> previous_coordinates;

    // Start is called before the first frame update
    void Start()
    {
        current_target_coordinate = coordinates_path[0];

        previous_coordinates = new List<Vector3>();

        for (int i = 0; i < 100; i++) {
            previous_coordinates.Add(transform.position);
        }
    }

    void FixedUpdate()
    {
        Vector3 current_position = transform.position;

        if (current_position.x < current_target_coordinate.x) {
            current_position.x += speed;
        }

        if (current_position.x > current_target_coordinate.x) {
            current_position.x -= speed;
        }

        if (current_position.y < current_target_coordinate.y) {
            current_position.y += speed;
        }

        if (current_position.y > current_target_coordinate.y) {
            current_position.y -= speed;
        }

        transform.position = current_position;

        Vector3 target_position = new Vector3(current_target_coordinate.x, current_target_coordinate.y, 0);
        float distance = Vector3.Distance(current_position, target_position);

        previous_coordinates.Insert(0, transform.position);

        if (previous_coordinates.Count > 100) {
            previous_coordinates.RemoveAt( previous_coordinates.Count - 1);
        }

        if (distance <= 0.1f) {
            current_coordinate_index++;
            if (current_coordinate_index >= coordinates_path.Length) {
                current_coordinate_index = 0;
            }
            current_target_coordinate = coordinates_path[current_coordinate_index];
        }

        int tail_count = 0;
        foreach (GameObject tail in tails) {
            Vector3 newtailposition = previous_coordinates[(tail_count * 3) + 5];
            float height = Mathf.Sin(Time.time * 10 + tail_count * 10);
            newtailposition.y += height / 20;
            tail.transform.position = newtailposition;
            tail_count++;
        }
    }
}
