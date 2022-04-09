using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OptionsScreen : MonoBehaviour
{
    public int active_pause_menu_item = 0;
    public PauseMenuItem active_pause_menu_item_object;
    public PauseMenuItem[] items;
    public Animator animator;
    public SpriteRenderer whitesprite;
    public SpriteRenderer backgroundsprite;
    Gamemaster game_master;
    overworld_cattybatty overworld;
    DialogueManager dialog_manager;
    public Camera maincamera;
    public GameObject[] noise;
    public SpriteRenderer[] noiserenderers;
    public OptionsSubMenu[] submenus;
    public bool in_submenu = false;
    public OptionsSubMenu active_submenu_item_object;
    UniverseMaster universemaster;
    public MusicInstructor instructor;

    // Start is called before the first frame update
    void Start() {
        items = GetComponentsInChildren<PauseMenuItem>();
        submenus = GetComponentsInChildren<OptionsSubMenu>();
        SetOverworldOrGameMaster();
        maincamera = get_camera();
        transform.localScale = new Vector3(10f, 10f, 0);
        instructor = FindObjectOfType<MusicInstructor>();
        StartCoroutine(InitMe(this));
    }

    public void SetOverworldOrGameMaster() {
        if (game_master == null)
            game_master = FindObjectOfType<Gamemaster>();

        if (overworld == null)
            overworld = FindObjectOfType<overworld_cattybatty>();

        if (dialog_manager == null)
            dialog_manager = FindObjectOfType<DialogueManager>();
    }

    IEnumerator InitMe(OptionsScreen screen) {
        yield return new WaitForSeconds(0.1f);
        screen.ReadFromObserver();
    }

    public Camera get_camera() {
        if (maincamera == null) {
            MainCameraScript maincamerascript = GameObject.FindObjectOfType<MainCameraScript>();
            maincamera = maincamerascript.GetComponent<Camera>();
        }
        return maincamera;
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

    public void ReadFromObserver() {
        universemaster = get_universemaster();
        Observer observer = universemaster.observer;
        foreach (OptionsSubMenu submenu in submenus) {
            foreach (OptionsSubMenuItem item in submenu.items) {
                OptionSetting found = observer.FindOption(item.property_name);
                if (found != null) {
                    item.property_value = found.property_value;
                }
                item.universemaster = universemaster;
                item.Initialize();
            }
        }
    }

    public void SaveToObserver() {
        universemaster = get_universemaster();
        Observer observer = universemaster.observer;
        foreach (OptionsSubMenu submenu in submenus) {
            foreach (OptionsSubMenuItem item in submenu.items) {
                if (item.item_type != "button") {
                    observer.AddOption(new OptionSetting(item.property_name, item.property_value));
                }
            }
        }
        observer.UpdateOptionsArray();
        universemaster.save_progress();
    }

    public void set_active(int active) {
        active_pause_menu_item = active;
        highlight_active_menu_item();
    }

    public void highlight_active_menu_item() {
        if (in_submenu)
            return;
        SetOverworldOrGameMaster();
        foreach (PauseMenuItem item in items) {
            item.hover_off();
            if (item.transform.GetSiblingIndex() == active_pause_menu_item) {
                item.hover_on();
                StartCoroutine(noiseAround(game_master, overworld, this, item.start_pos, noise, noiserenderers));
                active_pause_menu_item_object = item;
            }
        }
        universemaster.instructor.soundman.play_sound("select");
    }

    private static IEnumerator noiseAround(Gamemaster game_master, overworld_cattybatty overworld, OptionsScreen self, Vector3 item_pos, GameObject[] noise, SpriteRenderer[] noiserenderers) {
        float x = item_pos.x + 1.5f;
        float y = item_pos.y - 0.4f;
        float y_max = y + 0.9f;

        foreach (GameObject nois in noise) {
            if (game_master != null) {
                if (!game_master.is_in_options) {
                    break;
                }
            }
            else {
                if (overworld != null) {
                    if (!overworld.is_in_options) {
                        break;
                    }
                }
                else {
                    if (!self.universemaster.is_in_options) {
                        break;
                    }
                }
            }
            x += 0.4f + Random.Range(0, 0.5f);
            nois.transform.position = new Vector3(
                x,
                Random.Range(y, y_max),
                nois.transform.position.z
            );
            noiserenderers[nois.transform.GetSiblingIndex()].enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void next_item() {
        if (in_submenu) {
            active_submenu_item_object.next_item();
            universemaster.instructor.soundman.play_sound("select");
            return;
        }

        active_pause_menu_item++;
        if (active_pause_menu_item >= items.Length) {
            active_pause_menu_item = 0;
        }

        highlight_active_menu_item();
    }

    public void prev_item() {
        if (in_submenu) {
            active_submenu_item_object.prev_item();
            universemaster.instructor.soundman.play_sound("select");
            return;
        }

        active_pause_menu_item--;
        if (active_pause_menu_item < 0) {
            active_pause_menu_item = items.Length - 1;
        }

        highlight_active_menu_item();
    }

    public void left_item() {
        if (in_submenu) {
            active_submenu_item_object.left_item();
            universemaster.instructor.soundman.play_sound("select", pitch_amount: -3);
        }
    }

    public void right_item() {
        if (in_submenu) {
            active_submenu_item_object.right_item();
            universemaster.instructor.soundman.play_sound("select", pitch_amount : 3);
        }
    }

    public void ShowOptions() {
        SetOverworldOrGameMaster();

        animator.enabled = true;
        animator.StopPlayback();
        animator.Play("optionresizer", -1, 0f);
        transform.position = new Vector3(0, 0, 0);
        if (game_master != null) {
            game_master.pausescreen.hide_pause_screen();
        }
        if (overworld != null) {
            overworld.pausescreen.hide_pause_screen();
        }
        if (game_master == null && overworld == null) {
            universemaster.pause.hide_pause_screen();
        }
        set_active(0);
        universemaster.instructor.soundman.play_sound("open");
        //StartCoroutine(AttachFrameToOptionsBackground(this));
    }

    IEnumerator AttachFrameToOptionsBackground(OptionsScreen self) {
        yield return new WaitForEndOfFrame();

        var texture = ScreenCapture.CaptureScreenshotAsTexture();
        Sprite newsprite = Sprite.Create(
            texture,
            new Rect(0.0f, 0.0f, texture.width, texture.height),
            new Vector2(0.5f, 0.5f),
            100.0f
        );
        self.backgroundsprite.sprite = newsprite;
        self.backgroundsprite.enabled = true;
    }
    public void HideOptions() {
        SetOverworldOrGameMaster();

        animator.enabled = true;
        animator.StopPlayback();
        animator.Play("optionresizer_end", -1, 0f);
        if (game_master != null) {
            game_master.pausescreen.init_pause_screen();
        }
        if (overworld != null) {
            overworld.pausescreen.init_pause_screen(true);
        }
        if (game_master == null && overworld == null) {
            universemaster.pause.init_pause_screen();
        }

        foreach (GameObject nois in noise) {
            nois.GetComponent<SpriteRenderer>().enabled = false;
        }
        universemaster.instructor.soundman.play_sound("return");
    }

    public void fireCancel() {
        SetOverworldOrGameMaster();

        if (in_submenu) {
            active_submenu_item_object.SaveAndReturn();
            return;
        }

        if (game_master != null) {
            game_master.closeOptions();
        }
        if (overworld != null) {
            overworld.closeOptions();
        }
        if (dialog_manager != null) {
            dialog_manager.closeOptions();
        }

    }

    public bool is_in_options() {
        if (game_master != null) {
            if (game_master.is_in_options) {
                return true;
            }
        }
        if (overworld != null) {
            if (overworld.is_in_options) {
                return true;
            }
        }
        if (dialog_manager != null) {
            if (universemaster.is_in_options) {
                return true;
            }
        }

        return false;
    }

    public void call_action_of_active_menu_item() {
        SetOverworldOrGameMaster();

        if (!is_in_options()) {
            return;
        }

        if (in_submenu) {
            active_submenu_item_object.call_action_of_active_submenu_item();
        }
        else {
            foreach (PauseMenuItem item in items) {
                if (item.transform.GetSiblingIndex() == active_pause_menu_item && item.hover) {
                    item.call_action();
                }
            }
        }
        universemaster.instructor.soundman.play_sound("select", pitch_amount: 3);
    }

    public void action_return() {
        SetOverworldOrGameMaster();

        if (game_master != null) {
            game_master.closeOptions();
        }
        if (overworld != null) {
            overworld.closeOptions();
        }
        if (dialog_manager != null) {
            dialog_manager.closeOptions();
        }
    }

    void remove_all_main_categories() {
        foreach (PauseMenuItem item in items) {
            item.move_left();
        }
    }

    void return_all_main_categories() {
        foreach (PauseMenuItem item in items) {
            item.move_back();
        }
    }

    public void action_general() {
        open_active_pause_menu_item();
    }

    public void action_audio() {
        open_active_pause_menu_item();
    }

    public void action_video() {
        open_active_pause_menu_item();
    }

    public void action_controls() {
        open_active_pause_menu_item();
    }

    void open_active_pause_menu_item() {
        SetOverworldOrGameMaster();

        remove_all_main_categories();
        active_pause_menu_item_object.move_top();

        OptionsSubMenu submenu = submenus[active_pause_menu_item];
        submenu.show();
        in_submenu = true;
        active_submenu_item_object = submenu;

        StartCoroutine(
            noiseAround(game_master, overworld, this,
                new Vector3(active_pause_menu_item_object.transform.position.x, 2f, active_pause_menu_item_object.transform.position.z),
                noise, noiserenderers
            )
        );
    }

    public void exitSubmenu() {
        in_submenu = false;
        active_pause_menu_item_object.move_top();
        return_all_main_categories();
    }

    /* UPDATE FUNCTIONS */

    public void ApplyCameraMode() {
        universemaster = get_universemaster();
        OptionSetting found = universemaster.observer.FindOption("mode");
        if (found == null)
            return;
        SetCameraMode(found.property_value + 1);
    }

    public void SetCameraMode(int cammode) {
        maincamera = get_camera();
        MainCameraScript camscript = maincamera.GetComponent<MainCameraScript>();
        Material mat = camscript.mat;
        mat.SetInt("_CameraMode", cammode);
    }

    public void SetFullScreen() {
        universemaster = get_universemaster();
        OptionSetting found = universemaster.observer.FindOption("fullscreen");
        if (found == null)
            return;
        Screen.fullScreen = found.property_value == 1 ? true : false;
    }

    public void SetEffectsBoolean() {
        if (universemaster == null) {
            return;
        }
        OptionSetting found = universemaster.observer.FindOption("effects");
        if (found == null)
            return;
        if (found.property_value == 0) {
            universemaster.instructor.soundman.MuteEffects();
        }
        else {
            universemaster.instructor.soundman.UnmuteEffects();
        }
    }
    public void SetEffectsVolume() {
        if (universemaster == null) {
            return;
        }

        OptionSetting found = universemaster.observer.FindOption("effects_volume");
        if (found == null)
            return;
        universemaster.instructor.soundman.SetVolume(found.property_value / 100f);
    }

    public void SetMusicBoolean() {
        if (universemaster == null) {
            return;
        }

        OptionSetting found = universemaster.observer.FindOption("music");
        if (found == null)
            return;
        if (found.property_value == 0) {
            universemaster.instructor.MuteMusic();
        }
        else {
            universemaster.instructor.UnmuteMusic();
        }
    }

    public void SetMusicVolume() {
        if (universemaster == null) {
            return;
        }
        OptionSetting found = universemaster.observer.FindOption("music_volume");
        if (found == null)
            return;
        universemaster.instructor.SetVolume(found.property_value / 100f);
    }

    public void SetLanguage() {
        if (universemaster == null) {
            return;
        }
        OptionSetting found = universemaster.observer.FindOption("language");
        if (found == null)
            return;

        string[] available_options = new string[] { "" };
        string langu = "";

        foreach (OptionsSubMenu submenu in submenus) {
            foreach (OptionsSubMenuItem item in submenu.items) {
                if (item.property_name == "language") {
                    available_options = item.sliderselect.available_options;
                    langu = available_options[found.property_value];
                }
            }
        }

        if (langu == "English") {
            I2.Loc.LocalizationManager.CurrentLanguageCode = "en";
        }
        if (langu == "Russian") {
            I2.Loc.LocalizationManager.CurrentLanguageCode = "ru";
        }
        if (langu == "German") {
            I2.Loc.LocalizationManager.CurrentLanguageCode = "de-DE";
        }
        if (langu == "Austrian") {
            I2.Loc.LocalizationManager.CurrentLanguageCode = "de-AT";
        }
        if (langu == "Finnish") {
            I2.Loc.LocalizationManager.CurrentLanguageCode = "fi";
        }
        if (langu == "Japanese") {
            I2.Loc.LocalizationManager.CurrentLanguageCode = "ja";
        }
        if (langu == "Portuguese (Brazil)") {
            I2.Loc.LocalizationManager.CurrentLanguageCode = "pt-BR";
        }
        if (langu == "Spanish") {
            I2.Loc.LocalizationManager.CurrentLanguageCode = "es";
        }

        SetOverworldOrGameMaster();

        if (dialog_manager != null) {
            dialog_manager.DisplayCurrentSentence();
        }
        if (overworld != null) {
            overworld.DisplayCurrentLocationInfo();
        }

        StartCoroutine(update_options_sub_slider_selects());
    }

    IEnumerator update_options_sub_slider_selects() {
        OptionsSubSliderSelect[] selects = FindObjectsOfType<OptionsSubSliderSelect>();

        foreach (OptionsSubSliderSelect select in selects) {
            select.textmeshpro.text = "";
        }

        yield return new WaitForSeconds(0.01f);

        foreach (OptionsSubSliderSelect select in selects) {
            select.display_current_option_text();
        }
    }

    public void SetResolution() {

        // checks resolution and applies it
        // only lets you change resolution when inside options screen
        // and if the current resolution is an available option
        // otherwise ignore

        SetOverworldOrGameMaster();
        universemaster = get_universemaster();
        OptionSetting found = universemaster.observer.FindOption("resolution");
        string resolution = "";
        string current_resolution = Screen.width + " x " + Screen.height;
        string[] available_options = new string[] { "" };

        foreach (OptionsSubMenu submenu in submenus) {
            foreach (OptionsSubMenuItem item in submenu.items) {
                if (item.property_name == "resolution") {
                    available_options = item.sliderselect.available_options;
                    resolution = available_options[found.property_value];
                }
            }
        }

        bool resolution_found = false;

        foreach (string resolution_option in available_options) {
            if (resolution_option == current_resolution) {
                resolution_found = true;
            }
        }

        if (!is_in_options() && !resolution_found) {
            return;
        }

        string[] resolutions = resolution.Split('x');

        FullScreenMode setMode =
            Screen.fullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;

        Screen.SetResolution(
            int.Parse(resolutions[0].Trim()),
            int.Parse(resolutions[1].Trim()),
            setMode
        );
    }
}
