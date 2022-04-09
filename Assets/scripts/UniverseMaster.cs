using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UniverseMaster : MonoBehaviour
{
    public GameObject music_instructor_prefab;
    public GameObject pause_prefab;
    public GameObject options_prefab;

    public Observer observer;
    public OptionsScreen options;
    public PauseScreen pause;
    public MusicInstructor instructor;
    StorageMan storageman;

    public bool is_singleton = false;
    public bool is_in_achievements = false;
    public bool is_in_options = false;
    public bool is_paused = false;
    public bool returning_to_overworld = false;

    private void Awake() {
        UniverseMaster[] masters = GameObject.FindObjectsOfType<UniverseMaster>();

        if (masters.Length > 1) {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        is_singleton = true;

        storageman = new StorageMan();

        observer = storageman.load_from_json();
        observer.total_levels = new Observer().total_levels;
        observer.initialize_levels();
        observer.initialize_achievements();
    }

    private void Start() {
        instructor = GetOrCreateMusicInstructor();
        pause = GetOrCreatePauseScreen();
        options = GetOrCreateOptionsScreen();
    }

    public bool check_level_first_revealed(int level_index) {
        SpiritGuideLevel level = get_level(level_index);

        if (level.level_number <= observer.highest_played_level_number + 1 && level.revealed == false) {
            level.revealed = true;
            return true;
        }

        return false;
    }

    public bool is_level_revealed(int level_index) {
        SpiritGuideLevel level = get_level(level_index);

        return level.revealed;
    }

    public void set_highest_played_level(int level_number) {
        if (observer.highest_played_level_number < level_number) {
            observer.highest_played_level_number = level_number;
        }
    }

    public void check_win_achievement(int level_number) {
        observer.SetAchievementAchieved("LEVEL_" + level_number.ToString());
    }

    public void checkForDarkMode() {
        OptionSetting found = observer.FindOption("mode");
        if (found == null) {
            return;
        }
        if (found.property_value == 3 || found.property_value == 4) {
            if (SceneManager.GetActiveScene().name.Contains("20") ||
                SceneManager.GetActiveScene().name.Contains("Cutscene21")) {
                options.SetCameraMode(2);
            }
            else {
                ResetCameraMode();
            }
        }
    }
    public void ResetCameraMode() {
        OptionSetting found = observer.FindOption("mode");
        if (found == null) {
            return;
        }
        options.SetCameraMode(found.property_value + 1);
    }

    public void SetHighScore(int level_index, int boxes_placed, float total_time) {
        SpiritGuideLevel level = observer.GetLevelByIndex(level_index);
        level.total_times_finished++;
        if(boxes_placed < level.best_finished_boxes_used || level.best_finished_boxes_used == 0) {
            level.best_finished_boxes_used = boxes_placed;
        }
        if (total_time < level.best_finished_time_seconds || level.best_finished_time_seconds == 0) {
            level.best_finished_time_seconds = total_time;
        }
        save_progress();
    }

    public bool HasLevelBeenFinishedOnce(int level_number) {
        SpiritGuideLevel level = get_level(level_number - 1);
        return level.total_times_finished > 0;
    }

public void reveal_levels() {
        foreach (SpiritGuideLevel level in observer.levels) {
            if (level.level_number <= observer.highest_played_level_number + 1) {
                level.revealed = true;
            }
            else {
                level.revealed = false;
            }
        }
    }

    public void set_last_played_level(int level_number) {
        observer.last_played_level_number = level_number;
    }

    public SpiritGuideLevel get_level(int level_index) {
        return observer.levels[level_index];
    }

    public void reset_level() {
        is_in_options = false;
        is_paused = false;
        StartCoroutine(ResetLevel());
    }

    private static IEnumerator ResetLevel() {

        int active_scene_index = SceneManager.GetActiveScene().buildIndex;

        yield return null;
        AsyncOperation ao = SceneManager.UnloadSceneAsync(active_scene_index);
        if (ao != null) {
            while (!ao.isDone) {
                yield return null;
            }
        }
        Resources.UnloadUnusedAssets();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Single);
        while (!asyncLoad.isDone) {
            yield return null;
        }

        asyncLoad = SceneManager.LoadSceneAsync(active_scene_index, LoadSceneMode.Single);
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }

    public IEnumerator LoadOverworld(UniverseMaster self) {
        if (returning_to_overworld) {
            yield break;
        }

        is_in_options = false;
        is_paused = false;
        returning_to_overworld = true;
        SceneTransitioner transitioner = GameObject.FindObjectOfType<SceneTransitioner>();
        if (transitioner) {
            transitioner.StartTransitionV2();
        }
        self.instructor.soundman.play_sound("transition_out2");
        self.instructor.PlayMusic("entercutscene");

        string new_map_name = "overworld";
        if (SceneManager.GetActiveScene().name.IndexOf("_hard") > -1) {
            new_map_name = "overworld_hard";
        }
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(new_map_name, LoadSceneMode.Single);
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

        if (transitioner) {
            transitioner.EndTransitionV2();
        }
        self.instructor.soundman.play_sound("transition_in2");
        asyncLoad.allowSceneActivation = true;
        returning_to_overworld = false;
    }

    public UniverseMaster findUniverseMaster() {
        UniverseMaster[] masters = FindObjectsOfType<UniverseMaster>();
        foreach (UniverseMaster master in masters) {
            if (master.is_singleton) {
                return master;
            }
        }

        return null;
    }

    public void save_progress() {
        storageman.save_as_json(observer);
    }

    public void reset_progress() {
        Debug.Log("resetting progress");
        storageman.reset_progress();
        observer = storageman.load_from_json();
        observer.initialize_levels();
        observer.initialize_achievements();
    }

    public void unlock_new_game_plus() {
        Debug.Log("unlocked new game+");
        set_highest_played_level(31);
        set_last_played_level(31);
        save_progress();
    }

    public void unlock_levels() {
        Debug.Log("unlocked levels");
        set_highest_played_level(observer.total_levels);
        set_last_played_level(observer.total_levels);
        save_progress();
    }

    public bool CanPlayNewGamePlus() {
        if (observer.highest_played_level_number >= 30) {
            return true;
        }
        return false;
    }

    public bool AllLevelsCleared() {
        int max_levels = 30;
        if (SceneManager.GetActiveScene().name.IndexOf("_hard") > -1) {
            max_levels = observer.total_levels - 1;
        }
        if (observer.highest_played_level_number >= max_levels) {
            return true;
        }
        return false;
    }

    public MusicInstructor GetOrCreateMusicInstructor() {
        MusicInstructor mi = FindObjectOfType<MusicInstructor>();
        if (mi == null) {
            GameObject newmi = (GameObject)Instantiate(music_instructor_prefab);
            mi = newmi.GetComponent<MusicInstructor>();
        }
        return mi;
    }

    public PauseScreen GetOrCreatePauseScreen() {
        PauseScreen mi = FindObjectOfType<PauseScreen>();
        if (mi == null) {
            GameObject newop = (GameObject)Instantiate(pause_prefab);
            mi = newop.GetComponent<PauseScreen>();
            newop.transform.parent = this.transform;
        }
        return mi;
    }

    public OptionsScreen GetOrCreateOptionsScreen() {
        OptionsScreen mi = FindObjectOfType<OptionsScreen>();
        if (mi == null) {
            GameObject newop = (GameObject)Instantiate(options_prefab);
            mi = newop.GetComponent<OptionsScreen>();
            newop.transform.parent = this.transform;
        }
        return mi;
    }

}
