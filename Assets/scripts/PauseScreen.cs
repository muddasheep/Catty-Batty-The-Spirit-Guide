using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public PauseMenuItem[] items;
    public GameObject spikes;
    public Animator batty;
    public Animator catty;
    public Animator white;
    Gamemaster game_master;
    overworld_cattybatty overworld;
    DialogueManager dialog_manager;
    UniverseMaster universemaster;

    public int active_pause_menu_item = 0;
    public Vector3 look_at_target_pos;

    AchievementsScreen acheevos;

    MusicInstructor instructor;

    // Start is called before the first frame update
    void Start()
    {
        items = GetComponentsInChildren<PauseMenuItem>();
        instructor = FindObjectOfType<MusicInstructor>();
        universemaster = get_universemaster();
    }

    public UniverseMaster get_universemaster() {
        if (universemaster == null) {
            UniverseMaster[] allmasters = FindObjectsOfType<UniverseMaster>();
            foreach (UniverseMaster areyoumaster in allmasters) {
                if (areyoumaster.is_singleton) {
                    universemaster = areyoumaster;
                    return universemaster;
                }
            }
        }

        return universemaster;
    }

    public void set_active(int active) {
        active_pause_menu_item = active;
        highlight_active_pause_menu_item();
    }

    public void init_pause_screen(bool for_overworld = false) {

        float delay = 0;
        active_pause_menu_item = 0;
        highlight_active_pause_menu_item();

        foreach (PauseMenuItem item in items) {
            StartCoroutine(ActivatePauseMenuItem(item, delay));
            delay += 0.02f;
        }

        delay = 0;
        float rotation = 70f;
        float rotationstep = 250f / spikes.transform.childCount;
        foreach (Transform spike in spikes.transform) {
            spike.eulerAngles = new Vector3(0, 0, rotation);
            rotation += rotationstep;
            StartCoroutine(ActivatePauseMenuSpike(spike, delay));
            delay += 0.05f;
        }

        batty.StopPlayback();
        batty.Play("pausemenu_batty");
        catty.StopPlayback();
        catty.Play("pausemenu_catty");

        white.enabled = true;
        white.StopPlayback();
        white.Play("pausemenu_white_in");

        instructor.soundman.play_sound("opencloseoptions1");

        if (for_overworld) {
            string achi = I2.Loc.LocalizationManager.GetTranslation("Achievements");
            string retu = I2.Loc.LocalizationManager.GetTranslation("Return to Title");

            // set achievements and return to title
            items[1].item_display_text = achi;
            items[1].item_action_name = "achievements";
            items[1].UpdateText();
            items[3].item_display_text = retu;
            items[3].item_action_name = "return_to_title";
            items[3].UpdateText();
        }
        else {
            string resta = I2.Loc.LocalizationManager.GetTranslation("pause_restart");
            string retu = I2.Loc.LocalizationManager.GetTranslation("pause_return");

            // set restart and return to map
            items[1].item_display_text = resta;
            items[1].item_action_name = "restart";
            items[1].UpdateText();
            items[3].item_display_text = retu;
            items[3].item_action_name = "return_to_map";
            items[3].UpdateText();
        }
    }

    public void highlight_active_pause_menu_item() {
        if (universemaster.is_in_achievements) {
            return;
        }

        foreach (PauseMenuItem item in items) {
            item.hover_off();
            if (item.transform.GetSiblingIndex() == active_pause_menu_item) {
                item.hover_on();
                look_at_target_pos = item.GetComponentInChildren<SpriteRenderer>().transform.position;
            }
        }
        instructor.soundman.play_sound("select");
    }

    public void next_item() {
        active_pause_menu_item++;
        if (active_pause_menu_item >= items.Length) {
            active_pause_menu_item = 0;
        }

        highlight_active_pause_menu_item();
    }

    public void prev_item() {
        active_pause_menu_item--;
        if (active_pause_menu_item < 0) {
            active_pause_menu_item = items.Length - 1;
        }

        highlight_active_pause_menu_item();
    }

    public void SetOverworldOrGameMaster() {
        if (game_master == null)
            game_master = FindObjectOfType<Gamemaster>();
 
        if (overworld == null)
            overworld = FindObjectOfType<overworld_cattybatty>();

        if (dialog_manager == null)
            dialog_manager = FindObjectOfType<DialogueManager>();
    }

    public void call_action_of_active_pause_menu_item() {

        SetOverworldOrGameMaster();

        if (game_master != null) {
            if (!game_master.is_paused || game_master.is_in_options) {
                return;
            }
        }

        if (overworld != null) {
            if (!overworld.is_paused || overworld.is_in_options || universemaster.is_in_achievements) {
                return;
            }
        }

        if (dialog_manager != null) {
            if (!universemaster.is_paused || universemaster.is_in_options) {
                return;
            }
        }

        foreach (PauseMenuItem item in items) {
            if (item.transform.GetSiblingIndex() == active_pause_menu_item && item.hover) {
                item.call_action();
            }
        }
    }

    public void hide_pause_screen() {
        float delay = 0;
        foreach (PauseMenuItem item in items) {
            StartCoroutine(DeactivatePauseMenuItem(item, delay));
            delay += 0.02f;
        }

        delay = 0;
        foreach (Transform spike in spikes.transform) {
            StartCoroutine(DeactivatePauseMenuSpike(spike, delay));
            delay += 0.05f;
        }

        batty.StopPlayback();
        batty.Play("pausemenu_batty_away");
        catty.StopPlayback();
        catty.Play("pausemenu_catty_away");

        white.StopPlayback();
        white.Play("pausemenu_white_out");
        instructor.soundman.play_sound("opencloseoptions2");
    }

    private static IEnumerator ActivatePauseMenuItem(PauseMenuItem item, float delay) {
        float start = Time.time;
        while (Time.time <= start + delay) {
            yield return null;
        }
        Animator item_rotater = item.GetComponentInChildren<Animator>();
        item_rotater.enabled = true;
        item_rotater.StopPlayback();
        item_rotater.Play("pausemenu_item_movein");
    }

    private static IEnumerator DeactivatePauseMenuItem(PauseMenuItem item, float delay) {
        float start = Time.time;
        while (Time.time <= start + delay) {
            yield return null;
        }
        Animator item_rotater = item.GetComponentInChildren<Animator>();
        item_rotater.StopPlayback();
        item_rotater.Play("pausemenu_item_moveout");
    }

    private static IEnumerator ActivatePauseMenuSpike(Transform item, float delay) {
        float start = Time.time;
        while (Time.time <= start + delay) {
            yield return null;
        }
        Animator item_rotater = item.GetComponentInChildren<Animator>();
        item_rotater.enabled = true;
        item_rotater.StopPlayback();
        item_rotater.Play("pausemenu_spike_animation");
    }

    private static IEnumerator DeactivatePauseMenuSpike(Transform item, float delay) {
        float start = Time.time;
        while (Time.time <= start + delay) {
            yield return null;
        }
        Animator item_rotater = item.GetComponentInChildren<Animator>();
        item_rotater.StopPlayback();
        item_rotater.Play("pausemenu_spike_animation_away");
    }

    public void action_continue() {
        SetOverworldOrGameMaster();

        if (game_master != null) {
            game_master.unpauseGame();
            return;
        }

        if (overworld != null) {
            overworld.unpauseGame();
            return;
        }

        if (dialog_manager != null) {
            dialog_manager.unpauseGame();
            return;
        }
    }

    public void action_restart() {
        SetOverworldOrGameMaster();

        if (game_master) {
            game_master.reset_level();
        }
        else {
            DialogueManager dialogmanager = FindObjectOfType<DialogueManager>();
            if (dialogmanager) {
                dialogmanager.reset_level();
            }
        }
        hide_pause_screen();
    }

    public void action_achievements() {
        acheevos = FindObjectOfType<AchievementsScreen>();
        if (acheevos != null) {
            acheevos.ShowAchievements();
            universemaster.is_in_achievements = true;
        }
    }

    public void hide_achievements() {
        acheevos = FindObjectOfType<AchievementsScreen>();
        if (acheevos != null) {
            acheevos.HideAchievements();
            universemaster.is_in_achievements = false;
        }
    }

    public void action_options() {
        SetOverworldOrGameMaster();

        if (game_master != null) {
            game_master.openOptions();
            return;
        }

        if (overworld != null) {
            overworld.openOptions();
            return;
        }

        if (dialog_manager != null) {
            dialog_manager.openOptions();
            return;
        }
    }

    public void action_return_to_map() {
        SetOverworldOrGameMaster();

        if (game_master != null) {
            game_master.update_played_levels(false);
            game_master.return_to_map();
        }
        if (dialog_manager != null)
            dialog_manager.return_to_map();

        hide_pause_screen();
    }

    public void action_return_to_title() {
        SetOverworldOrGameMaster();

        overworld.return_to_title();
        hide_pause_screen();
    }

    public void action_quit() {
        Application.Quit();
    }
}
