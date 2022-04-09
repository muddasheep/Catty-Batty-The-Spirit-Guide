using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritWaitingRoomDisplay : MonoBehaviour
{
    SpiritWaitingRoomDisplayCorner[] corners;
    SpiritWaitingRoomDisplayLine[] lines;
    SpiritWaitingRoomDisplayNoise[] noises;
    SpiritWaitingRoomDisplayPicture picture;

    public Sprite[] spiritpictures;
    public int current_spirit = -1;

    private void Start() {
        corners = transform.GetComponentsInChildren<SpiritWaitingRoomDisplayCorner>();
        lines = transform.GetComponentsInChildren<SpiritWaitingRoomDisplayLine>();
        noises = transform.GetComponentsInChildren<SpiritWaitingRoomDisplayNoise>();
        picture = transform.GetComponentInChildren<SpiritWaitingRoomDisplayPicture>();

        revealBorder();
    }

    void revealBorder() {
        // draw corners
        foreach (SpiritWaitingRoomDisplayCorner corner in corners) {
            Animator cornimator = corner.GetComponentInChildren<Animator>();
            StartCoroutine(PlayDelayed(cornimator, "spiritwaitingroomdisplaycorner", Random.Range(0, 0.3f)));
        }

        // draw lines
        foreach (SpiritWaitingRoomDisplayLine line in lines) {
            Animator cornimator = line.GetComponentInChildren<Animator>();
            StartCoroutine(PlayDelayed(cornimator, "spiritwaitingroomdisplayline", Random.Range(0, 0.3f)));
        }
    }

    void removeBorder() {
        // remove corners
        foreach (SpiritWaitingRoomDisplayCorner corner in corners) {
            Animator cornimator = corner.GetComponentInChildren<Animator>();
            StartCoroutine(PlayDelayed(cornimator, "spiritwaitingroomdisplaycorner_reverse", Random.Range(1f, 1.3f)));
        }

        // remove lines
        foreach (SpiritWaitingRoomDisplayLine line in lines) {
            Animator cornimator = line.GetComponentInChildren<Animator>();
            StartCoroutine(PlayDelayed(cornimator, "spiritwaitingroomdisplayline_reverse", Random.Range(1f, 1.3f)));
        }
    }

    public void nextSpirit() {
        current_spirit++;

        displayCurrentSpirit();
    }

    void displayCurrentSpirit() {

        // draw noise
        foreach (SpiritWaitingRoomDisplayNoise noise in noises) {
            Animator cornimator = noise.GetComponentInChildren<Animator>();
            StartCoroutine(PlayDelayed(cornimator, "spiritwaitingroomdisplaynoise", Random.Range(0.1f, 0.4f)));
        }

        // hide picture
        StartCoroutine(HidePicture(picture, 0.5f));

        // all done
        if (current_spirit >= spiritpictures.Length) {
            removeBorder();
        }
        else {
            // swap + display pic
            StartCoroutine(DisplayPicture(picture, spiritpictures[current_spirit], 1.2f));
        }

        // remove noise
        foreach (SpiritWaitingRoomDisplayNoise noise in noises) {
            Animator cornimator = noise.GetComponentInChildren<Animator>();
            StartCoroutine(PlayDelayed(cornimator, "spiritwaitingroomdisplaynoise_reverse", Random.Range(1f, 1.4f)));
        }
    }

    private static IEnumerator DisplayPicture(SpiritWaitingRoomDisplayPicture picture, Sprite sprite, float delay) {
        float start = Time.time;
        while (Time.time <= start + delay) {
            yield return null;
        }
        SpriteRenderer picture_sr = picture.GetComponent<SpriteRenderer>();
        picture_sr.sprite = sprite;
        picture_sr.enabled = true;
        float alpha = 0;
        while (alpha <= 1f) {
            picture_sr.color = new Color(255, 255, 255, alpha);
            alpha += 0.1f;
            yield return null;
        }
        picture_sr.color = new Color(255, 255, 255, 1);
    }

    private static IEnumerator HidePicture(SpiritWaitingRoomDisplayPicture picture, float delay) {
        float start = Time.time;
        while (Time.time <= start + delay) {
            yield return null;
        }
        SpriteRenderer picture_sr = picture.GetComponent<SpriteRenderer>();
        if (picture_sr.enabled == false) {
            yield break;
        }
        picture_sr.enabled = true;
        float alpha = 1;
        while (alpha > 0f) {
            picture_sr.color = new Color(255, 255, 255, alpha);
            alpha -= 0.1f;
            yield return null;
        }
        picture_sr.color = new Color(255, 255, 255, 0);
    }


    private static IEnumerator PlayDelayed(Animator animator, string state_to_play, float delay) {
        float start = Time.time;
        while (Time.time <= start + delay) {
            yield return null;
        }
        animator.enabled = true;
        animator.StopPlayback();
        animator.Play(state_to_play);
    }

}
