using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class player_movement : MonoBehaviour
{
    public int player_id;
    public GameObject cursor_prefab;

    GameObject cursor;
    cursorScript cursorscript;
    private Player player;
    GridManager grid_manager;
    Gamemaster game_master;
    Animator animator;
    public GameObject eyes;
    public GameObject switchheart;
    public float last_built_tower_time = 0;
    public float last_beat_time = 0;
    public float last_grid_line_time = 0;
    SpriteRenderer eyes_spriterenderer;
    CattyBackFront cattybackfront;
    public bool is_hedgar = false;
    public bool is_batty = false;
    public GameObject poof;
    public GameObject bodysprite;
    public GameObject grid_line;
    public Camera cam;
    MusicInstructor instructor;

    public Vector3 look_at_target_pos;
    GameObject[] leaves;
    Rigidbody2D rb;
    public GameObject leafflying;

    float speed_modifier = 13f;
    float fire_pressed_once = 0;
    float time_left_menu = 0;
    bool horizontal_down = false;
    bool vertical_down = false;
    bool mousemode = false;
    private Collider2D[] colliders = new Collider2D[16];

    player_movement other_player;
    CreditCloud[] credit_clouds;
    Vector3 last_build_pos;
    float last_build_pos_time = 0;

    float x_axis = 0;
    float y_axis = 0;

    private void Awake() {
        setPlayerByPlayerId(player_id);

        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;

        grid_manager = GameObject.FindGameObjectWithTag("GridMaster").GetComponent<GridManager>();
        game_master = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<Gamemaster>();
        cursor = (GameObject)Instantiate(cursor_prefab);
        cursorscript = cursor.GetComponent<cursorScript>();

        check_for_leaves();

        animator = GetComponent<Animator>();

        if (gameObject.name == "catty" && is_hedgar == true) {
            eyes_spriterenderer = eyes.GetComponent<SpriteRenderer>();
            eyes_spriterenderer.enabled = false;
        }
        else if (gameObject.name == "catty" || gameObject.name == "batty") {
            animator = eyes.GetComponent<Animator>();
            eyes_spriterenderer = eyes.GetComponent<SpriteRenderer>();
        }

        cattybackfront = GetComponent<CattyBackFront>();

        addInputEvents();

        SetOtherPlayer();

        credit_clouds = FindObjectsOfType<CreditCloud>();
    }

    private void SetOtherPlayer() {
        player_movement[] other_players = FindObjectsOfType<player_movement>();
        foreach (player_movement one_other_player in other_players) {
            if (one_other_player != this) {
                other_player = one_other_player;
            }
        }
    }

    private void FixedUpdate() {
        if (animator != null && Random.Range(0, 5f) > 4.95f &&
            ((cattybackfront != null && cattybackfront.facing_front) || (cattybackfront == null))) {

            animator.enabled = true;
            animator.StopPlayback();
            animator.Play("blink", -1, 0f);
        }

        if (cattybackfront != null) {
            if (cattybackfront.facing_front && !is_hedgar) {
                eyes_spriterenderer.transform.localScale = new Vector3(1f, 1f, 1f);
                eyes_spriterenderer.enabled = true;
            }
            else {
                eyes_spriterenderer.transform.localScale = new Vector3(0, 0, 0);
                eyes_spriterenderer.enabled = false;
            }
        }

        LookAtNearestTarget();
    }

    void LookAtNearestTarget() {
        // creditclouds
        if (credit_clouds.Length > 0) {
            foreach (CreditCloud cloud in credit_clouds) {
                float distance = Vector3.Distance(cloud.transform.position, transform.position);
                if (distance < 3f && distance > 1f) {
                    look_at_target_pos = cloud.transform.position;
                    return;
                }
            }
        }

        // other player
        if (other_player != null) {
            float distance = Vector3.Distance(other_player.transform.position, transform.position);
            if (distance < 3f) {
                look_at_target_pos = other_player.transform.position;
                return;
            }
        }

        // spirits
        if (game_master.spiritmaster.transform.childCount > 0) {
            float closest_distance = 20f;
            spirit_ai closest_spirit = game_master.spirits[0];
            bool spirit_found = false;
            foreach (spirit_ai spirit in game_master.spirits) {
                if (spirit == null)
                    continue;

                float distance = Vector3.Distance(spirit.transform.position, transform.position);
                if (distance < closest_distance) {
                    closest_distance = distance;
                    closest_spirit = spirit;
                    spirit_found = true;
                }
            }
            if (spirit_found) {
                look_at_target_pos = closest_spirit.transform.position;
                if (closest_distance < 2f) {
                    if (!closest_spirit.moving_to_beatbox) {
                        closest_spirit.beatbox_boost(1f);
                    }
                }
                return;
            }
        }

        look_at_target_pos = other_player.transform.position;
    }
    public void stopAnimation() {
        if (animator != null) {
            animator.enabled = false;
        }
    }

    void check_for_leaves() {
        leaves = GameObject.FindGameObjectsWithTag("Leaf");
    }

    void OnMoveHorizontal(InputActionEventData data) {
        if (data.GetAxis() == 0) {
            horizontal_down = false;
            x_axis = 0;
            return;
        }

        if (game_master.is_paused) {
            if (data.GetAxis() > -0.5f && data.GetAxis() < 0.5f)
                return;

            if (horizontal_down == true) {
                return;
            }
            horizontal_down = true;

            if (game_master.is_in_options) {
                if (data.GetAxis() < 0) {
                    game_master.optionsscreen.left_item();
                }
                else {
                    game_master.optionsscreen.right_item();
                }
            }

            return;
        }

        mousemode = false;

        x_axis = data.GetAxis() * speed_modifier * Time.deltaTime;
    }

    void OnMoveVertical(InputActionEventData data) {
        if (data.GetAxis() == 0) {
            vertical_down = false;
            y_axis = 0;
            return;
        }

        if (game_master.is_paused) {
            if (data.GetAxis() > -0.4f && data.GetAxis() < 0.4f)
                return;

            if (vertical_down == true) {
                return;
            }
            vertical_down = true;

            if (game_master.is_in_options) {
                if (data.GetAxis() < 0) {
                    game_master.optionsscreen.next_item();
                }
                else {
                    game_master.optionsscreen.prev_item();
                }
            }
            else {
                if (data.GetAxis() < 0) {
                    game_master.pausescreen.next_item();
                }
                else {
                    game_master.pausescreen.prev_item();
                }
            }

            return;
        }

        mousemode = false;

        y_axis = data.GetAxis() * speed_modifier * Time.deltaTime;
    }

    void OnFireUpdate(InputActionEventData data) {
        if (!game_master.game_is_running && !game_master.returning_to_map) {
            game_master.finish_level();
            return;
        }
        if (game_master.is_paused) {
            return;
        }

        if (game_master.returning_to_map) {
            return;
        }

        if (game_master.raining) {
            return;
        }

        // ignore input for the first 1s
        if (Time.timeSinceLevelLoad < 1f) {
            return;
        }

        // ignore input after leaving menu
        if (Time.time - time_left_menu < 0.2f) {
            return;
        }

        // check for credit clouds
        if (credit_clouds.Length > 0) {
            foreach (CreditCloud cloud in credit_clouds) {
                float distance = Vector3.Distance(cloud.transform.position, transform.position);
                if (distance < 2f) {
                    cloud.wobble();
                }
            }
        }

        // if we're not catty, we should ignore leaf piles on cancel
        GameObject leaf = find_leaf_at_coordinates(cursor.transform.position.x, cursor.transform.position.y);
        if (leaf != null) {
            return;
        }

        if (last_build_pos.x == cursor.transform.position.x && last_build_pos.y == cursor.transform.position.y) {
            if (Time.time - last_build_pos_time < 1f) {
                return;
            }
        }

        if (grid_manager.build_tower(cursor.transform.position.x, cursor.transform.position.y)) {
            game_master.music_instructor.soundman.play_sound("kick" + Random.Range(1, 12));
            if (Time.time - fire_pressed_once < 0.1f) {
                last_built_tower_time = Time.time;
                last_beat_time = Time.time;
            }
            game_master.check_symbols_order(cursor.transform.position.x, cursor.transform.position.y);
            tower_poof_at(
                cursor.transform.position.x, cursor.transform.position.y,
                grid_manager.calc_sort_order_for_y_axis(cursor.transform.position.y)
            );
            ShowGridLines(cursor.transform.position.x, cursor.transform.position.y);
            last_build_pos = cursor.transform.position;
            last_build_pos_time = Time.time;
        }
    }

    void tower_poof_at(float x, float y, int sort_order) {
        GameObject left = GetPoof();
        GameObject right = GetPoof();
        GameObject bottom_left = GetPoof();
        GameObject bottom_right = GetPoof();

        left.transform.position = new Vector3(Wiggle(x - 0.5f), y, 0);
        right.transform.position = new Vector3(Wiggle(x + 0.5f), y, 0);
        bottom_left.transform.position = new Vector3(Wiggle(x - 0.5f), Wiggle(y - 0.5f), 0);
        bottom_right.transform.position = new Vector3(Wiggle(x + 0.5f), Wiggle(y - 0.5f), 0);

        left.BroadcastMessage("GoPoof", sort_order - 1);
        right.BroadcastMessage("GoPoof", sort_order - 1);
        bottom_left.BroadcastMessage("GoPoof", sort_order + 1);
        bottom_right.BroadcastMessage("GoPoof", sort_order + 1);
    }

    GameObject GetPoof() {
        if (game_master.poofpool.Count > 0) {
            GameObject existingpoof = game_master.poofpool[0].gameObject;
            game_master.poofpool.RemoveAt(0);
            return existingpoof;
        }

        GameObject newpoof = (GameObject)Instantiate(poof);
        Poof poofscript = newpoof.GetComponent<Poof>();
        poofscript.gm = game_master;
        return newpoof;
    }

    public void ShowGridLines(float x, float y) {
        // north
        GridLine gridline_l = GetGridLine();
        GridLine gridline_r = GetGridLine();
        gridline_l.transform.position = new Vector3(x - 0.5f, y - 0.5f, 0);
        gridline_r.transform.position = new Vector3(x + 0.5f, y - 0.5f, 0);
        gridline_l.transform.eulerAngles = new Vector3(0, 0, 0f);
        gridline_r.transform.eulerAngles = new Vector3(0, 0, 0f);
        gridline_l.MoveIn();
        gridline_r.MoveIn();

        // east
        gridline_l = GetGridLine();
        gridline_r = GetGridLine();
        gridline_l.transform.position = new Vector3(x + 0.5f, y - 0.5f, 0);
        gridline_r.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
        gridline_l.transform.eulerAngles = new Vector3(0, 0, 90f);
        gridline_r.transform.eulerAngles = new Vector3(0, 0, 90f);
        gridline_l.MoveIn();
        gridline_r.MoveIn();

        // west
        gridline_l = GetGridLine();
        gridline_r = GetGridLine();
        gridline_l.transform.position = new Vector3(x - 0.5f, y - 0.5f, 0);
        gridline_r.transform.position = new Vector3(x - 0.5f, y + 0.5f, 0);
        gridline_l.transform.eulerAngles = new Vector3(0, 0, -90f);
        gridline_r.transform.eulerAngles = new Vector3(0, 0, -90f);
        gridline_l.MoveIn();
        gridline_r.MoveIn();

        // south
        gridline_l = GetGridLine();
        gridline_r = GetGridLine();
        gridline_l.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
        gridline_r.transform.position = new Vector3(x - 0.5f, y + 0.5f, 0);
        gridline_l.transform.eulerAngles = new Vector3(0, 0, 180f);
        gridline_r.transform.eulerAngles = new Vector3(0, 0, 180f);
        gridline_l.MoveIn();
        gridline_r.MoveIn();
    }

    GridLine GetGridLine() {
        if (game_master.gridlinepool.Count > 0) {
            GridLine existingline = game_master.gridlinepool[0];
            game_master.gridlinepool.RemoveAt(0);
            return existingline;
        }

        GameObject newlinecinema = (GameObject)Instantiate(grid_line);
        GridLine linescript = newlinecinema.GetComponent<GridLine>();
        linescript.gm = game_master;
        return linescript;
    }

    float Wiggle(float numb) {
        return Random.Range(numb - 0.2f, numb + 0.2f);
    }

    void OnFireUpdateOnce(InputActionEventData data) {
        if (game_master.game_is_running == false) {
            return;
        }

        if (game_master.is_paused) {
            if (game_master.burgundy_active) {
                game_master.HideBurgundyHelp();
                time_left_menu = Time.time;
            }
            else if (game_master.is_in_options) {
                game_master.optionsscreen.call_action_of_active_menu_item();
            }
            else {
                game_master.pausescreen.call_action_of_active_pause_menu_item();
                time_left_menu = Time.time;
            }
            return;
        }

        fire_pressed_once = Time.time;

        SpiritWayWiser waywiser = grid_manager.get_waywiser_at(cursor.transform.position.x, cursor.transform.position.y);
        if (waywiser) {
            game_master.music_instructor.soundman.play_sound("confirm" + Random.Range(5, 7), Random.Range(-2, 2));
            waywiser.change_direction();
        }

        Shromp shromp = grid_manager.get_shromp_at(cursor.transform.position.x, cursor.transform.position.y);
        if (shromp) {
            shromp.jomp();
        }

        if (game_master.spirits_active == false && Time.time - last_build_pos_time > 1f) {
            SpiritSpawnPoint spawny = grid_manager.get_spawnpoint_at(cursor.transform.position.x, cursor.transform.position.y);
            if (spawny) {
                safely_activate_spirits();
            }
        }
    }

    void OnCancelUpdate(InputActionEventData data) {
        if (game_master.is_paused || !game_master.game_is_running) {
            return;
        }

        // ignore input for the first 1s
        if (Time.timeSinceLevelLoad < 1f) {
            return;
        }

        // if we're not catty, we should ignore leaf piles on cancel
        GameObject leaf = find_leaf_at_coordinates(cursor.transform.position.x, cursor.transform.position.y);
        if (leaf != null) {
            return;
        }

        if (grid_manager.delete_tower(cursor.transform.position.x, cursor.transform.position.y)) {
            game_master.music_instructor.soundman.play_sound("slide6", Random.Range(-5, 5));

            game_master.check_symbols_order(cursor.transform.position.x, cursor.transform.position.y);
        }
        else {
            if (Time.time - last_built_tower_time > 0.5f) {
                last_built_tower_time = Time.time;
            }
        }
    }

    void OnCancelUpdateOnce(InputActionEventData data) {
        if (game_master.burgundy_active) {
            game_master.HideBurgundyHelp();
            return;
        }

        if (game_master.is_in_options) {
            game_master.optionsscreen.fireCancel();
            return;
        }

        if (game_master.is_paused) {
            game_master.unpauseGame();
            return;
        }
    }

    void OnFireUpdateOnceLeaves(InputActionEventData data) {
        if (game_master.game_is_running == false) {
            return;
        }

        if (game_master.is_paused) {
            if (game_master.is_in_options) {
                game_master.optionsscreen.call_action_of_active_menu_item();
            }
            else {
                game_master.pausescreen.call_action_of_active_pause_menu_item();
            }
        }
    }

    void OnFireUpdateLeaves(InputActionEventData data) {
        if (!game_master.game_is_running && !game_master.returning_to_map) {
            game_master.finish_level();
            return;
        }
        if (game_master.is_paused) {
            return;
        }

        if (game_master.returning_to_map) {
            return;
        }

        if (game_master.raining) {
            return;
        }

        if (game_master.game_is_running == false) {
            return;
        }

        if (last_build_pos.x == cursor.transform.position.x && last_build_pos.y == cursor.transform.position.y) {
            if (Time.time - last_build_pos_time < 1f) {
                return;
            }
        }

        GameObject leaf = find_leaf_at_coordinates(cursor.transform.position.x, cursor.transform.position.y);

        if (leaf != null) {
            leaf.GetComponent<leaf>().grow_pile();

            if (grid_manager.build_tower(cursor.transform.position.x, cursor.transform.position.y)) {
                game_master.music_instructor.soundman.play_sound("leavev2_" + Random.Range(1, 5), Random.Range(-2, 2));
                SummonFlyingLeaves(cursor.transform.position.x, cursor.transform.position.y);
                last_build_pos = cursor.transform.position;
                last_build_pos_time = Time.time;
            }
            GameObject tower = grid_manager.get_tower(cursor.transform.position.x, cursor.transform.position.y);
            tower.SetActive(false);
        }

        game_master.CheckGateConditions();
    }

    void SummonFlyingLeaves(float x, float y) {
        for (int i = 0; i < Random.Range(1, 3); i++) {
            GameObject newleaf = (GameObject)Instantiate(leafflying);
            newleaf.transform.position = new Vector3(
                x - 0.5f + Random.Range(0, 1f),
                y - 0.5f + Random.Range(0, 1f),
                transform.position.z
            );
        }
    }

    GameObject find_leaf_at_coordinates(float x, float y) {
        foreach (GameObject a_leaf in leaves) {
            if (a_leaf.transform.position.x > cursor.transform.position.x - 0.2f &&
                a_leaf.transform.position.x < cursor.transform.position.x + 0.2f &&
                a_leaf.transform.position.y > cursor.transform.position.y - 0.2f &&
                a_leaf.transform.position.y < cursor.transform.position.y + 0.2f) {

                return a_leaf;
            }
        }

        return null;
    }

    void OnCancelUpdateLeaves(InputActionEventData data) {
        if (game_master.game_is_running == false) {
            return;
        }

        if (game_master.is_in_options) {
            return;
        }

        if (game_master.is_paused) {
            return;
        }

        GameObject leaf = find_leaf_at_coordinates(cursor.transform.position.x, cursor.transform.position.y);

        if (leaf != null) {
            leaf.GetComponent<leaf>().remove_pile();
            
            if(grid_manager.delete_tower(cursor.transform.position.x, cursor.transform.position.y)) {
                game_master.music_instructor.soundman.play_sound("slide6", Random.Range(-5, 5));
            }
        }

        game_master.CheckGateConditions();
    }

    void OnSquareUpdate(InputActionEventData data) {
        // ignore input for the first 1s
        if (Time.timeSinceLevelLoad < 1f) {
            return;
        }

        if (!game_master.raining) {
            safely_activate_spirits();
        }
    }

    public void safely_activate_spirits() {
        if (game_master.credits) {
            return;
        }

        if (game_master.game_is_running == false) {
            return;
        }

        if (game_master.is_paused) {
            return;
        }

        game_master.activate_spirits(gameObject.name);
    }

    void OnTriangleUpdate(InputActionEventData data) {
        if (game_master.game_is_running == false) {
            return;
        }

        if (game_master.is_paused) {
            return;
        }

        if (game_master.spirits_active == false && !game_master.raining) {
            game_master.send_preview_spirit();
        }
    }

    void OnR1Update(InputActionEventData data) {
        if (game_master.is_paused) {
            return;
        }

        game_master.switchPlayerIds();
        game_master.save_player_switched();
    }

    void OnL1Update(InputActionEventData data) {
        if (game_master.game_is_running == false) {
            return;
        }

        if (game_master.is_paused) {
            return;
        }

        cursorscript.toggleStyle();
        game_master.save_cursor_style(gameObject.name, cursorscript.current_style);
    }

    void OnUpUpdate(InputActionEventData data) {
        if (game_master.game_is_running == false) {
            return;
        }

        if (game_master.is_in_options) {
            game_master.optionsscreen.prev_item();
            return;
        }

        if (game_master.is_paused) {
            game_master.pausescreen.prev_item();
            return;
        }

		StartCoroutine(DPad_OneStepY(1f));
    }

    void OnDownUpdate(InputActionEventData data) {
        if (game_master.game_is_running == false) {
            return;
        }

        if (game_master.is_in_options) {
            game_master.optionsscreen.next_item();
            return;
        }

        if (game_master.is_paused) {
            game_master.pausescreen.next_item();
            return;
        }

		StartCoroutine(DPad_OneStepY(-1f));
    }

    private IEnumerator DPad_OneStepY(float direction) {
        float cursor_y = cursor.transform.position.y;
        float next_y = Mathf.Clamp(cursor_y + direction, -6f, 6f);

        y_axis = direction * 2f * speed_modifier * Time.deltaTime;

        while (cursor.transform.position.y != next_y && Mathf.Abs(cursor_y - cursor.transform.position.y) < 1f) {
            yield return new WaitForSeconds(0.01f);
        }
        y_axis = 0;
    }

    void OnLeftUpdate(InputActionEventData data) {
        if (game_master.game_is_running == false) {
            return;
        }

        if (game_master.is_paused) {
            if (game_master.is_in_options) {
                game_master.optionsscreen.left_item();
            }
            return;
        }

		StartCoroutine(DPad_OneStepX(-1f));
    }

    void OnRightUpdate(InputActionEventData data) {
        if (game_master.game_is_running == false) {
            return;
        }

        if (game_master.is_paused) {
            if (game_master.is_in_options) {
                game_master.optionsscreen.right_item();
            }
            return;
        }

        StartCoroutine(DPad_OneStepX(1f));
    }

    private IEnumerator DPad_OneStepX(float direction) {
        float cursor_x = cursor.transform.position.x;
        float next_x = Mathf.Clamp(cursor_x + direction, -11f, 11f);

        x_axis = direction * 2f * speed_modifier * Time.deltaTime;

        while (cursor.transform.position.x != next_x && Mathf.Abs(cursor_x - cursor.transform.position.x) < 1f) {
            yield return new WaitForSeconds(0.01f);
        }
        x_axis = 0;
    }

    void OnStartUpdate(InputActionEventData data) {
        if (game_master.game_is_running == false) {
            return;
        }

        if (game_master.burgundy_active) {
            game_master.HideBurgundyHelp();
            return;
        }

        if (game_master.is_in_options) {
            game_master.optionsscreen.fireCancel();
            return;
        }

        game_master.togglePause();
    }

    public void setPlayerByPlayerId(int new_player_id) {
        player_id = new_player_id;
        player = ReInput.players.GetPlayer(player_id);
        if (new_player_id == 1) {
            player.controllers.hasMouse = true;
        }
        else {
            player.controllers.hasMouse = false;
        }
    }

    public void ShootHeartAt(Transform heart_target) {
        GameObject my_heart = (GameObject)Instantiate(switchheart);
        my_heart.transform.position = transform.position;
        SwitchHeart my_heart_switch = my_heart.GetComponent<SwitchHeart>();
        my_heart_switch.FlyTowards(heart_target);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.controllers.hasMouse && !game_master.is_paused) {
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) {
                mousemode = true;
            }
            if (mousemode && !game_master.is_paused && !game_master.is_in_options) {
                Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
                if (screenRect.Contains(Input.mousePosition)) {
                    var targetPos = cam.ScreenToWorldPoint(Input.mousePosition);
                    targetPos.x = Mathf.Clamp(targetPos.x, -11f, 11f);
                    targetPos.y = Mathf.Clamp(targetPos.y - 1f, -7f, 7f);
                    targetPos.z = transform.position.z;
                    Vector3 oldpos = transform.position;
                    Vector3 newpos = Vector3.MoveTowards(transform.position, targetPos, 20f * Time.deltaTime);
                    x_axis = newpos.x - oldpos.x;
                    y_axis = newpos.y - oldpos.y;
                }
                else {
                    x_axis = 0;
                    y_axis = 0;
                }
            }
        }

        MoveForAxis();

        adjust_cursor();
    }

    void MoveForAxis() {
        Vector3 oldpos = transform.position;
        /*if (x_axis == 0 && y_axis == 0) {
            rb.velocity = Vector3.zero;
        }
        else {
            rb.AddForce(
                new Vector3(
                    x_axis * 500 * Time.deltaTime,
                    y_axis * 500 * Time.deltaTime, 0
                ),
                ForceMode2D.Impulse
            );
        }*/

        transform.Translate(new Vector3(x_axis, 0, 0));

        if (checkForCollision()) {
            transform.position = new Vector3(
                oldpos.x,
                transform.position.y,
                transform.position.z
            );
        }

        transform.Translate(new Vector3(0, y_axis, 0));

        if (checkForCollision()) {
            transform.position = new Vector3(
                transform.position.x,
                oldpos.y,
                transform.position.z
            );
        }

        Vector3 newpos = transform.position;

        if (newpos.y < -6.7f) {
            newpos.y = -6.7f;
        }

        if (newpos.y > 5.4f) {
            newpos.y = 5.4f;
        }

        if (newpos.x < -11f) {
            newpos.x = -11f;
        }

        if (newpos.x > 11f) {
            newpos.x = 11f;
        }

        transform.position = newpos;

        shiftbodies(0 - x_axis * 100f);
    }

    void shiftbodies(float amount) {
        bodysprite.transform.eulerAngles = new Vector3(0, 0, amount);
    }

    bool checkForCollision() {
        Vector3 check_position = transform.position;
        int numColliders = Physics2D.OverlapCircleNonAlloc(check_position, 0.7f, colliders);
        for (int i = 0; i < numColliders; i++) {
            if (colliders[i].name.Contains("heartvine")) {
                return true;
            }
        }
        return false;
    }

    void adjust_cursor() {
        int cursor_x = grid_manager.round(transform.position.x);
        int cursor_y = grid_manager.round(transform.position.y) + 1;

        if (game_master.credits) {
            cursor.transform.position = new Vector3(100f, 100f, 100f);
            return;
        }

        if (cursor_x != cursor.transform.position.x || cursor_y != cursor.transform.position.y) {
            cursor.transform.position = new Vector3(cursor_x, cursor_y, -0.3f);
            cursorscript.playCursorAnimation();
        }
    }

    public void setCursorStyle(int style) {
        if (cursorscript == null) {
            cursorscript = cursor.GetComponent<cursorScript>();
        }
        cursorscript.current_style = style;
        cursorscript.applyStyle();
    }

    public void addInputEvents() {
        player.AddInputEventDelegate(OnMoveHorizontal, UpdateLoopType.Update, InputActionEventType.AxisActiveOrJustInactive, "Move Horizontal");
        player.AddInputEventDelegate(OnMoveVertical, UpdateLoopType.Update, InputActionEventType.AxisActiveOrJustInactive, "Move Vertical");

        player.AddInputEventDelegate(OnSquareUpdate, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "Square");
        player.AddInputEventDelegate(OnTriangleUpdate, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "Triangle");
        player.AddInputEventDelegate(OnR1Update, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "R1");
        player.AddInputEventDelegate(OnL1Update, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "L1");
        player.AddInputEventDelegate(OnStartUpdate, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Start");
        player.AddInputEventDelegate(OnCancelUpdateOnce, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Cancel");

        player.AddInputEventDelegate(OnUpUpdate, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Up");
        player.AddInputEventDelegate(OnDownUpdate, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Down");
        player.AddInputEventDelegate(OnLeftUpdate, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Left");
        player.AddInputEventDelegate(OnRightUpdate, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Right");

        if (leaves.Length > 0 && gameObject.name == "catty") {
            player.AddInputEventDelegate(OnFireUpdateOnceLeaves, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Fire");
            player.AddInputEventDelegate(OnFireUpdateLeaves, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "Fire");
            player.AddInputEventDelegate(OnCancelUpdateLeaves, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "Cancel");
        }
        else {
            player.AddInputEventDelegate(OnFireUpdateOnce, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Fire");
            player.AddInputEventDelegate(OnFireUpdate, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "Fire");
            player.AddInputEventDelegate(OnCancelUpdate, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "Cancel");
        }
    }

    public void destroyInputEvents() {

        horizontal_down = false;
        x_axis = 0;
        vertical_down = false;
        y_axis = 0;
        player.ClearInputEventDelegates();
    }

    void OnDestroy() {
        // Unsubscribe from events when object is destroyed
        destroyInputEvents();
    }
}
