using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class overworld_cattybatty : MonoBehaviour {

    UniverseMaster universemaster;
    public int playerId;
    public GameObject overworld_map;
    private Player player_1;
    private Player player_2;
    GameObject active_location;
    bool horizontal_down_player_1 = false;
    float horizontal_down_player_1_time;
    bool horizontal_down_player_2 = false;
    float horizontal_down_player_2_time;
    bool vertical_down_player_1 = false;
    float vertical_down_player_1_time;
    bool vertical_down_player_2 = false;
    float vertical_down_player_2_time;

    public OptionsScreen optionsscreen;
    public PauseScreen pausescreen;
    public bool is_in_options = false;
    public bool is_paused = false;
    MusicInstructor instructor;
    LocationHover locationhover;
    LocationInfo locationinfo;

    int active_x = 0;
    int active_y = 0;
    int last_x = 0;
    int last_y = 0;
    public bool is_loading = false;
    DialogueManager dialogmanager;

    private void Awake() {
        player_1 = ReInput.players.GetPlayer(0);
        player_1.AddInputEventDelegate(OnMoveHorizontalPlayer1, UpdateLoopType.Update, InputActionEventType.AxisActive, "Move Horizontal");
        player_1.AddInputEventDelegate(OnMoveVerticalPlayer1, UpdateLoopType.Update, InputActionEventType.AxisActive, "Move Vertical");
        player_1.AddInputEventDelegate(OnUpUpdate, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Up");
        player_1.AddInputEventDelegate(OnDownUpdate, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Down");
        player_1.AddInputEventDelegate(OnLeftUpdate, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Left");
        player_1.AddInputEventDelegate(OnRightUpdate, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Right");
        player_1.AddInputEventDelegate(OnStartUpdate, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Start");
        player_1.AddInputEventDelegate(OnCancelUpdateOnce, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Cancel");
        player_1.AddInputEventDelegate(OnR1Update, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "R1");

        player_2 = ReInput.players.GetPlayer(1);
        player_2.AddInputEventDelegate(OnMoveHorizontalPlayer2, UpdateLoopType.Update, InputActionEventType.AxisActive, "Move Horizontal");
        player_2.AddInputEventDelegate(OnMoveVerticalPlayer2, UpdateLoopType.Update, InputActionEventType.AxisActive, "Move Vertical");
        player_2.AddInputEventDelegate(OnUpUpdate, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Up");
        player_2.AddInputEventDelegate(OnDownUpdate, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Down");
        player_2.AddInputEventDelegate(OnLeftUpdate, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Left");
        player_2.AddInputEventDelegate(OnRightUpdate, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Right");
        player_2.AddInputEventDelegate(OnStartUpdate, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Start");
        player_2.AddInputEventDelegate(OnCancelUpdateOnce, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Cancel");
        player_2.AddInputEventDelegate(OnR1Update, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "R1");
    }

    private void Start() {
        setUniverseMaster();

        optionsscreen = universemaster.options;
        pausescreen = universemaster.pause;

        dialogmanager = FindObjectOfType<DialogueManager>();
        locationhover = FindObjectOfType<LocationHover>();
        locationinfo = FindObjectOfType<LocationInfo>();

        instructor = FindObjectOfType<MusicInstructor>();
        if (instructor != null) {
            if (SceneManager.GetActiveScene().name.IndexOf("_hard") > -1) {
                instructor.PlayMusic("overworld_hard");
            }
            else {
                instructor.PlayMusic("overworld");
            }
        }

        if (SceneManager.GetActiveScene().name.IndexOf("_hard") > -1) {
            active_x = 0;
            active_y = 2;
        }

        setActiveLocationForLastPlayedLevel();
        fly_to_location(active_location);
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

    void setActiveLocationForLastPlayedLevel() {
        active_location = find_location_for_level_number(universemaster.observer.last_played_level_number);

        if (active_location == null) {
            // go through all levels, get location with "revealed" and highest level_number

            // else, just pick first of current overworld
            if (SceneManager.GetActiveScene().name.IndexOf("_hard") > -1) {
                active_location = find_location_for_level_number(32);
            }
            else {
                active_location = find_location_for_level_number(1);
            }
        }

        overworld_location location_script = active_location.GetComponent<overworld_location>();
        active_x = location_script.coordinate_x;
        active_y = location_script.coordinate_y;
    }

    void OnMoveHorizontalPlayer1(InputActionEventData data) {
        if (data.GetAxis() > -0.5f && data.GetAxis() < 0.5f)
            return;

        if (horizontal_down_player_1 == true) {
            return;
        }
        horizontal_down_player_1 = true;
        horizontal_down_player_1_time = Time.time;

        horizontal_move(data.GetAxis());
    }

    void OnMoveHorizontalPlayer2(InputActionEventData data) {
        if (data.GetAxis() > -0.5f && data.GetAxis() < 0.5f)
            return;

        if (horizontal_down_player_2 == true) {
            return;
        }
        horizontal_down_player_2 = true;
        horizontal_down_player_2_time = Time.time;

        horizontal_move(data.GetAxis());
    }

    void OnMoveVerticalPlayer1(InputActionEventData data) {
        if (data.GetAxis() > -0.5f && data.GetAxis() < 0.5f)
            return;

        if (vertical_down_player_1 == true) {
            return;
        }
        vertical_down_player_1 = true;
        vertical_down_player_1_time = Time.time;

        vertical_move(data.GetAxis());
    }

    void OnMoveVerticalPlayer2(InputActionEventData data) {
        if (data.GetAxis() > -0.5f && data.GetAxis() < 0.5f)
            return;

        if (vertical_down_player_2 == true) {
            return;
        }
        vertical_down_player_2 = true;
        vertical_down_player_2_time = Time.time;

        vertical_move(data.GetAxis());
    }

    void OnR1Update(InputActionEventData data) {
        if (Time.timeSinceLevelLoad < 2f) {
            return;
        }

        if (!universemaster.CanPlayNewGamePlus()) {
            return;
        }

        if (!is_loading) {
            is_loading = true;
            instructor.soundman.play_sound("click1");
            SwitchOverworld();
        }
    }

    void SwitchOverworld() {
        if (SceneManager.GetActiveScene().name.IndexOf("hard") > -1) {
            instructor.PlayMusic("entercutscene");
            StartCoroutine(LoadCutscene("overworld"));
        }
        else {
            instructor.PlayMusic("exitlevel_hard");
            StartCoroutine(LoadCutscene("overworld_hard"));
        }
    }

    void OnCancelUpdateOnce(InputActionEventData data) {
        if (universemaster.is_in_achievements) {
            pausescreen.hide_achievements();
            return;
        }

        if (is_in_options) {
            optionsscreen.fireCancel();
            return;
        }

        if (is_paused) {
            unpauseGame();
            return;
        }
    }

    void horizontal_move(float axis) {
        if (Time.timeSinceLevelLoad < 1f) {
            return;
        }

        if (universemaster.is_in_achievements) {
            return;
        }

        if (is_in_options) {
            if (axis < 0) {
                optionsscreen.left_item();
            }
            else {
                optionsscreen.right_item();
            }
            return;
        }

        if (axis > 0) {
            active_x++;

            if (active_x > 11)
                active_x = 11;
        }
        else {
            active_x--;
            if (active_x < 0)
                active_x = 0;
        }

        fly_to_active_location();
    }

    void vertical_move(float axis) {
        if (Time.timeSinceLevelLoad < 1f) {
            return;
        }

        if (is_paused) {
            if (axis > -0.4f && axis < 0.4f)
                return;

            if (universemaster.is_in_achievements) {
                return;
            }

            if (is_in_options) {
                if (axis < 0) {
                    optionsscreen.next_item();
                }
                else {
                    optionsscreen.prev_item();
                }
            }
            else {
                if (axis < 0) {
                    pausescreen.next_item();
                }
                else {
                    pausescreen.prev_item();
                }
            }

            return;
        }

        if (axis < 0) {
            active_y++;

            if (active_y > 3)
                active_y = 0;
        }
        else {
            active_y--;
            if (active_y < 0)
                active_y = 3;
        }

        fly_to_active_location();
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

    public void fly_to_coordinates(int x, int y) {
        int old_x = active_x;
        int old_y = active_y;

        active_x = x;
        active_y = y;

        GameObject next_location = find_revealed_location_for_x_y(active_x, active_y);

        if (next_location != null && next_location != active_location) {
            fly_to_active_location();
        }
        else {
            active_x = old_x;
            active_y = old_y;
        }
    }

    void fly_to_active_location() {
        GameObject next_location = find_revealed_location_for_x_y(active_x, active_y);

        if (next_location == null) {
            // try to see if there's one with same y but +/- 1 x
            active_x -= 1;
            next_location = find_revealed_location_for_x_y(active_x, active_y);

            if (next_location == null) {
                active_x += 2;
                next_location = find_revealed_location_for_x_y(active_x, active_y);
            }

            if (next_location == null) {
                active_x = last_x;
                active_y = last_y;
                next_location = find_revealed_location_for_x_y(active_x, active_y);
            }
        }

        fly_to_location(next_location);
    }

    void fly_to_location(GameObject location) {
        if (location == null) {
            return;
        }
        if (is_paused) {
            return;
        }

        gameObject.transform.position = new Vector3(
            location.transform.position.x + 0.2f, location.transform.position.y + 2f
        );

        active_location = location;

        if (Time.timeSinceLevelLoad > 1f) {
            if (last_x != active_x || last_y != active_y) {
                instructor.soundman.play_sound("slide4", Random.Range(-2, 2));
            }
            else {
                instructor.soundman.play_sound("slide3", Random.Range(-2, 2));
            }
        }

        locationhover.transform.position = location.transform.position;
        setLocationInfo(location);

        last_x = active_x;
        last_y = active_y;
    }

    public void setLocationInfo(GameObject location) {
        if (location == null) {
            return;
        }
        locationinfo.transform.position = location.transform.position;
        overworld_location location_script = location.GetComponent<overworld_location>();
        SpiritGuideLevel level = universemaster.observer.GetLevelByIndex(location_script.level_number - 1);
        int level_number_for_name = location_script.level_number;
        if (level_number_for_name > 31) {
            level_number_for_name -= 31;
        }
        string level_name = I2.Loc.LocalizationManager.GetTranslation("levelname" + level_number_for_name);
        if (level.total_times_finished > 0) {
            locationinfo.SetInfo(level.best_finished_boxes_used.ToString(), level.best_finished_time_seconds, level_name, location_script.level_number);
        }
        else {
            locationinfo.SetInfo("--", 0, level_name, location_script.level_number);
        }
    }

    public void DisplayCurrentLocationInfo() {
        setLocationInfo(active_location);
    }

    GameObject find_revealed_location_for_x_y(int x, int y) {
        GameObject found_location = find_location_for_x_y(x, y);

        if (found_location == null) {
            return null;
        }

        overworld_location location_script = found_location.GetComponent<overworld_location>();
        if (location_script.revealed) {
            return found_location;
        }

        return null;
    }

    GameObject find_location_for_x_y(int x, int y) {
        GameObject found_location = null;
        foreach (Transform location in overworld_map.transform) {
            overworld_location location_script = location.GetComponent<overworld_location>();
            if (location_script.coordinate_x == x && location_script.coordinate_y == y) {
                found_location = location.gameObject;
            }
        }

        return found_location;
    }

    GameObject find_location_for_level_number(int level_number) {
        GameObject found_location = null;
        foreach (Transform location in overworld_map.transform) {
            overworld_location location_script = location.GetComponent<overworld_location>();
            if (location_script.level_number == level_number) {
                found_location = location.gameObject;
            }
        }

        return found_location;
    }

    void Update() {
        if (is_loading) {
            return;
        }

        if (instructor == null)
            instructor = FindObjectOfType<MusicInstructor>();

        // ignore input for the first 2s
        if (Time.timeSinceLevelLoad < 1f) {
            return;
        }

        if (Input.GetKey(KeyCode.N) && Input.GetKey(KeyCode.RightArrow)) {
            universemaster.unlock_new_game_plus();
            if (!is_loading) {
                is_loading = true;
                instructor.soundman.play_sound("click1");
                SwitchOverworld();
            }
        }

        if (Input.GetKey(KeyCode.O) && Input.GetKey(KeyCode.RightArrow)) {
            universemaster.reset_progress();
        }

        if (Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.RightArrow)) {
            universemaster.unlock_levels();
        }

        if (player_1.GetAxisTimeActive("Move Horizontal") == 0 && Time.time - horizontal_down_player_1_time > 0.1f) {
            horizontal_down_player_1 = false;
        }

        if (player_2.GetAxisTimeActive("Move Horizontal") == 0 && Time.time - horizontal_down_player_2_time > 0.1f) {
            horizontal_down_player_2 = false;
        }

        if (player_1.GetAxisTimeActive("Move Vertical") == 0 && Time.time - vertical_down_player_1_time > 0.1f) {
            vertical_down_player_1 = false;
        }

        if (player_2.GetAxisTimeActive("Move Vertical") == 0 && Time.time - vertical_down_player_2_time > 0.1f) {
            vertical_down_player_2 = false;
        }

        if (player_1.GetButtonDown("Fire") || player_2.GetButtonDown("Fire")) {
            if (is_paused) {
                if (universemaster.is_in_achievements) {
                    return;
                }

                if (is_in_options) {
                    optionsscreen.call_action_of_active_menu_item();
                }
                else {
                    pausescreen.call_action_of_active_pause_menu_item();
                }
                return;
            }

            if (!is_loading) {
                is_loading = true;
                instructor.soundman.play_sound("click1");

                if (is_mouse_over_new_game_plus()) {
                    SwitchOverworld();
                }
                else {
                    overworld_location location_script = active_location.GetComponent<overworld_location>();
                    instructor.PlayMusic("exitlevel");
                    StartCoroutine(LoadCutscene(location_script.spirit_guide_scene));
                }
            }
        }
    }

    bool is_mouse_over_new_game_plus() {
        newgameplussign sign = FindObjectOfType<newgameplussign>();
        if (sign == null) {
            return false;
        }
        if (Input.GetMouseButtonDown(0) && sign.hover) {
            return true;
        }
        return false;
    }

    IEnumerator LoadCutscene(string newscene) {
        SceneTransitioner transitioner = GameObject.FindObjectOfType<SceneTransitioner>();
        if (newscene.IndexOf("overworld") > -1) {
            transitioner.StartTransitionV3();
        }
        else {
            transitioner.StartTransition();
        }
        instructor.soundman.play_sound("transition_in");

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(newscene);
        asyncLoad.allowSceneActivation = false;

        float starttime = Time.time;

        while (!asyncLoad.isDone) {
            if (asyncLoad.progress >= 0.9f) {
                break;
            }

            yield return null;
        }

        while (Time.time - starttime < 2f) {
            yield return null;
        }

        if (newscene.IndexOf("overworld") > -1) {
            transitioner.EndTransitionV3();
        }
        else {
            transitioner.EndTransition();
        }
        instructor.soundman.play_sound("transition_out");
        asyncLoad.allowSceneActivation = true;
    }

    void OnStartUpdate(InputActionEventData data) {
        if (is_loading) {
            return;
        }

        if (universemaster.is_in_achievements) {
            pausescreen.hide_achievements();
            return;
        }

        if (is_in_options) {
            optionsscreen.fireCancel();
            return;
        }

        togglePause();
    }

    public void togglePause() {
        if (is_paused) {
            unpauseGame();
        }
        else {
            pauseGame();
        }
    }

    public void pauseGame() {
        if (dialogmanager != null && dialogmanager.transitioning) {
            return;
        }
        pausescreen.init_pause_screen(true);
        is_paused = true;
    }

    public void unpauseGame() {
        pausescreen.hide_pause_screen();
        is_paused = false;
    }

    public void toggleOptions() {
        if (is_in_options) {
            closeOptions();
        }
        else {
            openOptions();
        }
    }

    public void openOptions() {
        optionsscreen.ShowOptions();
        is_in_options = true;
    }

    public void closeOptions() {
        optionsscreen.HideOptions();
        is_in_options = false;
    }

    public void return_to_title() {
        is_loading = true;

        instructor.PlayMusic("main_theme");

        StartCoroutine(LoadCutscene("Splash"));
    }

    void OnDestroy() {
        // Unsubscribe from events when object is destroyed
        player_1.ClearInputEventDelegates();
        player_2.ClearInputEventDelegates();
    }
}
