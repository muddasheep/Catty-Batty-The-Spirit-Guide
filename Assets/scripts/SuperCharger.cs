using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperCharger : MonoBehaviour
{
    public SuperChargeSprite[] superchargesprites;

    float current_charge = 0; // adds or subtracts Time.deltaTime

    public bool fully_charged = false;
    bool charging = false;
    float current_charge_percent; // 0 - 100
    float charging_start_time; // set every time a spirit hovers
    float charging_stop_time; // set after no spirit has hovered for 0.2 sec
    float charging_total_time; // set first time the charger started

    float scale = Mathf.Pow(2f, 1.0f / 12f);
    float charging_seconds_required = 3;

    int total_sprites;
    public SpriteRenderer spriter;
    public Animator wobb;
    public AudioSource chargesound;
    public AudioSource chargesound2;
    float current_pitch = 0.85f;
    float current_volume = 0;
    float current_pitch2 = 0.85f;
    float current_volume2 = 0;
    bool play_sound = false;
    Coroutine stopcoroutine;

    void Start()
    {
        total_sprites = superchargesprites.Length - 1;
    }

    void FixedUpdate()
    {
        check_charge();
    }

    void check_charge() {
        if (fully_charged == true) {
            return;
        }

        if (is_still_charging()) {
            if (current_charge < charging_seconds_required) {
                current_charge += Time.deltaTime;
            }
            if (current_charge > charging_seconds_required) {
                current_charge = charging_seconds_required;
            }
        }
        else {
            if (current_charge > 0) {
                current_charge -= Time.deltaTime;
            }
            if (current_charge < 0) {
                current_charge = 0;
            }
        }
        current_charge_percent = calculate_current_charge_percent();

        if (current_charge_percent >= 100) {
            fully_charged = true;
            Gamemaster gm = FindObjectOfType<Gamemaster>();
            gm.CheckGateConditions();
        }
        adjust_pitch(current_charge_percent);

        display_charge_sprite();
    }

    void adjust_pitch(float charge_percent) {
        if (charge_percent > 0) {
            PlaySound();
        }

        List<float> notes = new List<float>
        {
            -3f,
            -1f,
            0f,
            2f,
            4f,
            5f,
            7f,
            9f
        };

        List<float> notes2 = new List<float>
        {
            -12f,
            -5f,
            -3f,
            -1f,
            0f,
            2f,
            -1f,
            4f
        };

        float current_note = notes[Mathf.FloorToInt((charge_percent / 100) * 7f)];
        current_pitch = Mathf.Pow(scale, current_note);
        current_volume = 0.5f + (charge_percent / 100) * 0.5f;

        float current_note2 = notes2[Mathf.FloorToInt((charge_percent / 100) * 7f)];
        current_pitch2 = Mathf.Pow(scale, current_note2);
        current_volume2 = 0f + (charge_percent / 100) * 0.5f;

        chargesound.pitch = current_pitch;
        chargesound.panStereo = 0 + (charge_percent / 100) * 0.5f;
        chargesound.volume = current_volume;
        chargesound2.pitch = current_pitch2;
        chargesound.panStereo = 0 - (charge_percent / 100) * 0.5f;
        chargesound2.volume = current_volume2;

        if (charge_percent >= 100) {
            StopSound(0.003f);
        }
    }

    void PlaySound() {
        if (play_sound) {
            return;
        }

        chargesound.volume = 1f;
        chargesound2.volume = 1f;
        if (stopcoroutine != null) {
            StopCoroutine(stopcoroutine);
        }
        play_sound = true;
        chargesound.Play();
        chargesound2.Play();
    }

    void StopSound(float fadeoutamount = 0.01f) {
        if (!play_sound) {
            return;
        }

        play_sound = false;
        if (stopcoroutine != null) {
            StopCoroutine(stopcoroutine);
        }
        stopcoroutine = StartCoroutine(Fadeout(chargesound, chargesound2, fadeoutamount));
    }

    IEnumerator Fadeout(AudioSource audiosource, AudioSource audiosource2, float fadeoutamount) {
        while (audiosource.volume > 0) {
            audiosource.volume = audiosource.volume - fadeoutamount;
            audiosource2.volume = audiosource.volume;
            yield return new WaitForSeconds(0.01f);
        }
        chargesound.Stop();
        chargesound2.Stop();
    }

    void display_charge_sprite() {
        int chosen_sprite = Mathf.FloorToInt(total_sprites * current_charge_percent / 100);
        spriter.sprite = superchargesprites[chosen_sprite].superchargesprite;
    }

    float calculate_current_charge_percent() {
        if (current_charge > charging_seconds_required) {
            return 100;
        }
        return current_charge / charging_seconds_required * 100;
    }

    bool is_still_charging() {
        if (charging == false) {
            return false;
        }

        float current_time = Time.time;
        float time_since_last_charge = current_time - charging_start_time;

        if (time_since_last_charge < 0.5) {
            return true;
        }

        charging_stop_time = Time.time;
        charging = false;
        return false;
    }

    public void charge() {
        if (fully_charged == true) {
            return;
        }
        if (charging == false) {
            charging_total_time = Time.time;
        }
        charging = true;
        charging_start_time = Time.time;

        wobb.enabled = true;
        wobb.StopPlayback();
        wobb.Play("superchargerwobb", -1, 0);
    }
}
