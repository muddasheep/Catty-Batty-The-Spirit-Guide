using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenMaster : MonoBehaviour
{
    public GameObject[] poof_catty;
    public GameObject[] poof_and;
    public GameObject[] poof_batty;
    public GameObject[] poof_box1;
    public GameObject[] poof_box2;

    public SpriteRenderer title_catty;
    public SpriteRenderer title_and;
    public SpriteRenderer title_batty;

    public Vector3 title_catty_startpos;
    public Vector3 title_and_startpos;
    public Vector3 title_batty_startpos;

    public SplashTitle[] splash_titles;

    public Animator[] headervines;

    public Animator[] splash_lines;

    public Transform linerotator;

    public Animator splashkitty;
    public Animator splashbitty;
    public Animator splashbox1;
    public Animator splashbox2;

    public SplashTransitionHeart[] splash_transition_hearts;

    public Animator forauri;

    public Animator splash_continue;
    public Animator splash_continue_spirit;

    Coroutine poofroutine;
    Coroutine boxroutine;

    public MusicInstructor musicinstructor;

    bool has_clicked = false;

    void Awake() {
        SplashScreenMaster[] masters = GameObject.FindObjectsOfType<SplashScreenMaster>();

        if (masters.Length > 1) {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        poofroutine = StartCoroutine(Poofs(poof_catty, poof_and, poof_batty, this));
        boxroutine = StartCoroutine(Boxes(this));
        title_catty_startpos = title_catty.transform.position;
        title_and_startpos = title_and.transform.position;
        title_batty_startpos = title_batty.transform.position;
        musicinstructor = FindObjectOfType<MusicInstructor>();
    }

    private static IEnumerator Boxes(SplashScreenMaster self) {
        yield return new WaitForSeconds(1.89f);

        self.splashbox1.enabled = true;

        yield return new WaitForSeconds(0.42f);

        foreach (GameObject poff in self.poof_box1) {
            poff.GetComponent<SpriteRenderer>().enabled = true;
            Animator ar = poff.GetComponent<Animator>();
            ar.enabled = true;
        }
        self.musicinstructor.soundman.play_sound("kick5");

        self.splashbox2.enabled = true;

        yield return new WaitForSeconds(0.5f);

        foreach (GameObject poff in self.poof_box2) {
            poff.GetComponent<SpriteRenderer>().enabled = true;
            Animator ar = poff.GetComponent<Animator>();
            ar.enabled = true;
        }
        self.musicinstructor.soundman.play_sound("kick6");
    }
    private static IEnumerator Poofs(GameObject[] poof_catty, GameObject[] poof_and, GameObject[] poof_batty, SplashScreenMaster self) {
        float start = Time.time;
        while (Time.time <= start + 0.1f) {
            yield return null;
        }

        self.title_catty.enabled = true;
        self.splashkitty.enabled = true;
        poof_catty[0].transform.parent.position = self.title_catty.transform.position;
        foreach (GameObject poff in poof_catty) {
            poff.GetComponent<SpriteRenderer>().enabled = true;
            Animator ar = poff.GetComponent<Animator>();
            ar.enabled = true;
        }

        while (Time.time <= start + 0.6f) {
            yield return null;
        }

        self.title_and.enabled = true;
        poof_and[0].transform.parent.position = self.title_and.transform.position;
        foreach (GameObject poff in poof_and) {
            poff.GetComponent<SpriteRenderer>().enabled = true;
            Animator ar = poff.GetComponent<Animator>();
            ar.enabled = true;
        }

        while (Time.time <= start + 1.1f) {
            yield return null;
        }

        self.title_batty.enabled = true;
        self.splashbitty.enabled = true;
        poof_batty[0].transform.parent.position = self.title_batty.transform.position;
        foreach (GameObject poff in poof_batty) {
            poff.GetComponent<SpriteRenderer>().enabled = true;
            Animator ar = poff.GetComponent<Animator>();
            ar.enabled = true;
        }

        while (Time.time <= start + 1f) {
            yield return null;
        }

        foreach (Animator vine in self.headervines) {
            vine.enabled = true;
            vine.GetComponent<SpriteRenderer>().enabled = true;
        }

        while (Time.time <= start + 1.2f) {
            yield return null;
        }

        foreach (SplashTitle title in self.splash_titles) {
            title.ActivateMe();
        }

        while (Time.time <= start + 1.3f) {
            yield return null;
        }

        foreach (Animator line in self.splash_lines) {
            line.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }

        self.splash_continue.enabled = true;
        self.splash_continue.GetComponent<SpriteRenderer>().enabled = true;
        self.splash_continue_spirit.enabled = true;
        self.splash_continue_spirit.GetComponent<SpriteRenderer>().enabled = true;

        yield return new WaitForSeconds(0.5f);

        TextMeshPro presstext = self.splash_continue.GetComponentInChildren<TextMeshPro>();
        string to_write = presstext.text;
        presstext.text = "";
        presstext.enabled = true;

        foreach (char character in to_write.ToCharArray()) {
            presstext.text += character;
            yield return new WaitForSeconds(0.05f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (title_catty != null) {
            title_catty.transform.position = new Vector3(
                title_catty.transform.position.x,
                title_catty_startpos.y + Mathf.Sin(Time.time) * 0.5f,
                title_catty.transform.position.z
            );
            title_and.transform.position = new Vector3(
                title_and.transform.position.x,
                title_and_startpos.y + Mathf.Sin(Time.time + 2f) * 0.5f,
                title_and.transform.position.z
            );
            title_batty.transform.position = new Vector3(
                title_batty.transform.position.x,
                title_batty_startpos.y + Mathf.Sin(Time.time + 4f) * 0.5f,
                title_batty.transform.position.z
            );

            linerotator.Rotate(0, 0, -0.1f, Space.Self);
        }
    }

    private void Update() {
        if (Input.anyKeyDown) {
            if (!has_clicked) {
                TransitionNow();
                has_clicked = true;
            }
        }
    }

    void TransitionNow() {
        StartCoroutine(TransitionTimer(this));
    }

    private static IEnumerator TransitionTimer(SplashScreenMaster self) {
        self.StopCoroutine(self.poofroutine);
        self.StopCoroutine(self.boxroutine);
        self.musicinstructor.PlayMusic("for_auri");
        self.musicinstructor.soundman.play_sound("click1");

        float start = Time.time;
        while (Time.time <= start + 0.1f) {
            yield return null;
        }

        foreach (SplashTransitionHeart heart in self.splash_transition_hearts) {
            heart.SlideIn();
            yield return new WaitForSeconds(0.05f);
        }

        while (Time.time <= start + 1f) {
            yield return null;
        }
        self.forauri.enabled = true;
        self.forauri.GetComponent<SpriteRenderer>().enabled = true;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("overworld");
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone) {
            if (asyncLoad.progress >= 0.9f) {
                break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(2f);

        self.forauri.StopPlayback();
        self.forauri.Play("for_auri_out", -1, 0);

        foreach (SplashTransitionHeart heart in self.splash_transition_hearts) {
            heart.StopAllCoroutines();
        }

        yield return new WaitForSeconds(0.5f);

        asyncLoad.allowSceneActivation = true;

        foreach (SplashTransitionHeart heart in self.splash_transition_hearts) {
            heart.SlideOut();
            yield return new WaitForSeconds(0.05f);
        }

        Destroy(self.gameObject, 1f);
    }
}
