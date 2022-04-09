using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class SoundMan : MonoBehaviour {
	public GameObject sounds_source;
    public AudioMixerGroup fx_mixer_group;

    public float scale = Mathf.Pow(2f, 1.0f / 12f);

    public class AudioSourceData {
        
        public AudioSource audio_source { get; set; }
        public IEnumerator audio_source_numerator { get; set; }
    }

    public List<AudioSourceData> audio_sources = new List<AudioSourceData>();

    int current_audio_source = 0;
    int audio_source_limit = 30;

    float effects_volume = 0.6f;
    bool effects_muted = false;

    string last_sound_file;
    float last_sound_file_played;

    // Use this for initialization
    void Awake() {
        for (int i = 0; i < audio_source_limit; i++) {
            AudioSource newsource = sounds_source.AddComponent<AudioSource>();
            newsource.outputAudioMixerGroup = fx_mixer_group;

            audio_sources.Add(new AudioSourceData {
                audio_source = newsource
            });
        }
        last_sound_file_played = Time.time;
    }

    public void MuteEffects() {
        effects_muted = true;
        fx_mixer_group.audioMixer.SetVolume("volume", 0f);
    }

    public void UnmuteEffects() {
        effects_muted = false;
        fx_mixer_group.audioMixer.SetVolume("volume", effects_volume);
    }

    public void SetVolume(float volume) {
        effects_volume = volume;

        if (effects_muted)
            return;

        fx_mixer_group.audioMixer.SetVolume("volume", effects_volume);
    }

    public void play_sound(string sound, int pitch_amount = 0, float volume = 1f, float pan = 0f) {

        AudioSourceData previous_sound = audio_sources[current_audio_source];

        if (sound == last_sound_file) {
            // if it's the same sound played twice in a row, it's okay if some time has passed
            if (Time.time - last_sound_file_played < 0.1f) {
                return;
            }
        }

        last_sound_file = sound;
        last_sound_file_played = Time.time;

        if (previous_sound.audio_source_numerator != null) {
            StopCoroutine(previous_sound.audio_source_numerator);
        }

        previous_sound.audio_source_numerator = fade_out_sound(audio_sources[current_audio_source].audio_source);

        StartCoroutine(previous_sound.audio_source_numerator);

        current_audio_source++;

        if (current_audio_source >= audio_source_limit) {
            current_audio_source = 0;
        }

        AudioSource sound_player = audio_sources[current_audio_source].audio_source;
        sound_player.volume = volume;

        sound_player.pitch = Mathf.Pow(scale, pitch_amount);

        sound_player.Stop();
        AudioClip clip = Resources.Load<AudioClip>("sounds/" + sound);
        sound_player.clip = clip;
        sound_player.panStereo = calculatePan(pan);
        sound_player.Play();
	}

    float calculatePan(float pan) {
        if (pan >= -1f && pan < 1f) {
            return pan;
        }

        // if pan is greater than -1 / 1, calc from total x -11 / 11
        float absolutePan = Mathf.Abs(pan);
        float percentPan = absolutePan / 11f;

        if (pan < 0) {
            return 0 - percentPan;
        }

        return percentPan;
    }

    IEnumerator fade_out_sound(AudioSource the_source) {
        float t = 1;
        while (t > 0.0f) {
            t -= Time.deltaTime;
            the_source.volume = t;
            yield return new WaitForSeconds(0);
        }
        the_source.volume = 0.0f;
        the_source.Stop();
    }
}

namespace UnityEngine.Audio
{
    public static class AudioExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mixer"></param>
        /// <param name="exposedName">The name of 'The Exposed to Script' variable</param>
        /// <param name="value">value must be between 0 and 1</param>
        public static void SetVolume(this AudioMixer mixer, string exposedName, float value) {
            mixer.SetFloat(exposedName, VolumeForValue(value));
        }

        public static float VolumeForValue(float value) {
            float newvalue = Mathf.Round(0f + (value - 1) * 25f);
            if (value == 0) {
                newvalue = -80f;
            }
            return newvalue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="mixer"></param>
        /// <param name="exposedName">The name of 'The Exposed to Script' variable</param>
        /// <returns></returns>
        public static float GetVolume(this AudioMixer mixer, string exposedName) {
            if (mixer.GetFloat(exposedName, out float volume)) {
                return Mathf.InverseLerp(-80.0f, 0.0f, volume);
            }

            return 0f;
        }
    }
}