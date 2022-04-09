using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crystomality : MonoBehaviour
{
    public GameObject crysto1;
    public GameObject crysto2;
    public GameObject crysto3;
    public GameObject crysto4;
    public Animator animator1;
    public Animator animator2;
    public Animator animator3;
    public Animator animator4;
    public GameObject boxfox_wood1;
    public GameObject boxfox_wood2;
    public GameObject boxfox_wood3;
    public GameObject boxfox_wood4;

    bool exploded = false;
    private float initializationTime;
    Gamemaster gm;

    private void Awake() {
        initializationTime = Time.time;
        StartCoroutine(CrystActivator(gameObject));
        GameObject gamemaster = GameObject.FindGameObjectWithTag("GameMaster");
        gm = gamemaster.GetComponent<Gamemaster>();
    }

    private static IEnumerator CrystActivator(GameObject crystocontainer) {
        float start = Time.time;
        crystomality crystoscript = crystocontainer.GetComponent<crystomality>();
        List<Animator> animators = new List<Animator>();
        animators.Add(crystoscript.animator1);
        animators.Add(crystoscript.animator2);
        animators.Add(crystoscript.animator3);
        animators.Add(crystoscript.animator4);

        while (Time.time <= start + 0.1f) {
            yield return null;
        }

        int rando = Random.Range(0, animators.Count);
        Animator foundanimator = animators[rando];
        foundanimator.enabled = true;
        animators.RemoveAt(rando);
        while (Time.time <= start + 0.2f) {
            yield return null;
        }
        rando = Random.Range(0, animators.Count);
        foundanimator = animators[rando];
        foundanimator.enabled = true;
        animators.RemoveAt(rando);
        while (Time.time <= start + 0.3f) {
            yield return null;
        }
        rando = Random.Range(0, animators.Count);
        foundanimator = animators[rando];
        foundanimator.enabled = true;
        animators.RemoveAt(rando);
        while (Time.time <= start + 0.5f) {
            yield return null;
        }
        rando = Random.Range(0, animators.Count);
        foundanimator = animators[rando];
        foundanimator.enabled = true;
        animators.RemoveAt(rando);

        yield return new WaitForSeconds(0.5f);

        crystoscript.gm.CheckGateConditions();

        yield break;
    }

    public void explode(Vector3 origin) {
        if (exploded == true) {
            return;
        }
        if (animator1 == null || animator2 == null || animator3 == null || animator4 == null) {
            return;
        }
        animator1.SetBool("explode", true);
        animator2.SetBool("explode", true);
        animator3.SetBool("explode", true);
        animator4.SetBool("explode", true);
        Vector3 direction1 = crysto1.transform.position - origin;
        Vector3 direction2 = crysto2.transform.position - origin;
        Vector3 direction3 = crysto3.transform.position - origin;
        Vector3 direction4 = crysto4.transform.position - origin;
        crysto1.GetComponent<Rigidbody2D>().AddForce(direction1*2, ForceMode2D.Impulse);
        crysto2.GetComponent<Rigidbody2D>().AddForce(direction2*2, ForceMode2D.Impulse);
        crysto3.GetComponent<Rigidbody2D>().AddForce(direction3*2, ForceMode2D.Impulse);
        crysto4.GetComponent<Rigidbody2D>().AddForce(direction4*2, ForceMode2D.Impulse);
        exploded = true;
        GameObject gamemaster = GameObject.FindGameObjectWithTag("GameMaster");
        gamemaster.GetComponent<Gamemaster>().total_crystomalities--;
        gm.CheckGateConditions();
        gm.music_instructor.soundman.play_sound("crystomality_destroyed", pitch_amount: 12, volume: 0.5f);

        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        float timeSinceInitialization = Time.time - initializationTime;

        if (timeSinceInitialization < 0.1f) {
            // destroy boxes in range
            if (collider.gameObject.name == "tower_block") {
                if (Vector3.Distance(collider.gameObject.transform.position, transform.position) <= 2f) {
                    spawnWood(collider.gameObject.transform.position);
                    gm.grid_manager.delete_tower(collider.gameObject.transform.position.x, collider.gameObject.transform.position.y);
                    gm.music_instructor.soundman.play_sound("rip" + Random.Range(5, 9));
                }
            }
            return;
        }

        if (collider.gameObject.name == "tower_block") {
            explode(collider.gameObject.transform.position);
        }
    }

    void spawnWood(Vector3 target_position) {
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

            GameObject parent_wood = Instantiate(base_wood, target_position, Quaternion.Euler(rotation));

            GameObject new_leave = (GameObject)Instantiate(
                wood, target_position, Quaternion.Euler(rotation), parent_wood.transform
            );
            leave_count += 1f;
            Destroy(parent_wood, 0.5f);
        }
    }
}
