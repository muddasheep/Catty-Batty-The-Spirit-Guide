using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayYarn : MonoBehaviour
{
    public Animator[] yarns;
    public SpriteRenderer[] sprite_renderers;
    public WayYarn previousWayYarn;

    public bool is_activated = false;
    public bool reveal_gate = false;
    public bool activate_at_start = false;

    // Start is called before the first frame update
    void Start() {
        foreach (Animator yarn in yarns) {
            yarn.enabled = false;
        }
        foreach (SpriteRenderer sr in sprite_renderers) {
            sr.enabled = false;
        }

        if (activate_at_start) {
            Activate();
        }
    }

    public void Activate() {
        if (is_activated)
            return;

        if (previousWayYarn != null && !previousWayYarn.is_activated)
            return;

        float delay = 0;

        foreach (Animator yarn in yarns) {
            StartCoroutine(ActivateLater(yarn.gameObject, delay));
            delay += 0.1f;
        }

        is_activated = true;

        if (reveal_gate) {
            FindObjectOfType<Gamemaster>().revealGates();
        }
    }

    private static IEnumerator ActivateLater(GameObject yarn, float delay) {

        yield return new WaitForSeconds(delay);

        Animator yarnimator = yarn.GetComponent<Animator>();
        SpriteRenderer sr = yarn.GetComponent<SpriteRenderer>();

        yarnimator.enabled = true;
        sr.enabled = true;

        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "Spirit(Clone)") {
            spirit_ai spirit_brain = other.GetComponent<spirit_ai>();
            if (!spirit_brain.preview_mode) {
                Activate();
            }
        }
    }
}