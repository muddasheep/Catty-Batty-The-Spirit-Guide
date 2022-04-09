using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OverworldMap : MonoBehaviour
{
    public Sprite[] cloudsprites;
    public GameObject cloudprefab;

    public List<GameObject> clouds;
    UniverseMaster um;

    // Start is called before the first frame update
    void Awake()
    {
        UniverseMaster[] masters = GameObject.FindObjectsOfType<UniverseMaster>();
        foreach (UniverseMaster master in masters) {
            if (master.is_singleton) {
                um = master;
            }
        }

        if (um == null) {
            um = FindObjectOfType<UniverseMaster>();
        }

        if (um.AllLevelsCleared()) {
            newgameplussign sign = FindObjectOfType<newgameplussign>();
            if (sign != null) {
                sign.Show();
            }
        }
        else {
            createClouds();
            GameObject overworld_home = GameObject.Find("overworld_home");
            removeCloudsAt(overworld_home.transform.position);
        }
    }

    IEnumerator Start() {
        yield return new WaitForSeconds(0.4f);
        um = FindObjectOfType<UniverseMaster>();
        um.checkForDarkMode();
    }

    void createClouds() {
        float start_x = -11f;
        float max_x = 11f;
        float start_y = -9f;
        float max_y = 9f;

        float cur_x = start_x;
        float cur_y = start_y;

        List<int> spritenumbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };

        while (cur_y <= max_y) {

            while (cur_x <= max_x) {

                GameObject cloud = (GameObject)Instantiate(cloudprefab);
                clouds.Add(cloud);
                cloud.transform.position = new Vector3(
                    cur_x, cur_y, 0
                );

                SpriteRenderer sr = cloud.GetComponent<SpriteRenderer>();

                int index = Random.Range(0, spritenumbers.Count);
                sr.sprite = cloudsprites[spritenumbers[index]];
                sr.sortingOrder = 5 + index;

                spritenumbers.RemoveAt(index);

                if (spritenumbers.Count == 0) {
                    spritenumbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };
                }

                cur_x++;
            }

            cur_x = start_x;
            cur_y++;
        }
    }

    public void removeCloudsAt(Vector3 pos, int level_number = 0) {
        foreach (GameObject cloud in clouds) {

            float area = 2f;

            if (level_number == 30) {
                area = 4f;
            }

            if (cloud.transform.position.x > pos.x - area
                && cloud.transform.position.x < pos.x + area
                && cloud.transform.position.y > pos.y - area
                && cloud.transform.position.y < pos.y + area) {

                cloud.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }
}
