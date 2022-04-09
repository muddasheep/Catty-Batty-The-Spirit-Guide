using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Rewired;
using System.Net.Security;

public class DialogueManager : MonoBehaviour
{
    private UniverseMaster universemaster;
    public TextMeshPro dialogueText;
    public GameObject speechbubbly_batty;
    public GameObject speechbubbly_catty;
    public GameObject speechbubbly_hedgar;
    public GameObject star;
    public Camera main_camera;
    public Light ambient_light;
    private Player player_1;
    private Player player_2;

    private Queue<DialogueSentence> sentences;
    private Dialogue current_dialogue;
    private DialogueSentence current_sentence;
    private GameObject last_target_object;
    MusicInstructor instructor;
    public bool transitioning = false;

    bool horizontal_down_player_1 = false;
    bool horizontal_down_player_2 = false;
    bool vertical_down_player_1 = false;
    bool vertical_down_player_2 = false;

    bool square_down = false;
    float square_down_pressed;
    float square_down_pressed_start;
    ProgressBear progressbear;

    Coroutine sentencetype;

    // Start is called before the first frame update
    void Awake()
    {
        sentences = new Queue<DialogueSentence>();
        progressbear = FindObjectOfType<ProgressBear>();

        player_1 = ReInput.players.GetPlayer(0);
        player_1.AddInputEventDelegate(OnStartUpdate, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Start");
        player_1.AddInputEventDelegate(OnFireUpdateOnce, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Fire");
        player_1.AddInputEventDelegate(OnSquareUpdate, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "Square");
        player_1.AddInputEventDelegate(OnCancelUpdateOnce, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Cancel");

        player_1.AddInputEventDelegate(OnMoveHorizontalPlayer1, UpdateLoopType.Update, InputActionEventType.AxisActive, "Move Horizontal");
        player_1.AddInputEventDelegate(OnMoveVerticalPlayer1, UpdateLoopType.Update, InputActionEventType.AxisActive, "Move Vertical");
        player_1.AddInputEventDelegate(OnUpUpdate, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Up");
        player_1.AddInputEventDelegate(OnDownUpdate, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Down");
        player_1.AddInputEventDelegate(OnLeftUpdate, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Left");
        player_1.AddInputEventDelegate(OnRightUpdate, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Right");

        player_2 = ReInput.players.GetPlayer(1);
        player_2.AddInputEventDelegate(OnStartUpdate, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Start");
        player_2.AddInputEventDelegate(OnFireUpdateOnce, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Fire");
        player_2.AddInputEventDelegate(OnSquareUpdate, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "Square");
        player_2.AddInputEventDelegate(OnCancelUpdateOnce, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Cancel");

        player_2.AddInputEventDelegate(OnMoveHorizontalPlayer2, UpdateLoopType.Update, InputActionEventType.AxisActive, "Move Horizontal");
        player_2.AddInputEventDelegate(OnMoveVerticalPlayer2, UpdateLoopType.Update, InputActionEventType.AxisActive, "Move Vertical");
        player_2.AddInputEventDelegate(OnUpUpdate, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Up");
        player_2.AddInputEventDelegate(OnDownUpdate, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Down");
        player_2.AddInputEventDelegate(OnLeftUpdate, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Left");
        player_2.AddInputEventDelegate(OnRightUpdate, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Right");

        setUniverseMaster();

        instructor = universemaster.GetOrCreateMusicInstructor();
    }

    IEnumerator Start() {
        yield return new WaitForSeconds(0.5f);
        universemaster.checkForDarkMode();
    }

    private void FixedUpdate() {
        if (transitioning) {
            return;
        }

        if (square_down && Time.time - square_down_pressed < 0.2f) {
            float time_difference = Time.time - square_down_pressed_start;
            float time_needed = 1f;
            float percent = time_difference / time_needed * 100f;
            progressbear.DisplayPercent(percent);
            
            if (percent >= 100f) {
                progressbear.Done();
                loadNextScene();
            }
        }
        else {
            square_down = false;
            progressbear.Hide();
        }
    }

    void OnMoveHorizontalPlayer1(InputActionEventData data) {
        if (data.GetAxis() > -0.5f && data.GetAxis() < 0.5f)
            return;

        if (horizontal_down_player_1 == true) {
            return;
        }
        horizontal_down_player_1 = true;

        horizontal_move(data.GetAxis());
    }
    void horizontal_move(float axis) {
        if (transitioning)
            return;

        if (Time.timeSinceLevelLoad < 1f) {
            return;
        }

        if (universemaster.is_in_options) {
            if (axis < 0) {
                universemaster.options.left_item();
            }
            else {
                universemaster.options.right_item();
            }
            return;
        }
    }

    void vertical_move(float axis) {
        if (transitioning)
            return;

        if (Time.timeSinceLevelLoad < 1f) {
            return;
        }

        if (universemaster.is_paused) {
            if (axis > -0.4f && axis < 0.4f)
                return;

            if (universemaster.is_in_options) {
                if (axis < 0) {
                    universemaster.options.next_item();
                }
                else {
                    universemaster.options.prev_item();
                }
            }
            else {
                if (axis < 0) {
                    universemaster.pause.next_item();
                }
                else {
                    universemaster.pause.prev_item();
                }
            }

            return;
        }
    }

    void OnUpUpdate(InputActionEventData data) {
        vertical_move(1f);
    }

    void OnDownUpdate(InputActionEventData data) {
        vertical_move(-1f);
    }

    void OnLeftUpdate(InputActionEventData data) {
        horizontal_move(-1f);
    }

    void OnRightUpdate(InputActionEventData data) {
        horizontal_move(1f);
    }

    void OnMoveHorizontalPlayer2(InputActionEventData data) {
        if (data.GetAxis() > -0.5f && data.GetAxis() < 0.5f)
            return;

        if (horizontal_down_player_2 == true) {
            return;
        }
        horizontal_down_player_2 = true;

        horizontal_move(data.GetAxis());
    }

    void OnMoveVerticalPlayer1(InputActionEventData data) {
        if (data.GetAxis() > -0.5f && data.GetAxis() < 0.5f)
            return;

        if (vertical_down_player_1 == true) {
            return;
        }
        vertical_down_player_1 = true;

        vertical_move(data.GetAxis());
    }

    void OnMoveVerticalPlayer2(InputActionEventData data) {
        if (data.GetAxis() > -0.5f && data.GetAxis() < 0.5f)
            return;

        if (vertical_down_player_2 == true) {
            return;
        }
        vertical_down_player_2 = true;

        vertical_move(data.GetAxis());
    }

    void setUniverseMaster() {
        UniverseMaster[] masters = FindObjectsOfType<UniverseMaster>();
        foreach (UniverseMaster master in masters) {
            if (master.is_singleton) {
                universemaster = master;
                return;
            }
        }

        universemaster = null;
    }

    void OnStartUpdate(InputActionEventData data) {
        if (transitioning)
            return;

        if (universemaster.is_in_options) {
            universemaster.options.fireCancel();
            return;
        }

        togglePause();
    }

    public void togglePause() {
        if (universemaster.is_paused) {
            unpauseGame();
        }
        else {
            pauseGame();
        }
    }

    public void pauseGame() {
        DialogueManager dialogmanager = FindObjectOfType<DialogueManager>();

        if (dialogmanager != null && dialogmanager.transitioning) {
            return;
        }
        universemaster.pause.init_pause_screen();
        universemaster.is_paused = true;
    }

    public void unpauseGame() {
        universemaster.pause.hide_pause_screen();
        universemaster.is_paused = false;
    }

    public void toggleOptions() {
        if (universemaster.is_in_options) {
            closeOptions();
        }
        else {
            openOptions();
        }
    }

    public void openOptions() {
        universemaster.options.ShowOptions();
        universemaster.is_in_options = true;
    }

    public void closeOptions() {
        universemaster.options.HideOptions();
        universemaster.is_in_options = false;
    }

    void OnSquareUpdate(InputActionEventData data) {
        square_down_pressed = Time.time;
        if (square_down == false) {
            square_down = true;
            square_down_pressed_start = Time.time;
        }
    }

    void OnFireUpdateOnce(InputActionEventData data) {
        if (transitioning)
            return;

        if (universemaster.is_paused) {
            if (universemaster.is_in_options) {
                universemaster.options.call_action_of_active_menu_item();
            }
            else {
                universemaster.pause.call_action_of_active_pause_menu_item();
            }
            return;
        }
        DisplayNextSentence();
    }

    void OnCancelUpdateOnce(InputActionEventData data) {
        if (transitioning)
            return;

        if (universemaster.is_in_options) {
            universemaster.options.fireCancel();
            return;
        }

        if (universemaster.is_paused) {
            unpauseGame();
            return;
        }
    }

    public void StartDialogue(Dialogue dialogue) {
        instructor.PlayMusic("dialog1");

        sentences.Clear();

        current_dialogue = dialogue;
        last_target_object = null;

        foreach (DialogueSentence sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if (transitioning)
            return;

        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }

        current_sentence = sentences.Dequeue();

        DisplayCurrentSentence();

        ChangeEmotion(current_sentence);

        PositionSpeechBubbly(current_sentence);

        HideSpriteWithName(current_sentence);

        HideLastTargetObject(current_sentence);

        ShowSpeechBubblyNext(current_sentence);

        instructor.soundman.play_sound("slide5");
    }

    public void DisplayCurrentSentence() {
        if (current_sentence == null) {
            return;
        }
        if (sentencetype != null) {
            StopCoroutine(sentencetype);
        }
        string text = current_sentence.text;
        if (current_sentence.KeyID != null) {
            text = I2.Loc.LocalizationManager.GetTranslation(current_sentence.KeyID);
            if (I2.Loc.LocalizationManager.CurrentLanguageCode == "ja") {
                text = text.Replace(" ", "\n"); // japanese carriage return
            }
        }
        sentencetype = StartCoroutine(TypeSentence(text));

        if (current_sentence.function_name != "") {
            BroadcastMessage(current_sentence.function_name);
        }
    }

    void ChangeEmotion(DialogueSentence sentence) {
        if (sentence.target_object != null) {
            dialog_actor actor = sentence.target_object.GetComponent<dialog_actor>();
            if (actor) {
                actor.changeEmotion(sentence.emotion);
            }
        }
    }

    void ShowSpeechBubblyNext(DialogueSentence sentence) {
        if (sentence.target_object != null && sentence.target_object.name.Contains("batty")) {
            ShowSprite(speechbubbly_batty.GetComponent<SpriteRenderer>());
            HideSprite(speechbubbly_catty.GetComponent<SpriteRenderer>());
            if (speechbubbly_hedgar != null) {
                HideSprite(speechbubbly_hedgar.GetComponent<SpriteRenderer>());
            }
        }
        else if (sentence.target_object != null && sentence.target_object.name.Contains("hedgar")) {
            if (speechbubbly_hedgar != null) {
                ShowSprite(speechbubbly_hedgar.GetComponent<SpriteRenderer>());
            }
            HideSprite(speechbubbly_catty.GetComponent<SpriteRenderer>());
            HideSprite(speechbubbly_batty.GetComponent<SpriteRenderer>());
        }
        else {
            ShowSprite(speechbubbly_catty.GetComponent<SpriteRenderer>());
            HideSprite(speechbubbly_batty.GetComponent<SpriteRenderer>());
            if (speechbubbly_hedgar != null) {
                HideSprite(speechbubbly_hedgar.GetComponent<SpriteRenderer>());
            }
        }
    }

    void HideLastTargetObject(DialogueSentence sentence) {
        if (last_target_object != null && sentence.hide_last_target_object) {
            last_target_object.GetComponent<SpriteRenderer>().color = new Color(1F, 1F, 1F, 0);
        }
        last_target_object = sentence.target_object;
    }

    void HideSpriteWithName(DialogueSentence sentence) {
        if (sentence.hide_sprite_with_name != "") {
            GameObject victim = GameObject.Find(sentence.hide_sprite_with_name);
            SpriteRenderer victim_renderer = victim.GetComponent<SpriteRenderer>();
            HideSprite(victim_renderer);
        }
    }

    void ShowSprite(SpriteRenderer sprite) {
        sprite.color = new Color(1F, 1F, 1F, 1F);
    }

    void HideSprite(SpriteRenderer sprite) {
        sprite.color = new Color(1F, 1F, 1F, 0);
    }

    void PositionSpeechBubbly(DialogueSentence sentence) {
        if (sentence.target_object != null) {
            SpriteRenderer sr = sentence.target_object.GetComponent<SpriteRenderer>();

            float new_y = sentence.target_object.transform.position.y
                + sr.bounds.size.y
                + Mathf.Clamp(sr.sprite.bounds.center.y, 0, 2f);

            Vector3 new_position = new Vector3(
                sentence.target_object.transform.position.x,
                new_y,
                sentence.target_object.transform.position.z
            );

            dialogueText.transform.parent.transform.position = new_position;
            ShowSprite(sentence.target_object.GetComponent<SpriteRenderer>());
        }
        else {
            dialogueText.transform.parent.transform.position = sentence.target_position;
        }
    }

    IEnumerator TypeSentence(string sentence) {
        dialogueText.text = "";
        int letter_count = 1;
        foreach (char letter in sentence.ToCharArray()) {
            string oldtext = sentence.Substring(0, letter_count);
            string newtext = sentence.Substring(letter_count);
            dialogueText.text = oldtext + "<color=#FFFFFF>" + newtext + "</color>";
            letter_count++;
            yield return new WaitForSeconds(0.01f);
        }
    }

    void EndDialogue() {
        StopAllCoroutines();
        if (current_dialogue.nextScene != null) {
            loadNextScene();
        }
    }

    void loadNextScene() {
        if(transitioning == true) {
            return;
        }

        transitioning = true;
        instructor.FadeOutAll();

        instructor.soundman.play_sound("slide7");

        StartCoroutine(LoadNextSceneSlowly(current_dialogue.nextScene));
    }

    IEnumerator LoadNextSceneSlowly(string nextscene) {

        TransitionGradient tr = FindObjectOfType<TransitionGradient>();
        if (tr != null) {
            tr.FadeIn();
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(current_dialogue.nextScene);
        asyncLoad.allowSceneActivation = false;

        float starttime = Time.time;

        while (!asyncLoad.isDone) {
            if (asyncLoad.progress >= 0.9f) {
                break;
            }

            yield return null;
        }

        while (Time.time - starttime < 1.5f) {
            yield return null;
        }

        asyncLoad.allowSceneActivation = true;
    }

    void showSpirits() {
        int spirit_count = 1;
        while (spirit_count <= 8) {
            GameObject spirit = GameObject.Find("cs_spirit" + spirit_count.ToString());

            Bounce spiritscript = spirit.GetComponent<Bounce>();
            spiritscript.myoffset = Mathf.Floor(spirit_count) * 2f;

            StartCoroutine(ActivateAnimatorDelayed(spirit.GetComponent<Animator>(), spirit_count / 20f));
            spirit_count++;
        }
    }

    private static IEnumerator ActivateAnimatorDelayed(Animator animator, float delay) {
        yield return new WaitForSeconds(delay);
        animator.enabled = true;
    }

    public void reset_level() {
        StopAllCoroutines();
        player_1.ClearInputEventDelegates();
        player_2.ClearInputEventDelegates();
        universemaster.reset_level();
    }

    void cattyFliesAway() {
        GameObject catty = GameObject.Find("dialog_catty");
        catty.GetComponent<Animator>().enabled = true;

        GameObject portal_big = GameObject.Find("portal_big");
        StartCoroutine(FadeInSprite(portal_big.GetComponentInChildren<SpriteRenderer>(), 0.5F));
        instructor.soundman.play_sound("slide2", Random.Range(-2, 2), volume: 1f);
    }

    void showHeartVine() {
        GameObject vine = GameObject.Find("heartvine");
        StartCoroutine(FadeInSprite(vine.GetComponent<SpriteRenderer>(), 1F, 0F));
    }

    void showSpiritsThroughHeartVine() {
        Bounce[] bouncers = FindObjectsOfType<Bounce>();

        float delay = 0;
        foreach (Bounce bounce in bouncers) {
            StartCoroutine(StartAnimatorDelayed(bounce, delay));
            delay += 1f;
        }
    }

    private static IEnumerator StartAnimatorDelayed(Bounce bounce, float delay) {
        yield return new WaitForSeconds(delay);

        bounce.transform.parent.GetComponent<Animator>().enabled = true;
    }

    void startRain() {
        int starCount = 0;
        while (starCount < 30) {
            GameObject newstar = (GameObject)Instantiate(star);
            starCount++;
        }
    }

    void HedgarJoinsTheParty() {
        GameObject joins_the_pt = GameObject.Find("hedgar_joins_the_party");
        Animator joinimator = joins_the_pt.GetComponent<Animator>();
        joinimator.enabled = true;
    }

    void HedgarJoinsThePartyDone() {
        GameObject joins_the_pt = GameObject.Find("hedgar_joins_the_party");
        Animator joinimator = joins_the_pt.GetComponent<Animator>();
        joinimator.StopPlayback();
        joinimator.Play("optionssubmenuitem_fadeout", -1, 0);
    }

    void turnOnLight() {
        ambient_light.intensity = 1f;
        main_camera.backgroundColor = new Color(255f, 255f, 255f);
        universemaster.ResetCameraMode();
    }

    private static IEnumerator FadeInSprite (SpriteRenderer renderer, float duration, float delay = 0F) {
        float start = Time.time;
        while (Time.time <= start + delay) {
            yield return null;
        }

        start = Time.time;
        while (Time.time <= start + duration) {
            Color color = renderer.color;
            color.a = 0f + Mathf.Clamp01((Time.time - start) / duration);
            renderer.color = color;
            yield return null;
        }
    }

    private static IEnumerator FadeOutSprite(SpriteRenderer renderer, float duration) {
        float start = Time.time;
        while (Time.time <= start + duration) {
            Color color = renderer.color;
            color.a = 1f - Mathf.Clamp01((Time.time - start) / duration);
            renderer.color = color;
            yield return null;
        }
    }

    public void return_to_map() {
        transitioning = true;
        StartCoroutine(universemaster.LoadOverworld(universemaster));
    }

    void OnDestroy() {
        player_1.ClearInputEventDelegates();
        player_2.ClearInputEventDelegates();
    }

}
