using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class spirit_ai : MonoBehaviour {
    GridManager grid_manager;
    GameObject gamemanager;
    Gamemaster gamemanager_script;
    GameObject current_tower;
    GameObject last_tower;
    GameObject next_tower;
    List<GameObject> available_space;
    bool moving = false;
    public bool preview_mode = false;
    bool stranger_mode = false;
    float born_time = 0;
    private float started_moving;
    public float speed;
    public float base_speed = 2.5f;
    public float speed_boost = 0f;
    float moved_distance;
    Vector3 start_position;
    public bool active = false;
    public GameObject spirit_letter;
    GameObject[] spirit_letter_dispensers;
    List<GameObject> letter_displays;
    List<GateLetter> collected_letters;
    GameObject last_letter_dispenser;
    GameObject[] portals;
    GameObject[] spirit_buttons;
    GameObject[] super_chargers;
    GameObject[] heartbeans;
    Shromp[] shromps;

    bool stunned_by_portal = false;
    bool moving_towards_spawnpoint = false;
    bool asleep_from_eyeflower = false;
    bool standing_on_button = false;
    public bool follow_player = false;
    public bool has_hedgar_glasses = false;
    public float follow_number;
    public GameObject my_player;
    int spirit_colliding_count = 0;
    public int higher_spirit_colliding_count = 0;
    public GameObject heartvine_crank_handle;
    public Material normal_sprite_material;

    GameObject stunning_portal;
    GameObject my_button;
    public SpiritWayWiser my_waywiser;

    bool riding_cattypillar = false;
    public bool moving_to_beatbox = false;
    float started_riding;
    float stopped_riding = 0;
    bool wandering_left = false;
    GameObject target_cattypillar_tail;

    public GameObject spiritsmoke;
    public Animator myspiritsmoke;

    public Animator spiritanimator;
    public string animation_letter;
    public GameObject asleepzzz;
    GameObject my_asleepzzz;

    int spirit_number;

    public void Activate() {
        active = true;
    }

    public void fallAsleep() {
        if (asleep_from_eyeflower) {
            return;
        }
        asleep_from_eyeflower = true;
        speed_boost = 2f;
        speed += speed_boost;
        spiritanimator.Play("asleepspirit_" + animation_letter);
        my_asleepzzz = (GameObject)Instantiate(asleepzzz);
        my_asleepzzz.transform.position = new Vector3(
            transform.position.x + 0.6f,
            transform.position.y + 0.6f,
            transform.position.z
        );
    }

    public void WakeUp() {
        if (asleep_from_eyeflower == false) {
            return;
        }
        asleep_from_eyeflower = false;
        spiritanimator.Play("spirit_" + animation_letter);
        if (my_asleepzzz != null) {
            Destroy(my_asleepzzz);
        }
    }

    // Use this for initialization
    void Start () {
        spirit_number = gameObject.transform.GetSiblingIndex();
        if (born_time == 0)
            born_time = Time.time;

        reset_speed();

        grid_manager = GameObject.FindGameObjectWithTag("GridMaster").GetComponent<GridManager>();
        gamemanager = GameObject.FindGameObjectWithTag("GameMaster");
        gamemanager_script = gamemanager.GetComponent<Gamemaster>();
        available_space = new List<GameObject>();
        letter_displays = new List<GameObject>();
        collected_letters = new List<GateLetter>();

        if (stranger_mode)
            return;

        portals = GameObject.FindGameObjectsWithTag("Portal");

        spirit_letter_dispensers = GameObject.FindGameObjectsWithTag("SpiritLetterDispenser");

        spirit_buttons = GameObject.FindGameObjectsWithTag("SpiritButton");
        super_chargers = GameObject.FindGameObjectsWithTag("SuperCharger");
        heartbeans = GameObject.FindGameObjectsWithTag("HeartBean");
        shromps = FindObjectsOfType<Shromp>();

        GameObject newspiritsmoke = (GameObject)Instantiate(spiritsmoke);
        myspiritsmoke = newspiritsmoke.GetComponent<Animator>();
        adjust_spirit_smoke();

        stopped_riding = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (!active)
            return;

        if (stranger_mode) {
            if (wandering_left) {
                wander_left();
                return;
            }
        }

        // check for stranger spirits on the same coordinates
        if (!stranger_mode && gamemanager_script.wandering_spirit_strangers
            && gamemanager_script.is_stranger_spirit_on_coords(transform.position.x, transform.position.y)) {

            return;
        }

        if (gamemanager_script.has_waiting_room_display
            && gamemanager_script.waitingroomdisplay.current_spirit > -1
            && gamemanager_script.waitingroomdisplay.current_spirit != spirit_number) {
            return;
        }

        if (gamemanager_script.is_paused) {
            return;
        }

        if (asleep_from_eyeflower) {
            return;
        }

        if (riding_cattypillar) {
            ride_cattypillar();
            return;
        }

        if (stunned_by_portal) {
            move_towards_portal();
            return;
        }

        if (transform.localScale.x < 0.5f) {
            float newscale = Mathf.Clamp(transform.localScale.x + 0.05f, 0, 0.5f);
            transform.localScale = new Vector3(newscale, newscale, 0);
        }

        if (standing_on_button) {
            return;
        }

        if (follow_player) {
            move_towards_player();
            check_surroundings();
            return;
        }

        // wait for leading spirits to clear the field
        if (higher_spirit_colliding_count > 0) {
            return;
        }

        if (preview_mode && Time.time - born_time >= 30) {
            Destroy(gameObject);
            return;
        }

        if (moving) {
            moved_distance += Time.deltaTime * speed;
            if (moved_distance < 1) {
                transform.position = Vector3.Lerp(start_position, next_tower.transform.position, moved_distance);
            }
            else {
                moving = false;
                if (Random.Range(0, 10f) > 7f) {
                    adjust_spirit_smoke();
                }
            }

            if (speed_boost > 0) {
                speed_boost -= 0.1f;
                speed = base_speed + speed_boost;
            }
        }

        if (!moving) {
            next_tower = check_towers();
            start_moving();
            check_surroundings();
        }
    }

    void adjust_spirit_smoke() {
        if (stranger_mode)
            return;

        myspiritsmoke.transform.position = transform.position;
        myspiritsmoke.StopPlayback();
        myspiritsmoke.Play("spiritsmoke", -1, 0f);
    }

    void wander_left() {
        float new_x = transform.position.x - 0.1f;
        float new_y = transform.position.y;
        if (new_x < -13f) {
            new_x = 13f;
            new_y = Mathf.Floor(Random.Range(-5, 5));
        }

        transform.position = new Vector3(
            new_x,
            new_y,
            transform.position.z
        );

        if (new_x > -11f && new_x <= 11f) {
            wandering_left = false;
            last_tower = null;
            moving = false;
        }
    }

    public void start_moving() {
        moving = true;
        started_moving = Time.time;
        moved_distance = 0.0f;
        start_position = gameObject.transform.position;
    }

    void check_surroundings() {
        if (stranger_mode)
            return;

        if (preview_mode) {
            check_gate();
            check_spirit_letter_dispenser();
            check_portals();
            return;
        }

        check_gate();
        check_spirit_letter_dispenser();
        check_spirit_buttons();
        check_super_chargers();
        check_heartbeans();
        if (!riding_cattypillar) {
            check_portals();
            check_shromps();
        }
    }

    void check_gate() {
        foreach (SpiritGate gate in gamemanager_script.gates) {
            float gate_x = gate.transform.position.x;
            float gate_y = gate.transform.position.y;

            float x = gameObject.transform.position.x;
            float y = gameObject.transform.position.y;

            float range = 0.2f;
            if (riding_cattypillar) {
                range = 0.7f;
            }
            if (follow_player) {
                range = 0.5f;
            }

            if (gate_x >= x - range && gate_x <= x + range &&
                gate_y >= y - range && gate_y <= y + range) {

                if (check_gate_letters() && gate.IsOpen() && !gamemanager_script.has_hidden_gate) {

                    if (gamemanager_script.has_hedgar_waiting_for_glasses) {
                        if (has_hedgar_glasses) {
                            gamemanager_script.hedgar_got_glasses();
                            //gate.spiritAnimation();
                        }
                        else {
                            return;
                        }
                    }
                    else {
                        if (gamemanager_script.spirits_active && gamemanager_script.gates_evaporate) {
                            gate.Dissoleave();
                        }
                        else {
                            gate.spiritAnimation();
                        }

                        check_waiting_room_display();
                        if (preview_mode) {
                            gamemanager_script.music_instructor.soundman.play_sound("spirit_enter_gate9", volume: 0.3f);
                        }
                        else {
                            int pitch = 0;
                            if (gamemanager_script.level_number > 31) {
                                pitch = -5;
                            }
                            gamemanager_script.music_instructor.soundman.play_sound("spirit_enter_gate" + (spirit_number + 1), volume: 0.5f, pitch_amount: pitch);
                        }
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    void check_waiting_room_display() {
        if (gamemanager_script.has_waiting_room_display
            && gamemanager_script.waitingroomdisplay.current_spirit > -1
            && gamemanager_script.waitingroomdisplay.current_spirit == spirit_number) {

            gamemanager_script.waitingroomdisplay.nextSpirit();
            return;
        }
    }

    bool check_gate_letters_count() {
        if (gamemanager_script.gate_letters == null) {
            return false;
        }

        if (gamemanager_script.gate_letters.Length == collected_letters.Count) {
            return true;
        }

        return false;
    }

    bool check_gate_letters() {
        if (gamemanager_script.gate_letters == null) {
            return true;
        }

        if (!check_gate_letters_count()) {
            reset_letters();
            return false;
        }

        int letter_count = 0;
        foreach (GateLetter letter in collected_letters) {
            if (letter.text != gamemanager_script.gate_letters[letter_count].text) {
                reset_letters();
                return false;
            }
            letter_count++;
        }

        return true;
    }

    void reset_letters() {
        collected_letters.Clear();
        delete_collected_letters_above_spirit();
        show_collected_letters_above_spirit();
    }

    void check_spirit_letter_dispenser() {
        GameObject hover_letter_dispenser = null;

        foreach (GameObject dispenser in spirit_letter_dispensers) {
            float dispenser_x = dispenser.transform.position.x;
            float dispenser_y = dispenser.transform.position.y;

            float x = gameObject.transform.position.x;
            float y = gameObject.transform.position.y;

            float range = 0.2f;
            if (riding_cattypillar || follow_player) {
                range = 0.7f;
            }

            if (dispenser_x >= x - range && dispenser_x <= x + range &&
                dispenser_y >= y - range && dispenser_y <= y + range) {

                hover_letter_dispenser = dispenser;
            }
        }

        if (hover_letter_dispenser != null) {
            if (last_letter_dispenser == null && collected_letters.Count < 30) {
                last_letter_dispenser = hover_letter_dispenser;
                collected_letters.Add(hover_letter_dispenser.GetComponent<SpiritLetterDispenser>().gate_letter);
                delete_collected_letters_above_spirit();
                show_collected_letters_above_spirit();
                gamemanager_script.music_instructor.soundman.play_sound("pickupletter", Random.Range(-2, 2), volume: 1f);
            }
        }
        else {
            last_letter_dispenser = null;
        }
    }

    void check_portals() {
        foreach (GameObject portal in portals) {
            float portal_start_x = portal.transform.position.x - 1.4f;
            float portal_start_y = portal.transform.position.y - 1.4f;
            float portal_end_x = portal.transform.position.x + 1.4f;
            float portal_end_y = portal.transform.position.y + 1.4f;

            float x = gameObject.transform.position.x;
            float y = gameObject.transform.position.y;

            if (portal_start_x <= x && x <= portal_end_x &&
                portal_start_y <= y && y <= portal_end_y && standing_on_button != true) {

                stunned_by_portal = true;
                stunning_portal = portal;
                moving = false;
                start_position = gameObject.transform.position;
                moved_distance = 0;
                gamemanager_script.music_instructor.soundman.play_sound("slide2", Random.Range(-2, 2), volume: 1f);
            }
        }
    }

    void check_spirit_buttons() {
        foreach (GameObject button in spirit_buttons) {
            float button_start_x = button.transform.position.x - 0.5f;
            float button_start_y = button.transform.position.y - 0.5f;
            float button_end_x = button.transform.position.x + 0.5f;
            float button_end_y = button.transform.position.y + 0.5f;

            float x = gameObject.transform.position.x;
            float y = gameObject.transform.position.y;

            if (button_start_x <= x && x <= button_end_x &&
                button_start_y <= y && y <= button_end_y) {

                SpiritButton button_script = button.gameObject.GetComponent<SpiritButton>();

                if (button_script.active == false) {
                    gamemanager_script.music_instructor.soundman.play_sound("kick28", volume: 0.3f, pan: x);
                    button.GetComponent<SpiritButton>().Activate();
                    standing_on_button = true;
                    my_button = button;
                    transform.position = new Vector3(button.transform.position.x, button.transform.position.y, transform.position.z);
                }

                gamemanager_script.CheckGateConditions();
            }
        }
    }

    bool spirit_is_on_gameobject(GameObject given_object) {
        float start_x = given_object.transform.position.x - 0.5f;
        float start_y = given_object.transform.position.y - 0.5f;
        float end_x = given_object.transform.position.x + 0.5f;
        float end_y = given_object.transform.position.y + 0.5f;

        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;

        if (start_x <= x && x <= end_x &&
            start_y <= y && y <= end_y) {
            return true;
        }
        else {
            return false;
        }
    }

    void check_super_chargers() {
        foreach (GameObject charger in super_chargers) {
            if (spirit_is_on_gameobject(charger)) {
                SuperCharger charger_script = charger.gameObject.GetComponent<SuperCharger>();
                charger_script.charge();
            }
        }
    }

    void check_heartbeans() {
        foreach (GameObject heartbean in heartbeans) {
            float range = 0.5f;
            if (riding_cattypillar || follow_player) {
                range = 0.7f;
            }

            float button_start_x = heartbean.transform.position.x - range;
            float button_start_y = heartbean.transform.position.y - range;
            float button_end_x = heartbean.transform.position.x + range;
            float button_end_y = heartbean.transform.position.y + range;

            float x = gameObject.transform.position.x;
            float y = gameObject.transform.position.y;

            if (button_start_x <= x && x <= button_end_x &&
                button_start_y <= y && y <= button_end_y) {

                heartbean heartbean_script = heartbean.gameObject.GetComponent<heartbean>();

                speed_boost = 3f;

                if (heartbean_script.blooming == false) {
                    heartbean_script.startAnimation();
                    heartbean_script.blooming = true;

                    gamemanager_script.CheckGateConditions();
                    gamemanager_script.music_instructor.soundman.play_sound("heartbean", pitch_amount: Random.Range(-2, 2), volume: 0.5f);
                }
            }
        }
    }

    void check_shromps() {
        foreach (Shromp shromp in shromps) {
            float button_start_x = shromp.transform.position.x - 0.5f;
            float button_start_y = shromp.transform.position.y - 0.5f;
            float button_end_x = shromp.transform.position.x + 0.5f;
            float button_end_y = shromp.transform.position.y + 0.5f;

            float x = gameObject.transform.position.x;
            float y = gameObject.transform.position.y;

            if (button_start_x <= x && x <= button_end_x &&
                button_start_y <= y && y <= button_end_y) {

                shromp.jomp();
            }
        }
    }

    public void move_for_beatbox() {
        moving_to_beatbox = true;
        base_speed = 0f;
        speed = 0.05f;
    }

    public void beatbox_boost(float additional_boost) {
        speed_boost = 1f + additional_boost;
    }

    public void step_away_from_button() {
        if (standing_on_button) {
            standing_on_button = false;
            my_button = null;
        }
    }

    public void blow_to_the_right(float x) {
        if (!active)
            return;

        int current_x = grid_manager.round(gameObject.transform.position.x);
        int current_y = grid_manager.round(gameObject.transform.position.y);
        GameObject tower_right = get_tower(current_x + 1, current_y);

        if (x < (current_x - 1) && x > current_x) {
            return;
        }

        if (!is_tower_available(tower_right)) {
            return;
        }
        current_tower = get_tower(current_x, current_y);

        last_tower = current_tower;

        next_tower = tower_right;
        start_moving();
        check_surroundings();
    }

    void ride_cattypillar() {
        cattypillar cp = target_cattypillar_tail.GetComponent<cattypillar>();
        Vector3 target = cp.tails[0].transform.position;
        transform.position = new Vector3(
            target.x - 0.2f + (spirit_number/20f),
            target.y + 0.5f,
            gameObject.transform.position.z
        );

        check_surroundings();

        if (Time.time - started_riding > 2f && Random.Range(0, 10f) > 9.8f) {
            stopped_riding = Time.time;
            descend_from_cattypillar();
        }
    }

    void descend_from_cattypillar() {
        riding_cattypillar = false;
        target_cattypillar_tail = null;
        last_tower = null;
        moving = false;
    }

    void move_towards_portal() {
        if (moving_towards_spawnpoint) {
            move_towards_spawnpoint();
            return;
        }

        speed += 0.1f;
        moved_distance += Time.deltaTime * speed;
        transform.position = Vector3.Lerp(start_position, stunning_portal.transform.position, moved_distance);

        float distance = Vector3.Distance(transform.position, stunning_portal.transform.position);

        transform.localScale = new Vector3(Mathf.Clamp(distance, 0, 0.5f), Mathf.Clamp(distance, 0, 0.5f), 1f);

        hide_letters();

        if (distance <= 0.2f) {
            GameObject spawn_point = find_spawn_point();
            stunning_portal = spawn_point;
            start_position = gameObject.transform.position;
            moved_distance = 0;
            transform.localScale = new Vector3(1f, 1f, 1f);
            spiritanimator.Play("spirit_smol");
            moving_towards_spawnpoint = true;
            reset_speed();
        }
    }

    void move_towards_spawnpoint() {
        speed += 0.1f;
        moved_distance += Time.deltaTime * speed;
        transform.position = Vector3.Lerp(start_position, stunning_portal.transform.position, moved_distance);

        float distance = Vector3.Distance(transform.position, stunning_portal.transform.position);

        if (distance <= 0.2f) {
            GameObject spawn_point = find_spawn_point();
            moving_towards_spawnpoint = false;
            stunned_by_portal = false;
            stunning_portal = null;
            moving = false;
            last_tower = null;
            transform.position = new Vector3(spawn_point.transform.position.x, spawn_point.transform.position.y, transform.position.z);
            start_position = gameObject.transform.position;
            moved_distance = 0;
            reset_speed();
            big_letters();
            transform.localScale = new Vector3(0, 0, 1f);
            spiritanimator.Play("spirit_" + animation_letter);
        }
    }

    void move_towards_player() {
        float distance = Vector3.Distance(transform.position, my_player.transform.position);

        if (distance > 1f + follow_number * 2 && distance < 3f + follow_number * 2) {
            moved_distance += Time.deltaTime * 1f;
            transform.position = Vector3.Lerp(start_position, my_player.transform.position, moved_distance);
            if (!gamemanager_script.spirits_active) {
                if (gamemanager_script.game_is_running == false) {
                    return;
                }

                if (gamemanager_script.is_paused) {
                    return;
                }

                gamemanager_script.activate_spirits(my_player.name);
            }
            return;
        }

        moved_distance = 0;
        start_position = gameObject.transform.position;
    }

    void reset_speed() {
        speed = base_speed;
    }

    GameObject find_spawn_point() {
        return GameObject.Find("SpiritSpawnpoint");
    }

    void show_collected_letters_above_spirit() {
        float letter_width = 1.4F;
        float total_width = (collected_letters.Count - 1) * letter_width;
        float x = 0 - total_width / 2;
        foreach (GateLetter letter in collected_letters) {
            GameObject new_letter = (GameObject)Instantiate(spirit_letter, new Vector3(0, 0, 0), Quaternion.identity);
            letter_displays.Add(new_letter);
            new_letter.transform.parent = gameObject.transform;
            new_letter.transform.localPosition = new Vector3(x, 2F, 0);
            new_letter.transform.localScale = new Vector3(2F, 2F, 2F);

            SpiritLetter spiritletter = new_letter.GetComponentInChildren<SpiritLetter>();
            spiritletter.setLetter(spiritletter.findNumberForLetter(letter.text));
            spiritletter.ar.enabled = true;
            x += letter_width;
        }
    }

    void hide_letters() {
        foreach (GameObject letter in letter_displays) {
            if (letter != null) {
                letter.SetActive(false);
            }
        }
    }
    void big_letters() {
        foreach (GameObject letter in letter_displays) {
            if (letter != null) {
                letter.SetActive(true);
            }
        }
    }

    void delete_collected_letters_above_spirit() {
        foreach (GameObject letter in letter_displays) {
            Destroy(letter);
        }
    }

    GameObject get_tower(float x, float y) {

        return grid_manager.get_tower(x, y);
    }

    bool is_tower_the_same_as_last_tower(GameObject tower) {
        if (!tower) {
            return false;
        }

        if (GameObject.ReferenceEquals(tower, last_tower)) {
            return true;
        }

        return false;
    }

    bool is_tower_available(GameObject tower) {
        if (!tower) {
            return false;
        }

        if (tower.transform.childCount == 0) {
            return true;
        }

        return false;
    }

    public GameObject check_towers() {
        available_space = new List<GameObject>();
        int current_x = grid_manager.round(gameObject.transform.position.x);
        int current_y = grid_manager.round(gameObject.transform.position.y);
        current_tower = get_tower(current_x, current_y);

        GameObject tower_right = get_tower(current_x + 1, current_y);
        //        if (tower_right) {
        //            Debug.DrawLine(current_tower.transform.position, tower_right.transform.position, Color.white, 2.5f);
        //        }
        GameObject tower_top = get_tower(current_x, current_y - 1);
        GameObject tower_left = get_tower(current_x - 1, current_y);
        GameObject tower_bottom = get_tower(current_x, current_y + 1);

        if (!last_tower) {
            last_tower = current_tower;
        }

        GameObject same_direction_tower = get_direction_tower(current_x, current_y, last_tower);

        // check waywiser for suggested direction
        GameObject waywiser_tower = get_waywiser_tower(tower_top, tower_right, tower_bottom, tower_left);
        if (waywiser_tower) {
            last_tower = current_tower;
            return waywiser_tower;
        }

        // strangers go to the left
        if (stranger_mode) {
            if (Random.Range(1f, 5f) > 2f && is_tower_available(tower_left)) {
                last_tower = current_tower;
                return tower_left;
            }
            if (!tower_left) {
                wandering_left = true;
            }
        }
        else {
            // keep direction a priority
            if (is_tower_available(same_direction_tower) && !is_tower_the_same_as_last_tower(same_direction_tower)) {
                last_tower = current_tower;
                return same_direction_tower;
            }
        }

        // otherwise check all available directions randomly
        GameObject random_tower = get_random_tower(tower_top, tower_right, tower_bottom, tower_left);
        if (random_tower) {

            // if preview spirits have 2 available space, send clone to the alternative route
            if (preview_mode && available_space.Count == 2) {
                random_tower = available_space[0];
                clone_preview_spirit(available_space[1]);
            }

            if (preview_mode && (available_space.Count == 2 ||
                (available_space.Count >= 2 && Time.time - born_time <= 0.1f))) {

                random_tower = available_space[0];
                for (int i = 1; i < available_space.Count; i++) {
                    clone_preview_spirit(available_space[i]);
                }
            }

            last_tower = current_tower;
            return random_tower;
        }

        // if all else fails, return to last known tower
        // set current to last tower
        GameObject return_to_last_tower = last_tower;
        if (is_tower_available(last_tower)) {
            last_tower = current_tower;
            return return_to_last_tower;
        }

        last_tower = current_tower;
        return current_tower;
    }

    void clone_preview_spirit(GameObject target_tower) {
        if (gamemanager_script.total_preview_spirits < 10) {
            GameObject clone = (GameObject)Instantiate(gameObject);
            spirit_ai clone_ai = clone.GetComponent<spirit_ai>();
            clone_ai.activatePreviewMode();
            clone_ai.last_tower = last_tower;
            clone_ai.next_tower = target_tower;
            clone_ai.Activate();
            clone_ai.start_moving();
            clone_ai.born_time = born_time;
            gamemanager_script.total_preview_spirits++;
        }
    }

    GameObject get_random_tower(GameObject top, GameObject right, GameObject bottom, GameObject left) {
        if (is_tower_available(right) && !is_tower_the_same_as_last_tower(right)) {
            available_space.Add(right);
        }

        if (is_tower_available(left) && !is_tower_the_same_as_last_tower(left)) {
            available_space.Add(left);
        }

        if (is_tower_available(top) && !is_tower_the_same_as_last_tower(top)) {
            available_space.Add(top);
        }

        if (is_tower_available(bottom) && !is_tower_the_same_as_last_tower(bottom)) {
            available_space.Add(bottom);
        }

        if (available_space.Count > 0) {
            last_tower = current_tower;
            return available_space[Random.Range(0, available_space.Count)];
        }

        return null;
    }

    GameObject get_waywiser_tower(GameObject top, GameObject right, GameObject bottom, GameObject left) {
        if (!my_waywiser) {
            return null;
        }

        if (my_waywiser.arrow_down.on && is_tower_available(top)) {
            return top;
        }

        if (my_waywiser.arrow_right.on && is_tower_available(right)) {
            return right;
        }

        if (my_waywiser.arrow_up.on && is_tower_available(bottom)) {
            return bottom;
        }

        if (my_waywiser.arrow_left.on && is_tower_available(left)) {
            return left;
        }

        return null;
    }

    GameObject get_direction_tower(int current_x, int current_y, GameObject last_tower) {
        int last_x = grid_manager.round(last_tower.transform.position.x);
        int last_y = grid_manager.round(last_tower.transform.position.y);

        int next_x = current_x;
        int next_y = current_y;

        if (current_x < last_x) {
            next_x--;
        }
        else {
            if (current_x > last_x) {
                next_x++;
            }
        }

        if (current_y < last_y) {
            next_y--;
        }
        else {
            if (current_y > last_y) {
                next_y++;
            }
        }

        return get_tower(next_x, next_y);
    }

    private void adjust_light() {
        Light light = GetComponentInChildren<Light>();
        if (light) {
            light.intensity = 1 - (spirit_colliding_count * 0.12f);
        }
    }

    public void activatePreviewMode() {
        preview_mode = true;
    }

    public void activateStrangerMode() {
        stranger_mode = true;
        Destroy(GetComponentInChildren<SpiritWaveyScript>());
        Destroy(GetComponentInChildren<Animator>(), Random.Range(0.5f, 1.5f));
        GetComponentInChildren<SpriteRenderer>().material = normal_sprite_material;
        Destroy(GetComponent<CircleCollider2D>());
        Destroy(GetComponent<Rigidbody2D>());
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if (other.name == "catty" || other.name == "batty" || other.name == "tower_block") {
            if (asleep_from_eyeflower) {
                WakeUp();
            }
        }

        if (other.name == "Spirit(Clone)") {
            spirit_colliding_count++;
            spirit_ai other_spirit_ai = other.GetComponent<spirit_ai>();
            if (!other_spirit_ai.standing_on_button && !other_spirit_ai.stunned_by_portal
                && !preview_mode && !stranger_mode) {
                if (other.transform.GetSiblingIndex() < gameObject.transform.GetSiblingIndex()) {

                    higher_spirit_colliding_count++;
                }
            }
            adjust_light();
        }

        if (other.name.Contains("spiritwaywiser")) {
            my_waywiser = other.GetComponent<SpiritWayWiser>();
        }

        if (!preview_mode && gamemanager_script.has_hedgar_waiting_for_glasses && other.name.Contains("hedgar_glasses")) {
            other.GetComponent<HedgarGlasses>().attachToSpirit(this);
        }

        if (other.transform.parent == null) {
            return;
        }

        if (other.transform.parent.CompareTag("CattyPillar") && !stunned_by_portal && active
            && Time.time - stopped_riding > 2f && !riding_cattypillar) {
            riding_cattypillar = true;
            started_riding = Time.time;
            target_cattypillar_tail = other.transform.parent.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (stranger_mode)
            return;

        if (other.name == "Spirit(Clone)") {
            spirit_colliding_count--;
            if (other.transform.GetSiblingIndex() < gameObject.transform.GetSiblingIndex()) {
                higher_spirit_colliding_count--;
            }
            if (higher_spirit_colliding_count < 0) {
                higher_spirit_colliding_count = 0;
            }
            adjust_light();
        }

        if (other.name.Contains("spiritwaywiser")) {
            my_waywiser = null;
        }
    }

    private void OnDestroy() {
        if (!preview_mode && !stranger_mode)
            gamemanager_script.check_spirit_count();
    }
}
