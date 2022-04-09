using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulbtree : MonoBehaviour {
    public Light twilight;
    public Material spritewithlights;
    public SpriteRenderer spriterenderer;
    
    void Start() {
        twilight = GetComponentInChildren<Light>();
        spriterenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (twilight.enabled) {
            return;
        }

        if (other.name == "Spirit(Clone)") {
            spirit_ai spirit_intelligence = other.GetComponent<spirit_ai>();
            if (spirit_intelligence.preview_mode) {
                return;
            }
            twilight.enabled = true;
            StartCoroutine(LightenUp(twilight));
            spriterenderer.material = spritewithlights;
            Gamemaster gm = FindObjectOfType<Gamemaster>();
            gm.music_instructor.soundman.play_sound("lightbulb", Random.Range(-2, 2), volume: 1f);
        }
    }

    private static IEnumerator LightenUp(Light light) {
        float start = Time.time;
        while (light.intensity < 1f) {
            light.intensity = light.intensity + 0.01f;
            yield return null;
        }
    }
}
