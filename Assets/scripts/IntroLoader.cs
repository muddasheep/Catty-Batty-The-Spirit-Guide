using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroLoader : MonoBehaviour
{
    public Animator agameby;
    public Animator philipplehner;
    public Animator floor;

    private void Start() {
        StartCoroutine(Loader(this));
    }

    private static IEnumerator Loader(IntroLoader self) {
        yield return new WaitForSeconds(1f);

        self.agameby.enabled = true;
        self.agameby.GetComponent<SpriteRenderer>().enabled = true;

        yield return new WaitForSeconds(1f);

        self.philipplehner.enabled = true;
        self.philipplehner.GetComponent<SpriteRenderer>().enabled = true;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Splash");
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone) {
            if (asyncLoad.progress >= 0.9f) {
                break;
            }

            yield return null;
        }

        MusicInstructor instructor = FindObjectOfType<MusicInstructor>();
        instructor.PlayMusic("main_theme");

        yield return new WaitForSeconds(1.9f);

        self.agameby.StopPlayback();
        self.agameby.Play("intro_fadeout", -1, 0);
        self.philipplehner.StopPlayback();
        self.philipplehner.Play("intro_fadeout", -1, 0);
        self.floor.Play("intro_moveout", -1, 0);

        yield return new WaitForSeconds(0.5f);

        asyncLoad.allowSceneActivation = true;
    }
}
