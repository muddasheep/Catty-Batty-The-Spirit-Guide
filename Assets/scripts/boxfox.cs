using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxfox : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    Vector2 movement;

    public bool asleep = true;
    GameObject gamemanager;
    public Gamemaster gamemanager_script;
    GridManager gridmanager;
    GameObject[] boxes_to_nom;
    public GameObject box_to_nom;

    public GameObject boxfox_cloud;
    public GameObject boxfox_dustcloud;
    public GameObject boxfox_wood1;
    public GameObject boxfox_wood2;
    public GameObject boxfox_wood3;
    public GameObject boxfox_wood4;

    // Start is called before the first frame update
    void Start()
    {
        gamemanager = GameObject.Find("gamemanager");
        gamemanager_script = gamemanager.GetComponent<Gamemaster>();
        gridmanager = FindObjectOfType<GridManager>();
        StartCoroutine(waitForBox(gameObject.GetComponent<boxfox>(), 2F));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!asleep && box_to_nom != null && gamemanager_script.spirits_active) {
            goTowardsBoxToNom();
        }

        animator.SetFloat("horizontal", movement.x);
        animator.SetFloat("vertical", movement.y);
        animator.SetFloat("speed", movement.sqrMagnitude);

        if (!asleep && box_to_nom == null) {
            movement.x = 0;
            movement.y = 0;
            stopAnimation();
        }
    }

    public void stopAnimation() {
        //gameObject.GetComponent<Animator>().enabled = false;
        asleep = true;

        StartCoroutine(waitForBox(gameObject.GetComponent<boxfox>(), 2F));
    }

    public void startAnimation() {
        if (asleep) {
            //gameObject.GetComponent<Animator>().enabled = true;
            asleep = false;
            gamemanager_script.music_instructor.soundman.play_sound("squeek_v2a");
        }
    }

    void goTowardsBoxToNom() {
        if (box_to_nom) {
            Vector3 target = Vector3.MoveTowards(gameObject.transform.position, box_to_nom.transform.position, 0.1f);
            movement.x = box_to_nom.transform.position.x - gameObject.transform.position.x;
            movement.y = box_to_nom.transform.position.y - gameObject.transform.position.y;

            float distance = Vector3.Distance(gameObject.transform.position, box_to_nom.transform.position);

            transform.position = target;

            if (distance < 0.05f) {
                BoxNomNomNom();
            }
        }
    }

    void BoxNomNomNom() {
        if (box_to_nom) {
            spawnWood();
            gamemanager_script.music_instructor.soundman.play_sound("rip" + Random.Range(1, 5));
            gamemanager_script.music_instructor.soundman.play_sound("rip" + Random.Range(5, 9));

            GameObject cloud = (GameObject)Instantiate(boxfox_cloud);
            cloud.transform.position = gameObject.transform.position;
            GameObject dustcloud1 = (GameObject)Instantiate(boxfox_dustcloud);
            dustcloud1.transform.position = new Vector3(
                randomize(transform.position.x), randomize(transform.position.y), transform.position.z
            );

            GameObject dustcloud2 = (GameObject)Instantiate(boxfox_dustcloud);
            dustcloud2.transform.position = new Vector3(
                randomize(transform.position.x), randomize(transform.position.y), transform.position.z
            );

            Destroy(cloud, 1f);
            Destroy(dustcloud1, 1f);
            Destroy(dustcloud2, 1f);
            gridmanager.delete_tower(box_to_nom.transform.position.x, box_to_nom.transform.position.y);

            stopAnimation();
        }
    }

    float randomize(float value) {
        value += Random.Range(0, 0.5f);
        value -= Random.Range(0, 1f);
        return value;
    }

    void spawnWood() {
        float leave_count = 0;

        GameObject base_wood = new GameObject();

        while (leave_count < 5f) {
            Vector3 rotation = new Vector3(0, 0, Random.Range(0, 360f));
            GameObject wood = boxfox_wood1;
            if (leave_count > 1f) {
                wood = boxfox_wood2;
            }
            if (leave_count > 2f) {
                wood = boxfox_wood3;
            }
            if (leave_count > 3f) {
                wood = boxfox_wood4;
            }

            GameObject parent_wood = Instantiate(base_wood, transform.position, Quaternion.Euler(rotation));

            GameObject new_leave = (GameObject)Instantiate(
                wood, transform.position, Quaternion.Euler(rotation), parent_wood.transform
            );
            leave_count += 1f;
            Destroy(parent_wood, 0.5f);
        }
    }

    private static IEnumerator waitForBox(boxfox boxfox, float delay) {
        float start = Time.time;
        while (Time.time <= start + delay) {
            yield return null;
        }

        boxfox.FindBoxToNom();

        if (boxfox.box_to_nom == null || !boxfox.gamemanager_script.spirits_active) {
            boxfox.stopAnimation();
        }
        else {
            boxfox.startAnimation();
        }
    }

    void FindBoxToNom() {
        boxes_to_nom = GameObject.FindGameObjectsWithTag("TowerContainer");

        if (boxes_to_nom.Length > 0) {
            box_to_nom = boxes_to_nom[0];
        }
        else {
            box_to_nom = null;
        }
    }
}
