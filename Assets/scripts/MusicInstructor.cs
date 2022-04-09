using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicInstructor : MonoBehaviour
{
    public AudioClip main_theme;
    public AudioClip for_auri;
    public AudioClip overworld;
    public AudioClip dialog1;
    public AudioClip winninglevel;
    public AudioClip entercutscene;
    public AudioClip exitlevel;

    public AudioClip gameplay_variant1_stage1;
    public AudioClip gameplay_variant1_stage2;
    public AudioClip gameplay_variant1_stage3;
    public AudioClip gameplay_variant1_stage4;
    public AudioClip gameplay_variant2_stage1;
    public AudioClip gameplay_variant2_stage2;
    public AudioClip gameplay_variant2_stage3;
    public AudioClip gameplay_variant2_stage4;
    public AudioClip gameplay_variant2_stage5;
    public AudioClip gameplay_variant2_stage6;
    public AudioClip gameplay_variant2_stage7;
    public AudioClip gameplay_variant2_stage8;

    public AudioClip gameplay_variant3_stage1;
    public AudioClip gameplay_variant3_stage2;
    public AudioClip gameplay_variant3_stage3;
    public AudioClip gameplay_variant3_stage4;

    public AudioClip gameplay_variant4_stage1;
    public AudioClip gameplay_variant4_stage2;
    public AudioClip gameplay_variant4_stage3;
    public AudioClip gameplay_variant4_stage4;
    public AudioClip gameplay_variant4_stage5;
    public AudioClip gameplay_variant4_stage6;

    public AudioClip credits;
    public AudioClip credits_broken_heaven;

    public AudioSource background_music1;
    public AudioSource background_music2;

    public SoundMan soundman;
    public bool track1_playing = false;

    float music_volume = 0.6f;
    bool music_muted = false;

    void Awake() {
        MusicInstructor[] masters = GameObject.FindObjectsOfType<MusicInstructor>();

        if (masters.Length > 1) {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayMusic(string song, float pitch = 1f) {
        if (song == "main_theme") {
            SetMusic(main_theme, true);
        }
        if (song == "for_auri") {
            SetMusic(for_auri, false);
        }
        if (song == "overworld") {
            SetMusic(overworld, true);
        }
        if (song == "overworld_hard") {
            SetMusic(overworld, true, 0.9f);
        }
        if (song == "dialog1") {
            SetMusic(dialog1, true);
        }
        if (song == "winninglevel") {
            SetMusic(winninglevel, true, pitch);
        }
        if (song == "entercutscene") {
            SetMusic(entercutscene, false);
        }
        if (song == "exitlevel") {
            SetMusic(exitlevel, false);
        }
        if (song == "exitlevel_hard") {
            SetMusic(exitlevel, false, 0.9f);
        }
        if (song == "gameplay_variant1") {
            SetDualMusic(gameplay_variant2_stage1, gameplay_variant2_stage2, true, pitch);
        }
        if (song == "gameplay_variant2") {
            SetDualMusic(gameplay_variant1_stage1, gameplay_variant1_stage3, true, pitch);
        }
        if (song == "gameplay_variant3") {
            SetDualMusic(gameplay_variant2_stage5, gameplay_variant1_stage4, true, pitch);
        }
        if (song == "gameplay_variant4") {
            SetDualMusic(gameplay_variant2_stage3, gameplay_variant2_stage4, true, pitch);
        }
        if (song == "gameplay_variant5") {
            SetDualMusic(gameplay_variant1_stage1, gameplay_variant1_stage3, true, pitch);
        }
        if (song == "gameplay_variant6") {
            SetDualMusic(gameplay_variant2_stage1, gameplay_variant2_stage6, true, pitch);
        }
        if (song == "gameplay_variant7") {
            SetDualMusic(gameplay_variant2_stage7, gameplay_variant2_stage8, true, pitch);
        }
        if (song == "gameplay_variant8") {
            SetDualMusic(gameplay_variant1_stage1, gameplay_variant1_stage2, true, pitch);
        }

        if (song == "gameplay_variant9") {
            SetDualMusic(gameplay_variant1_stage1, gameplay_variant2_stage2, true, pitch);
        }
        if (song == "gameplay_variant10") {
            SetDualMusic(gameplay_variant2_stage1, gameplay_variant2_stage4, true, pitch);
        }
        if (song == "gameplay_variant11") {
            SetDualMusic(gameplay_variant3_stage1, gameplay_variant3_stage4, true, pitch);
        }
        if (song == "gameplay_variant12") {
            SetDualMusic(gameplay_variant4_stage1, gameplay_variant4_stage2, true, pitch);
        }
        if (song == "gameplay_variant13") {
            SetDualMusic(gameplay_variant2_stage7, gameplay_variant1_stage3, true, pitch);
        }

        if (song == "gameplay_variant14") {
            SetDualMusic(gameplay_variant1_stage1, gameplay_variant1_stage2, true, pitch);
        }
        if (song == "gameplay_variant15") {
            SetDualMusic(gameplay_variant2_stage1, gameplay_variant2_stage2, true, pitch);
        }
        if (song == "gameplay_variant16") {
            SetDualMusic(gameplay_variant2_stage3, gameplay_variant1_stage3, true, pitch);
        }
        if (song == "gameplay_variant17") {
            SetDualMusic(gameplay_variant3_stage3, gameplay_variant3_stage2, true, pitch);
        }
        if (song == "gameplay_variant18") {
            SetDualMusic(gameplay_variant2_stage7, gameplay_variant1_stage4, true, pitch);
        }

        if (song == "gameplay_variant19") {
            SetDualMusic(gameplay_variant1_stage1, gameplay_variant2_stage6, true, pitch);
        }
        if (song == "gameplay_variant20") {
            SetDualMusic(gameplay_variant2_stage1, gameplay_variant2_stage1, true, pitch);
        }
        if (song == "gameplay_variant21") {
            SetDualMusic(gameplay_variant2_stage3, gameplay_variant1_stage2, true, pitch);
        }
        if (song == "gameplay_variant22") {
            SetDualMusic(gameplay_variant2_stage5, gameplay_variant2_stage2, true, pitch);
        }
        if (song == "gameplay_variant23") {
            SetDualMusic(gameplay_variant2_stage7, gameplay_variant1_stage3, true, pitch);
        }

        if (song == "gameplay_variant24") {
            SetDualMusic(gameplay_variant1_stage1, gameplay_variant2_stage4, true, pitch);
        }
        if (song == "gameplay_variant25") {
            SetDualMusic(gameplay_variant4_stage3, gameplay_variant4_stage3, true, pitch);
        }
        if (song == "gameplay_variant26") {
            SetDualMusic(gameplay_variant2_stage3, gameplay_variant2_stage6, true, pitch);
        }
        if (song == "gameplay_variant27") {
            SetDualMusic(gameplay_variant2_stage5, gameplay_variant1_stage2, true, pitch);
        }
        if (song == "gameplay_variant28") {
            SetDualMusic(gameplay_variant2_stage7, gameplay_variant2_stage2, true, pitch);
        }

        if (song == "gameplay_variant29") {
            SetDualMusic(gameplay_variant1_stage1, gameplay_variant1_stage2, true, pitch);
        }
        if (song == "gameplay_variant30") {
            // gameplay_variant4_stage4?
            SetDualMusic(gameplay_variant4_stage5, gameplay_variant4_stage6, true, pitch);
        }
        if (song == "gameplay_variant31") {
            SetDualMusic(credits, gameplay_variant2_stage8, true, pitch);
        }
        if (song == "gameplay_variant62") {
            SetDualMusic(credits_broken_heaven, gameplay_variant2_stage8, true, pitch);
        }
    }

    public void MuteMusic() {
        music_muted = true;
        background_music1.outputAudioMixerGroup.audioMixer.SetVolume("volume", 0f);
    }

    public void UnmuteMusic() {
        music_muted = false;
        background_music1.outputAudioMixerGroup.audioMixer.SetVolume("volume", music_volume);
    }

    public void SetVolume(float volume) {
        music_volume = volume;

        if (music_muted)
            return;

        background_music1.outputAudioMixerGroup.audioMixer.SetVolume("volume", music_volume);
    }

    public void SetDualMusic(AudioClip clip1, AudioClip clip2, bool loop, float pitch = 1f) {
        AudioSource layer1 = background_music2;
        AudioSource layer2 = background_music1;

        if (!track1_playing) {
            track1_playing = true;
            layer1 = background_music1;
            layer2 = background_music2;
        }
        else {
            track1_playing = false;
        }

        layer1.clip = clip1;
        layer1.Play();
        layer1.loop = loop;
        layer1.pitch = pitch;

        layer2.clip = clip2;
        layer2.Play();
        layer2.loop = loop;
        layer2.pitch = pitch;
        layer2.volume = 0;

        StartCoroutine(FadeIn(layer1));
    }

    public float CurrentPlayTime() {
        return background_music1.time;
    }

    public void SwitchLayer() {
        if (track1_playing) {
            track1_playing = false;
            StartCoroutine(FadeOut(background_music1));
            StartCoroutine(FadeIn(background_music2));
        }
        else {
            track1_playing = true;
            StartCoroutine(FadeIn(background_music1));
            StartCoroutine(FadeOut(background_music2));
        }
    }

    public void SetMusic(AudioClip clip, bool loop, float pitch = 1f) {
        if (track1_playing) {
            track1_playing = false;
            background_music2.clip = clip;
            background_music2.Play();
            background_music2.loop = loop;
            background_music2.pitch = pitch;
            StartCoroutine(FadeOut(background_music1));
            StartCoroutine(FadeIn(background_music2));
        }
        else {
            track1_playing = true;
            background_music1.clip = clip;
            background_music1.Play();
            background_music1.loop = loop;
            background_music1.pitch = pitch;
            StartCoroutine(FadeOut(background_music2));
            StartCoroutine(FadeIn(background_music1));
        }
    }

    public void FadeOutAll() {
        StartCoroutine(FadeOut(background_music1));
        StartCoroutine(FadeOut(background_music2));
    }

    IEnumerator FadeOut(AudioSource source) {
        while (source.volume > 0) {
            source.volume = source.volume - 0.1f;

            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator FadeIn(AudioSource source) {
        while (source.volume < 1f) {
            source.volume = 1;

            yield return new WaitForSeconds(0.01f);
        }
    }
}
