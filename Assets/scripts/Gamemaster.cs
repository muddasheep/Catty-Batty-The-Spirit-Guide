using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Gamemaster : MonoBehaviour {
    private UniverseMaster universemaster;
    public GameObject spiritmaster;
    public List<spirit_ai> spirits;
    private GameObject[] spiritspawnpoints;
    private GameObject grid_master;
    public GridManager grid_manager;
    public int level_number;
    public GameObject spirit_letter;
    public GameObject spirit;
    public GameObject star;
    public GateLetter[] gate_letters;
    GameObject[] spirit_buttons;
    GameObject[] super_chargers;
    GameObject[] heart_beans;
    GameObject[] leaves;
    public SpiritGate[] gates;
    ShieldSymbol[] shield_symbols;
    public PauseScreen pausescreen;
    public OptionsScreen optionsscreen;
    public bool is_paused = false;
    public bool burgundy_active = false;
    public bool is_in_options = false;
    public bool returning_to_map = false;
    public GameObject box_wood1;
    public GameObject box_wood2;
    public GameObject box_wood3;
    public GameObject box_wood4;

    public bool credits = false;
    public TextMeshPro towerdisplay;
    public int box_limit;
    public int spirit_limit;
    public int total_preview_spirits = 0;
    public bool spirits_active = false;
    public bool game_is_running = true;
    public float game_finished_since = 0;
    public bool night = false;
    public bool raining;
    public bool followPlayersByXAxis = false;
    public bool windy;
    public bool spawn_crystomalities = false;
    public bool has_beatbox = false;
    public bool gates_evaporate = false;
    public bool has_waiting_room_display = false;
    public bool wandering_spirit_strangers = false;
    public bool has_hedgar_waiting_for_glasses = false;
    public bool has_hidden_gate = false;
    public SpiritWaitingRoomDisplay waitingroomdisplay;
    public GameObject windswirley;
    public GameObject spiritlight;
    public GameObject crystomality;
    public Material sprite_with_lights_material;
    float preview_spirit_sent = 0;
    float time_last_crystomality_spawn = 0;
    float time_last_player_switch = 0;
    public int total_crystomalities = 0;

    GameObject catty;
    GameObject batty;
    player_movement cattyscript;
    player_movement battyscript;
    public List<Poof> poofpool;
    public List<GridLine> gridlinepool;
    public List<spirit_ai> stranger_spirits;
    List<SpiritGridRow> spirits_grid = new List<SpiritGridRow>();

    public float level_start_time;
    int current_shield_symbol_count = 0;

    public MusicInstructor music_instructor;

    public Material standard_shader;

    // Start is called before the first frame update
    void Start()
    {
        if (night) {
            RenderSettings.ambientLight = Color.black;
        }

        level_start_time = Time.time;
        poofpool = new List<Poof>();
        gridlinepool = new List<GridLine>();

        setUniverseMaster();

        pausescreen = FindObjectOfType<PauseScreen>();
        optionsscreen = FindObjectOfType<OptionsScreen>();

        init_gates();
        init_grid();
        init_players();
        init_spirits();
        init_music();
        
        show_required_letters_above_gate();
        check_for_spirit_buttons();

        if (wandering_spirit_strangers) {
            init_wandering_spirit_strangers();
        }

        if (has_waiting_room_display) {
            waitingroomdisplay = FindObjectOfType<SpiritWaitingRoomDisplay>();
        }

        if (has_beatbox) {
            activate_spirits_movement_for_beatbox();
        }

        if (windy) {
            startWind();
        }

        if (raining) {
            startRain();
            setSpiritsToFollow();
            BegoneSigns();
        }

        if (box_limit <= 0) {
            box_limit = 9999;
        }

        if (spirit_limit <= 0) {
            spirit_limit = 8;
        }

        init_tower_placement_markers();
        time_last_crystomality_spawn = Time.time;

        SpriteRenderer catty_sr = catty.GetComponentInChildren<SpriteRenderer>();
        sprite_with_lights_material = catty_sr.material;

        if (FindObjectOfType<BurgundyHelp>() != null && !universemaster.HasLevelBeenFinishedOnce(level_number)) {
            DisplayBurgundyHelp();
        }

        if (is_player_switched()) {
            switchPlayerIds(shoothearts: false);
        }

        TransitionGradient tr = FindObjectOfType<TransitionGradient>();
        if (tr != null && SceneManager.GetActiveScene().name.IndexOf("_hard") == -1) {
            tr.FadeOut();
        }

        if (night) {
            StartCoroutine(SetNightModeShader());
        }
    }

    IEnumerator SetNightModeShader() {
        // set standard shader for objects during night
        catty.GetComponentInChildren<SpriteRenderer>().material = standard_shader;
        batty.GetComponentInChildren<SpriteRenderer>().material = standard_shader;

        foreach (bulbtree tree in FindObjectsOfType<bulbtree>()) {
            tree.GetComponentInChildren<SpriteRenderer>().material = standard_shader;
        }

        foreach (SpiritGate gate in gates) {
            gate.GetComponentInChildren<SpriteRenderer>().material = standard_shader;
        }

        foreach (SpiritButton button in FindObjectsOfType<SpiritButton>()) {
            foreach (SpriteRenderer sprite in button.GetComponentsInChildren<SpriteRenderer>()) {
                sprite.material = standard_shader;
            }
        }

        foreach (GameObject spawn in spiritspawnpoints) {
            spawn.GetComponentInChildren<SpriteRenderer>().material = standard_shader;
        }

        foreach (cursorScript cursor in FindObjectsOfType<cursorScript>()) {
            foreach (SpriteRenderer sprite in cursor.GetComponentsInChildren<SpriteRenderer>()) {
                sprite.material = standard_shader;
            }
        }

        foreach (Gaterfly gatefly in FindObjectsOfType<Gaterfly>()) {
            gatefly.GetComponentInChildren<SpriteRenderer>().material = standard_shader;
        }

        foreach (SpriteRenderer sprite in FindObjectOfType<LevelLayout>().GetComponentsInChildren<SpriteRenderer>()) {
            if (sprite.gameObject.name.IndexOf("gameplay_background") > -1
                || sprite.gameObject.transform.parent.name.IndexOf("levelborder") > -1) {
                sprite.material = standard_shader;
            }
        }

        // wait until grass has been built
        yield return new WaitForSeconds(0.1f);

        foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject))) {
            if (obj.transform.parent == null) {
                // separate if for performance reasons
                if (obj.name.IndexOf("Grass") > -1) {
                    obj.GetComponentInChildren<SpriteRenderer>().material = standard_shader;
                }
            }
        }
    }

    public void DestroyBoxWithEffects(GameObject box) {
        int x = grid_manager.round(box.transform.position.x);
        int y = grid_manager.round(box.transform.position.y);

        if (grid_manager.delete_tower(x, y)) {
            spawnWood(box.transform.position);
            music_instructor.soundman.play_sound("rip" + Random.Range(5, 9));
        }
        else {
            Debug.Log("cant destroy at " + box.transform.position.x + " " +  box.transform.position.y);
        }
    }

    void spawnWood(Vector3 target_position) {
        float leave_count = 0;

        GameObject base_wood = new GameObject();

        while (leave_count < 5f) {
            Vector3 rotation = new Vector3(0, 0, Random.Range(0, 360f));
            GameObject wood = box_wood1;
            if (leave_count > 1f) {
                wood = box_wood2;
            }
            if (leave_count > 2f) {
                wood = box_wood3;
            }
            if (leave_count > 3f) {
                wood = box_wood4;
            }

            GameObject parent_wood = Instantiate(base_wood, target_position, Quaternion.Euler(rotation));

            GameObject new_leave = (GameObject)Instantiate(
                wood, target_position, Quaternion.Euler(rotation), parent_wood.transform
            );
            leave_count += 1f;
            Destroy(parent_wood, 0.5f);
        }
    }

    void DisplayBurgundyHelp() {
        BurgundyHelp burgundy = FindObjectOfType<BurgundyHelp>();
        burgundy.DisplayHelp(this);
        is_paused = true;
        burgundy_active = true;
    }

    public void HideBurgundyHelp() {
        BurgundyHelp burgundy = FindObjectOfType<BurgundyHelp>();
        burgundy.GoodbyeBurgundy();
        is_paused = false;
        burgundy_active = false;
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

    private void FixedUpdate() {
        summon_crystomalities();
    }

    void init_music() {
        music_instructor = universemaster.GetOrCreateMusicInstructor();
        int level_number_for_music = level_number;
        float pitch = 1f;
        if (level_number > 31) {
            level_number_for_music -= 31;
            pitch = 0.9f;
        }
        if (level_number > 31 && credits) {
            level_number_for_music = 62;
            pitch = 1f;
        }
        music_instructor.PlayMusic("gameplay_variant" + level_number_for_music.ToString(), pitch);
    }

    public void send_preview_spirit() {
        if (Time.time - preview_spirit_sent < 1f) {
            return;
        }

        deletePreviewSpirits();

        foreach (GameObject spawnpoint in spiritspawnpoints) {
            GameObject preview_spirit = (GameObject)Instantiate(spirit);
            preview_spirit.transform.position = new Vector3(
                spawnpoint.transform.position.x,
                spawnpoint.transform.position.y,
                preview_spirit.transform.position.z
            );
            spirit_ai preview_spirit_ai = preview_spirit.GetComponent<spirit_ai>();
            preview_spirit_ai.activatePreviewMode();
            preview_spirit_ai.Activate();
            preview_spirit_ai.base_speed = 10f;
            preview_spirit_sent = Time.time;
            preview_spirit.AddComponent<SpiritPreview>();
            SpriteRenderer sr = preview_spirit.GetComponentInChildren<SpriteRenderer>();
            sr.material = sprite_with_lights_material;
            sr.gameObject.transform.localPosition = new Vector3(0, 0.18f, 0);
        }

        music_instructor.soundman.play_sound("confirm");
    }
    public void deletePreviewSpirits() {
        SpiritPreview[] previouspreviews = FindObjectsOfType<SpiritPreview>();
        foreach (SpiritPreview preview in previouspreviews) {
            Destroy(preview.gameObject);
        }
        total_preview_spirits = 0;
    }

    public void init_wandering_spirit_strangers() {
        stranger_spirits = new List<spirit_ai>();

        float start_x = -11f;
        float start_y = -5f;
        float max_x = 11f;
        float max_y = 5f;
        float x = start_x;
        float y = start_y;

        while (y <= max_y) {

            if (Random.Range(1f, 5f) > 3f) {
                GameObject spirit_stranger = (GameObject)Instantiate(spirit);

                Animator new_spirit_animator = spirit_stranger.GetComponentInChildren<Animator>();
                new_spirit_animator.StopPlayback();
                new_spirit_animator.Play("spirit_stranger");
                spirit_stranger.transform.position = new Vector3(
                    x,
                    y,
                    spirit_stranger.transform.position.z
                );
                spirit_ai spirit_stranger_ai = spirit_stranger.GetComponent<spirit_ai>();
                spirit_stranger_ai.activateStrangerMode();
                spirit_stranger_ai.Activate();
                spirit_stranger_ai.base_speed = 5f;

                stranger_spirits.Add(spirit_stranger_ai);
            }

            x++;
            if (x > max_x) {
                x = start_x;
                y++;
            }
        }

        UpdateStrangerSpiritsGridInfo();
        StartCoroutine(UpdateStrangerSpiritsGrid());
    }

    public class SpiritGridRow
    {
        public int count_x;
        public bool[] count_y = new bool[24];
    }

    void UpdateStrangerSpiritsGridInfo() {
        if (spirits_grid.Count == 0) {
            for (int i = 0; i < 24; i++) {
                spirits_grid.Add(new SpiritGridRow());
            }
        }
        else {
            foreach (SpiritGridRow row in spirits_grid) {
                for (int i = 0; i < row.count_y.Length; i++) {
                    row.count_y[i] = false;
                }
            }

        }

        foreach (spirit_ai stranger in stranger_spirits) {
            GridCoordinates coords = new GridCoordinates(stranger.transform.position.x, stranger.transform.position.y);
            float x = coords.x();
            float y = coords.y();

            if (x > 0 && x < 24) {
                spirits_grid[Mathf.FloorToInt(x)].count_x = Mathf.FloorToInt(x);
                spirits_grid[Mathf.FloorToInt(x)].count_y[Mathf.FloorToInt(y)] = true;
            }
        }
    }

    public bool is_stranger_spirit_on_coords(float x, float y) {
        GridCoordinates coords = new GridCoordinates(x, y);

        return spirits_grid[Mathf.FloorToInt(coords.x())].count_y[Mathf.FloorToInt(coords.y())];
    }

    IEnumerator UpdateStrangerSpiritsGrid() {
        while (true) {
            UpdateStrangerSpiritsGridInfo();
            yield return new WaitForSeconds(0.5f);
        }
    }

    void init_gates() {
        gates = GameObject.FindObjectsOfType<SpiritGate>();
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
        pausescreen.init_pause_screen();
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

    void summon_crystomalities() {
        if (!spawn_crystomalities) {
            return;
        }

        if (!game_is_running) {
            return;
        }

        if (total_crystomalities < 5) {
            float time_difference = Time.time - time_last_crystomality_spawn;

            if (time_difference > 3f) {
                GameObject new_crystomality = (GameObject)Instantiate(crystomality);
                int new_x = Random.Range(-9, 9);
                int new_y = Random.Range(-5, 5);
                if (grid_manager.objects_exist_at_coords(new_x, new_y)) {
                    return;
                }
                new_crystomality.transform.position = new Vector3(new_x, new_y, 0);
                time_last_crystomality_spawn = Time.time;
                total_crystomalities++;
                music_instructor.soundman.play_sound("crystomality", pitch_amount: 12, volume: 0.5f);
            }
        }
    }

    void init_grid() {
        grid_master = GameObject.FindGameObjectWithTag("GridMaster");
        grid_manager= grid_master.GetComponent<GridManager>();
        grid_manager.build_grid();
    }

    void init_tower_placement_markers() {
        GameObject[] towerplacementmarkers = GameObject.FindGameObjectsWithTag("TowerPlacementMarker");

        foreach (GameObject marker in towerplacementmarkers) {
            int marker_x = grid_manager.round(marker.transform.position.x);
            int marker_y = grid_manager.round(marker.transform.position.y);

            grid_manager.build_tower(marker_x, marker_y);

            Destroy(marker);
        }
    }

    void console_log(string text) {
        LevelLayout level_layout = FindObjectOfType<LevelLayout>();
        level_layout.console.text += "\n" + text;
    }

    void init_spirits() {
        spirits = new List<spirit_ai>();
        spiritmaster = GameObject.FindGameObjectWithTag("SpiritMaster");
        spiritspawnpoints = GameObject.FindGameObjectsWithTag("SpiritSpawnpoint");

        int spawnpoint_count = 0;
        int spirit_count = 0;
        string st = "abcdefghijklmnopqrstuvwxyz";

        while (spirit_count < spirit_limit) {
            GameObject spiritspawnpoint = spiritspawnpoints[spawnpoint_count];

            float spirit_x = spiritspawnpoint.transform.position.x;
            float spirit_y = spiritspawnpoint.transform.position.y;

            float spirit_x_new = spirit_x - 0.3f + Random.Range(0, 0.6f);
            float spirit_y_new = spirit_y - 0.3f + Random.Range(0, 0.6f);

            GameObject new_spirit = (GameObject)Instantiate(
                spirit, new Vector3(spirit_x_new, spirit_y_new, 0.1f), transform.rotation, spiritmaster.transform
            );

            if (night) {
                GameObject new_spirit_light = (GameObject)Instantiate(
                    spiritlight, new_spirit.transform
                );
                new_spirit_light.transform.localPosition = new Vector3(
                    0, 0, -6f
                );
            }

            spirit_ai spiritscript = new_spirit.GetComponent<spirit_ai>();
            spiritscript.animation_letter = st[spirit_count].ToString();
            spirits.Add(spiritscript);

            Animator new_spirit_animator = new_spirit.GetComponentInChildren<Animator>();
            new_spirit_animator.StopPlayback();
            new_spirit_animator.Play("spirit_" + st[spirit_count]);

            SpriteRenderer new_spirit_renderer = new_spirit.GetComponentInChildren<SpriteRenderer>();
            new_spirit_renderer.sortingOrder = 29 + spirit_count;
            spirit_count++;

            spawnpoint_count++;

            if (spawnpoint_count >= spiritspawnpoints.Length) {
                spawnpoint_count = 0;
            }
        }
    }

    void show_required_letters_above_gate() {
        if (gate_letters == null) {
            return;
        }
        float letter_width = 0.7F;
        float total_width = (gate_letters.Length - 1) * letter_width;
        GameObject gate = gates[0].gameObject; // just take first gate for this
        float x = gate.transform.position.x - total_width / 2;
        float y = gate.transform.position.y + 1F;
        float z = gate.transform.position.z;
        foreach (GateLetter letter in gate_letters) {
            GameObject new_letter = (GameObject) Instantiate(spirit_letter, new Vector3(x, y, z), Quaternion.identity);
            SpiritLetter spiritletter = new_letter.GetComponentInChildren<SpiritLetter>();
            spiritletter.setLetter(spiritletter.findNumberForLetter(letter.text));
            x += letter_width;
        }
    }

    public void check_for_spirit_buttons() {
        spirit_buttons = GameObject.FindGameObjectsWithTag("SpiritButton");
        super_chargers = GameObject.FindGameObjectsWithTag("SuperCharger");
        heart_beans = GameObject.FindGameObjectsWithTag("HeartBean");
        leaves = GameObject.FindGameObjectsWithTag("Leaf");
        shield_symbols = GameObject.FindObjectsOfType<ShieldSymbol>();

        if (spirit_buttons.Length > 0 || heart_beans.Length > 0 ||
            leaves.Length > 0 || super_chargers.Length > 0 || shield_symbols.Length > 0 ||
            total_crystomalities > 0 || has_hidden_gate) {
            CloseTheGate();
        }
        else {
            OpenTheGate();
        }
    }

    public void CheckGateConditions() {
        // check various conditions and open / close gate depending on it
        bool open_the_gate = true;

        if (!check_all_leaf_piles_grown()) {
            open_the_gate = false;
        }

        if (!check_all_heart_beans_blooming()) {
            open_the_gate = false;
        }

        if (!check_all_super_chargers_charged()) {
            open_the_gate = false;
        }

        if (!check_all_spirit_buttons_pressed()) {
            open_the_gate = false;
        }

        if (total_crystomalities > 0) {
            open_the_gate = false;
        }

        if (open_the_gate) {
            OpenTheGate();
        }
        else {
            CloseTheGate();
        }
    }

    public void check_symbols_order(float x, float y) {
        int cursor_x = grid_manager.round(x);
        int cursor_y = grid_manager.round(y);

        int box_count = 0;

        foreach (ShieldSymbol sym in shield_symbols) {
            float symx = grid_manager.round(sym.gameObject.transform.position.x);
            float symy = grid_manager.round(sym.gameObject.transform.position.y);
            GameObject box = grid_manager.get_tower(symx, symy);

            if (box != null) {
                box_count++;
            }
        }

        // reset when no boxes placed
        if (box_count == 0) {
            current_shield_symbol_count = 0;

            reset_shield_symbols();
        }

        foreach (ShieldSymbol sym in shield_symbols) {
            if (grid_manager.round(sym.gameObject.transform.position.x) == cursor_x &&
                grid_manager.round(sym.gameObject.transform.position.y) == cursor_y) {

                if (current_shield_symbol_count + 1 == sym.sequence_number) {
                    current_shield_symbol_count++;
                    sym.set_checked();
                }
                else {
                    current_shield_symbol_count = 0;
                    reset_shield_symbols();
                }

                if (current_shield_symbol_count == 4) {
                    OpenTheGate();
                }
            }
        }
    }

    public void reset_shield_symbols() {
        foreach (ShieldSymbol sym in shield_symbols) {
            float symx = grid_manager.round(sym.gameObject.transform.position.x);
            float symy = grid_manager.round(sym.gameObject.transform.position.y);
            GameObject box = grid_manager.get_tower(symx, symy);

            if (box != null && box.transform.childCount > 0) {
                grid_manager.delete_tower(symx, symy);
                sym.spawnWood();
            }

            sym.set_unchecked();
        }
    }
    public bool check_all_spirit_buttons_pressed() {
        if (spirit_buttons.Length == 0) {
            return true;
        }

        int active_buttons = 0;
        foreach (GameObject button in spirit_buttons) {
            SpiritButton button_script = button.GetComponent<SpiritButton>();
            if (button_script.active) {
                active_buttons++;
            }
        }

        if (active_buttons == spirit_buttons.Length) {
            foreach (Transform spirit in spiritmaster.transform) {
                spirit_ai spirit_script = spirit.gameObject.GetComponent<spirit_ai>();
                spirit_script.step_away_from_button();
            }

            if (has_waiting_room_display && waitingroomdisplay.current_spirit == -1) {
                waitingroomdisplay.nextSpirit();
            }

            return true;
        }

        return false;
    }

    public bool check_all_super_chargers_charged() {
        int active_chargers = 0;
        foreach (GameObject charger in super_chargers) {
            SuperCharger charger_script = charger.GetComponent<SuperCharger>();
            if (charger_script.fully_charged) {
                active_chargers++;
            }
        }

        if (active_chargers == super_chargers.Length) {
            return true;
        }

        return false;
    }

    public bool check_all_heart_beans_blooming() {
        if (heart_beans.Length == 0) {
            return true;
        }

        int blooming_beans = check_all_heart_beans_blooming_count();
        if (blooming_beans == heart_beans.Length) {
            return true;
        }

        return false;
    }

    public int check_all_heart_beans_blooming_count() {
        int blooming_beans = 0;
        foreach (GameObject bean in heart_beans) {
            heartbean bean_script = bean.GetComponent<heartbean>();
            if (bean_script.blooming) {
                blooming_beans++;
            }
        }

        return blooming_beans;
    }

    public bool check_all_leaf_piles_grown() {
        if (leaves.Length == 0) {
            return true;
        }

        int grown_piles = 0;
        foreach (GameObject a_leaf in leaves) {
            leaf leaf_script = a_leaf.GetComponent<leaf>();
            if (leaf_script.is_pile) {
                grown_piles++;
            }
        }

        if (grown_piles == leaves.Length) {
            return true;
        }

        return false;
    }

    void activate_spirits_movement_for_beatbox() {
        foreach (Transform spirit in spiritmaster.transform) {
            spirit_ai spirit_script = spirit.gameObject.GetComponent<spirit_ai>();
            spirit_script.move_for_beatbox();
        }
    }

    public void beatbox_kick_boost_spirits() {
        float additional_boost = 0;
        if (Time.time - cattyscript.last_beat_time < 0.2f) {
            additional_boost += 2.5f;
        }
        if (Time.time - battyscript.last_beat_time < 0.2f) {
            additional_boost += 2.5f;
        }

        if (grid_manager.shromps.Length > 0) {
            foreach (Shromp shromp in grid_manager.shromps) {
                if (Time.time - shromp.last_jomp < 0.2f) {
                    additional_boost += 2.5f;
                }
            }
        }

        boost_spirits(additional_boost);
        // allow player to also place box after the kick
        if (additional_boost == 0) {
            StartCoroutine(beatbox_spirit_boost_after_kick(cattyscript, battyscript, spiritmaster.transform, grid_manager));
        }
    }

    public void boost_spirits(float additional_boost) {
        foreach (Transform spirit in spiritmaster.transform) {
            spirit_ai spirit_script = spirit.gameObject.GetComponent<spirit_ai>();
            spirit_script.beatbox_boost(additional_boost);
        }
    }

    private static IEnumerator beatbox_spirit_boost_after_kick(player_movement cattyscript, player_movement battyscript, Transform spiritmaster, GridManager grid_manager) {
        yield return new WaitForSeconds(0.1f);
        float additional_boost = 0;
        if (Time.time - cattyscript.last_beat_time < 0.3f) {
            additional_boost += 2.5f;
        }
        if (Time.time - battyscript.last_beat_time < 0.3f) {
            additional_boost += 2.5f;
        }
        if (grid_manager.shromps.Length > 0) {
            foreach (Shromp shromp in grid_manager.shromps) {
                if (Time.time - shromp.last_jomp < 0.2f) {
                    additional_boost += 2.5f;
                }
            }
        }
        if (additional_boost > 0) {
            foreach (Transform spirit in spiritmaster.transform) {
                spirit_ai spirit_script = spirit.gameObject.GetComponent<spirit_ai>();
                spirit_script.beatbox_boost(additional_boost);
            }
        }
    }

    public void CloseTheGate() {
        foreach (SpiritGate gate in gates) {
            gate.Close();
        }
    }

    public void OpenTheGate() {
        foreach (SpiritGate gate in gates) {
            gate.Open();
        }
    }

    void startWind() {
        InvokeRepeating("Wind", 4f, 10f);
    }

    void Wind() {
        create_swirleys();
    }

    void create_swirleys() {
        // create one for every 2nd X cordinate, random Y
        float delay = 0;
        for (int x = -9; x < 9; x += 2) {
            delay += 0.1f;
            StartCoroutine(spawn_swirley(windswirley, delay, x, spiritmaster, this));
        }
    }

    private static IEnumerator spawn_swirley(GameObject windswirley, float delay, float x, GameObject spiritmaster, Gamemaster self) {

        yield return new WaitForSeconds(delay);

        float y = Random.Range(-5f, 5f);

        GameObject swirlwind = (GameObject)Instantiate(
            windswirley, new Vector3(x, y, 0.1f), windswirley.transform.rotation
        );
        //self.music_instructor.soundman.play_sound("windchime" + Mathf.Clamp(Mathf.Floor(y), 1, 8));
        self.music_instructor.soundman.play_sound("windchime" + Random.Range(1, 9), volume: Random.Range(0.2f, 0.6f));

        foreach (Transform child in spiritmaster.transform) {
            spirit_ai spirit_intelligence = child.gameObject.GetComponent<spirit_ai>();
            spirit_intelligence.blow_to_the_right(x);
        }
    }

    void startRain() {
        int starCount = 0;
        while (starCount < 50) {
            GameObject newstar = (GameObject)Instantiate(star);
            starCount++;
        }
    }

    void setSpiritsToFollow() {

        bool follow_catty = true;

        float follow_number = 0f;

        foreach (Transform child in spiritmaster.transform) {
            spirit_ai spirit_intelligence = child.gameObject.GetComponent<spirit_ai>();
            spirit_intelligence.Activate();
            spirit_intelligence.follow_player = true;
            spirit_intelligence.follow_number = follow_number;

            if (followPlayersByXAxis) {
                if (child.transform.position.x < 0) {
                    spirit_intelligence.my_player = catty;
                }
                else {
                    spirit_intelligence.my_player = batty;
                    follow_number += 0.1f;
                }
            }
            else {
                if (follow_catty) {
                    spirit_intelligence.my_player = catty;
                    follow_catty = false;
                }
                else {
                    spirit_intelligence.my_player = batty;
                    follow_catty = true;
                    follow_number += 0.1f;
                }
            }
        }
    }

    public void check_spirit_count() {
        // if 1, then it's the last spirit, game is about to end
        if (spiritmaster.transform.childCount <= 1) {

            end_game();
        }
    }

    public void hedgar_got_glasses() {
        if (game_is_running == false)
            return;

        // when hedgar gets his glasses, display happy hedgar and end game
        HeadgarTreeWaiting hedgar = FindObjectOfType<HeadgarTreeWaiting>();
        hedgar.happy();

        end_game();
    }

    public void revealGates() {
        foreach (SpiritGate gate in gates) {
            gate.Reveal();
        }
        has_hidden_gate = false;
        OpenTheGate();
    }

    public void end_game() {
        game_is_running = false;

        game_finished_since = Time.time;

        CompletePopup popup = FindObjectOfType<CompletePopup>();

        if (level_number < 11) {
            popup.hedgar.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        float total_playtime = Time.time - level_start_time;
        popup.showPopup(grid_manager.total_towers_placed, total_playtime);
        set_high_score();
        music_instructor.soundman.play_sound("levelpopup_complete");
        music_instructor.soundman.play_sound("levelcomplete_rattle1");
        music_instructor = FindObjectOfType<MusicInstructor>();

        float pitch = 1f;
        if (level_number > 31) {
            pitch = 0.9f;
        }

        music_instructor.PlayMusic("winninglevel", pitch);
    }

    public void set_high_score() {
        float total_playtime = Time.time - level_start_time;
        universemaster.SetHighScore(level_number - 1, grid_manager.total_towers_placed, total_playtime);
        update_played_levels(true);
    }

    public int spirits_left() {
        return spiritmaster.transform.childCount;
    }

    public void finish_level() {
        if (Time.time - game_finished_since > 2f) {
            return_to_map();
        }
    }

    public void update_played_levels(bool win) {
        if (win) {
            universemaster.set_highest_played_level(level_number);
            universemaster.check_win_achievement(level_number);
        }
        universemaster.set_last_played_level(level_number);
        universemaster.save_progress();
    }

    public void update_tower_display(int total_towers) {
        if (towerdisplay == null) {
            return;
        }

        towerdisplay.text = "x " + (box_limit - total_towers);
    }

    public void return_to_map() {
        game_is_running = false;
        returning_to_map = true;
        StartCoroutine(universemaster.LoadOverworld(universemaster));
    }


    public void reset_level() {
        StopAllCoroutines();
        cattyscript.destroyInputEvents();
        battyscript.destroyInputEvents();
        universemaster.reset_level();
    }

    public void init_players() {
        catty = GameObject.Find("catty");
        batty = GameObject.Find("batty");

        cattyscript = catty.GetComponent<player_movement>();
        battyscript = batty.GetComponent<player_movement>();

        cattyscript.setCursorStyle(get_cursor_style("catty"));
        battyscript.setCursorStyle(get_cursor_style("batty"));
    }

    public void switchPlayerIds(bool shoothearts = true) {
        if (Time.time - time_last_player_switch < 1f) {
            return;
        }

        time_last_player_switch = Time.time;

        cattyscript.destroyInputEvents();
        battyscript.destroyInputEvents();

        if (cattyscript.player_id == 0) {
            cattyscript.setPlayerByPlayerId(1);
            battyscript.setPlayerByPlayerId(0);
        }
        else {
            cattyscript.setPlayerByPlayerId(0);
            battyscript.setPlayerByPlayerId(1);
        }

        cattyscript.addInputEvents();
        battyscript.addInputEvents();

        if (shoothearts) {
            music_instructor.soundman.play_sound("switchplayers_real");
            cattyscript.ShootHeartAt(battyscript.transform);
            battyscript.ShootHeartAt(cattyscript.transform);
        }
    }

    public void activate_spirits(string who) {
        if (spirits_active) {
            return;
        }

        deletePreviewSpirits();

        float spirit_count = 1F;
        foreach (Transform child in spiritmaster.transform) {
            StartCoroutine(SpiritActivator(child.gameObject, spirit_count - 1f));
            spirit_count += 1F;
        }
        spirits_active = true;

        LevelLayout level_layout = FindObjectOfType<LevelLayout>();
        level_layout.removeLevelHint();

        LetsGoBig letsgo = FindObjectOfType<LetsGoBig>();
        letsgo.LetsGo(who);

        BegoneSigns();

        music_instructor.soundman.play_sound("start_spirits");
        music_instructor.SwitchLayer();
    }

    public void BegoneSigns() {
        LetsgoSign[] signs = FindObjectsOfType<LetsgoSign>();
        foreach (LetsgoSign sign in signs) {
            sign.Begone();
        }
    }

    public int get_cursor_style(string name) {
        if (name == "batty") {
            return universemaster.observer.batty_style_number;
        }

        return universemaster.observer.catty_style_number;
    }

    public void save_cursor_style(string name, int style) {
        if (name == "batty") {
            universemaster.observer.batty_style_number = style;
        }
        else {
            universemaster.observer.catty_style_number = style;
        }
        universemaster.save_progress();
    }

    public bool is_player_switched() {
        return universemaster.observer.player_control_switched;
    }

    public void save_player_switched() {
        universemaster.observer.player_control_switched = universemaster.observer.player_control_switched ? false : true;
        universemaster.save_progress();
    }

    private static IEnumerator SpiritActivator(GameObject spirit, float delay) {
        float start = Time.time;
        while (Time.time <= start + delay) {
            yield return null;
        }
        spirit.BroadcastMessage("Activate");
    }

}
