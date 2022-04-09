using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashTransitionHeart : MonoBehaviour
{
    public Animator ar;
    public SplashTransitionHeartSmall[] smallhearts;

    public void SlideIn() {
        ar.enabled = true;
        transform.position = new Vector3(
            transform.position.x,
            0,
            transform.position.z
        );
        ar.Play("splashtransitionheart_in", -1, 0);
        List<SplashTransitionHeartSmall> chosenhearts = new List<SplashTransitionHeartSmall>();
        foreach (SplashTransitionHeartSmall heart in smallhearts) {
            if (!heart.chosen && chosenhearts.Count < 2) {
                chosenhearts.Add(heart);
                heart.chosen = true;
                heart.sr.enabled = true;
            }
        }

        StartCoroutine(PaintHearts(this, chosenhearts));
    }

    private static IEnumerator PaintHearts(SplashTransitionHeart self, List<SplashTransitionHeartSmall> chosenhearts) {
        foreach (SplashTransitionHeartSmall heart in chosenhearts) {
            float start = Time.time;
            while (Time.time <= start + Random.Range(0.3f, 0.5f)) {
                yield return null;
            }
            Transform child = self.transform.GetChild(0).GetChild(2);
            heart.transform.position = new Vector3(
                child.position.x - Random.Range(0, 0.1f) + Random.Range(0, 0.2f),
                child.position.y,
                child.position.z
            );
            if (self.transform.rotation.z > 0) {
                heart.transform.Rotate(0, 0, 140f + Random.Range(0, 80f), Space.Self);
            }
            else {
                heart.transform.Rotate(0, 0, Random.Range(0, 40f) - Random.Range(0, 80f), Space.Self);
            }
            heart.moveit();
        }
    }

    public void SlideOut() {
        StopAllCoroutines();
        ar.Play("splashtransitionheart_out", -1, 0);
    }
}
