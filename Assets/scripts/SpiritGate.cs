using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritGate : MonoBehaviour
{
    bool open = false;
    public Sprite gate_open;
    public Sprite gate_closed;
    public GameObject dissoleaves;
    public GameObject gatebutterfly;
    public SpriteRenderer sr;

    public GameObject gateline_prefab;

    Gamemaster gamemanager_script;

    private void Start() {
        gamemanager_script = FindObjectOfType<Gamemaster>();
    }

    public void Close() {
        sr.sprite = gate_closed;
        if (open == true) {
            if (Time.timeSinceLevelLoad > 1f) {
                gamemanager_script.music_instructor.soundman.play_sound("gate_closed");
            }
        }
        open = false;
    }

    public void Open() {
        sr.sprite = gate_open;
        if (open == false) {
            if (Time.timeSinceLevelLoad > 1f) {
                gamemanager_script.music_instructor.soundman.play_sound("gate_open2");
            }
        }
        open = true;
    }

    public bool IsOpen() {
        return open;
    }

    public void Dissoleave() {
        GameObject dissoleave = (GameObject)Instantiate(dissoleaves);
        dissoleave.transform.position = new Vector3(
            transform.position.x,
            transform.position.y + 0.85f,
            transform.position.z
        );
        transform.position = new Vector3(-20f, -20f, transform.position.z);
    }

    public void Reveal() {
        sr.enabled = true;
        Gaterfly flyscript = gatebutterfly.GetComponent<Gaterfly>();
        flyscript.GetComponent<SpriteRenderer>().enabled = true;
        Open();
    }

    public void spiritAnimation() {
        for (int i=0; i < 6; i++) {
            GameObject line = (GameObject)Instantiate(gateline_prefab);
            line.transform.position = gateline_prefab.transform.position;
            StartCoroutine(ActivateAnimatorDelayed(line, Random.Range(0, 0.3f)));
            line.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));
        }

        if (gamemanager_script.spirits_left() == 1) {
            Gaterfly flyscript = gatebutterfly.GetComponent<Gaterfly>();
            Vector3 newtarget = new Vector3(
                gatebutterfly.transform.position.x - 8f + Random.Range(0f, 16f),
                9f,
                gatebutterfly.transform.position.z
            );
            flyscript.setTarget(newtarget);
            flyscript.fly();
        }
    }

    private static IEnumerator ActivateAnimatorDelayed(GameObject line, float delay) {

        yield return new WaitForSeconds(delay);

        SpriteRenderer sr = line.GetComponentInChildren<SpriteRenderer>();
        sr.enabled = true;

        Animator animator = line.GetComponentInChildren<Animator>();
        animator.enabled = true;
        animator.StopPlayback();
        animator.Play("gateline", -1, 0f);
        Destroy(line, 2f);
    }

}
