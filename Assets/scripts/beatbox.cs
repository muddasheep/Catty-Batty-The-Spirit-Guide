using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beatbox : MonoBehaviour
{
    public Animator beatboxanimator;
    public Animator beatboxkickeranimator;
    public Gamemaster gm;
    public float shaking_time = 0f;

    public int Step = 1;
    public float BPM = 110f;
    public int CurrentStep = 1;
    public int CurrentMeasure = 1;

    private float interval;
    private float nextTime;
    float last_music_time = 0;
    float last_kick_time = 0;

    Coroutine ticker;

    public void shake() {
        beatboxanimator.StopPlayback();
        beatboxanimator.Play("beatboxshake", -1, 0f);
        gm.beatbox_kick_boost_spirits();
        shaking_time = Time.time;
    }

    void Start() {
        StartMetronome();
    }

    private void Update() {
        if (!gm.game_is_running) {
            StopCoroutine(ticker);
            return;
        }

        float current_music_time = gm.music_instructor.CurrentPlayTime();
        if (current_music_time < last_music_time) {
            // sample has looped! restart the tick
            StopCoroutine(ticker);
            ticker = StartCoroutine(DoTick(this));
            KickIt();
        }
        last_music_time = current_music_time;
    }

    public void StartMetronome() {
        StopCoroutine("DoTick");
        CurrentStep = 1;
        float multiplier = 0.5f;
        float tmpInterval = 60f / BPM;
        interval = tmpInterval / multiplier;
        nextTime = Time.time; // set the relative time to now
        ticker = StartCoroutine(DoTick(this));
    }

    IEnumerator DoTick(beatbox self) // yield methods return IEnumerator
    {
        CurrentStep = 1;
        yield return new WaitForSeconds(0.005f);
        while (gm.music_instructor.CurrentPlayTime() < 1f) {
            yield return new WaitForSeconds(0.001f);
        }

        for (; ; )
        {
            KickIt();
            nextTime = Time.time; // set the relative time to now
            // do something with this beat
            nextTime += interval; // add interval to our relative time
            yield return new WaitForSeconds(nextTime - Time.time); // wait for the difference delta between now and expected next time of hit
            CurrentStep++;
        }
    }

    public void KickIt() {
        if (Time.time - last_kick_time < 0.1f)
            return;
        beatboxkickeranimator.enabled = true;
        beatboxkickeranimator.StopPlayback();
        beatboxkickeranimator.Play("beatboxkicker_animation", -1, 0);
        gm.music_instructor.soundman.play_sound("kick1" + (CurrentStep % 2), pitch_amount: -7);
        last_kick_time = Time.time;
    }
}
