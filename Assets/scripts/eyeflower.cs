using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eyeflower : MonoBehaviour
{
    public GameObject eyeflower_leave;
    GameObject gamemanager;

    // Start is called before the first frame update
    void Start()
    {
        gamemanager = GameObject.Find("gamemanager");
    }

    public void stopAnimation() {
        gameObject.GetComponent<Animator>().enabled = false;
        StartCoroutine(waitForCurse(gameObject.GetComponent<eyeflower>(), 10F));
    }

    public void startAnimation() {
        if (gamemanager.GetComponent<Gamemaster>().spirits_active) {
            gameObject.GetComponent<Animator>().enabled = true;
        }
        else {
            StartCoroutine(waitForCurse(gameObject.GetComponent<eyeflower>(), 10F));
        }
    }

    void sleepingCurse() {
        GameObject spiritmaster = GameObject.Find("SpiritMaster");
        if (spiritmaster.transform.childCount > 0) {
            spiritmaster.BroadcastMessage("fallAsleep");
        }
        spawnLeaves();
        gamemanager.GetComponent<Gamemaster>().music_instructor.soundman.play_sound("eyeflower_windchimev2_" + Random.Range(1, 3));
    }

    void spawnLeaves() {
        float leave_count = 0;
        while (leave_count < 10f) {
            Vector3 rotation = new Vector3(0, 0, leave_count * 36f);
            GameObject new_leave = (GameObject)Instantiate(
                eyeflower_leave, gameObject.transform.position, Quaternion.Euler(rotation), gameObject.transform
            );
            leave_count += 1f;
            Destroy(new_leave, 1.1f);
        }
    }

    private static IEnumerator waitForCurse(eyeflower eyeflower, float delay) {
        float start = Time.time;
        while (Time.time <= start + delay) {
            yield return null;
        }

        eyeflower.startAnimation();
    }
}
