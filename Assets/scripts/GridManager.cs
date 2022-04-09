using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tower_prefab;
    public GameObject grid_field;
    Gamemaster game_master;
    int total_towers;
    public int total_towers_placed;
    public SpiritWayWiser[] waywisers;
    public Shromp[] shromps;
    public SpiritSpawnPoint[] spawnpoints;
    public BlockBoxPlacement[] blockboxplacements;
    public heartbean[] heart_beans;

    private void Awake() {
        game_master = FindObjectOfType<Gamemaster>();
    }

    // Start is called before the first frame update
    void Start()
    {
        waywisers = GameObject.FindObjectsOfType<SpiritWayWiser>();
        shromps = GameObject.FindObjectsOfType<Shromp>();
        spawnpoints = GameObject.FindObjectsOfType<SpiritSpawnPoint>();
        heart_beans = GameObject.FindObjectsOfType<heartbean>();
        blockboxplacements = GameObject.FindObjectsOfType<BlockBoxPlacement>();
    }

    public int round(float number) {
        return Mathf.FloorToInt(number + 0.5f);
    }

    public GameObject get_tower(float x, float y) {

        GameObject grid_field = get_grid_field(x, y);

        return grid_field;
    }

    public bool build_tower(float x, float y) {
        GameObject field = get_grid_field(x, y);

        if (!field) {
            return false;
        }

        if (field.transform.childCount > 0) {
            return false;
        }

        if (total_towers >= game_master.box_limit) {
            return false;
        }

        if (objects_exist_at_coords(x, y)) {
            return false;
        }

        GameObject new_tower = (GameObject)Instantiate(tower_prefab);
        new_tower.transform.parent = field.transform;
        new_tower.transform.localPosition = new Vector3(0, 0, 0);
        set_tower_sorting_layer(new_tower, y);

        total_towers++;
        total_towers_placed++;

        game_master.update_tower_display(total_towers);

        Destroy(new_tower.GetComponent<Animator>(), 0.5f); // never needed again
        if (game_master.wandering_spirit_strangers) { // boxes don’t collide in level 29
            Destroy(new_tower.GetComponentInChildren<Collider2D>());
            Destroy(new_tower.GetComponentInChildren<Rigidbody2D>());
        }

        if (game_master.night) {
            new_tower.GetComponentInChildren<SpriteRenderer>().material = game_master.standard_shader;
        }

        return true;
    }

    public bool objects_exist_at_coords(float x, float y) {

        BlockBoxPlacement blockedplacement = get_block_box_placement(x, y);
        SpiritWayWiser waywiser = get_waywiser_at(x, y);
        heartbean bean = get_heartbean_at(x, y);

        if (waywiser || blockedplacement || bean) {
            return true;
        }

        return false;
    }

    public heartbean get_heartbean_at(float x, float y) {
        x = round(x);
        y = round(y);

        foreach (heartbean bean in heart_beans) {
            if (bean.transform.position.x == x && bean.transform.position.y == y) {
                return bean;
            }
        }

        return null;

    }

    public SpiritWayWiser get_waywiser_at(float x, float y) {
        x = round(x);
        y = round(y);

        foreach (SpiritWayWiser waywiser in waywisers) {
            if (waywiser.transform.position.x == x && waywiser.transform.position.y == y) {
                return waywiser;
            }
        }

        return null;
    }

    public Shromp get_shromp_at(float x, float y) {
        x = round(x);
        y = round(y);

        foreach (Shromp shromp in shromps) {
            if (shromp.transform.position.x == x && shromp.transform.position.y == y) {
                return shromp;
            }
        }

        return null;
    }

    public SpiritSpawnPoint get_spawnpoint_at(float x, float y) {
        x = round(x);
        y = round(y);

        foreach (SpiritSpawnPoint spawny in spawnpoints) {
            if (spawny.transform.position.x == x && spawny.transform.position.y == y) {
                return spawny;
            }
        }

        return null;
    }

    public BlockBoxPlacement get_block_box_placement(float x, float y) {
        x = round(x);
        y = round(y);

        foreach (BlockBoxPlacement blocker in blockboxplacements) {
            if (blocker.transform.position.x == x && blocker.transform.position.y == y) {
                return blocker;
            }
        }

        return null;
    }

    public int calc_sort_order_for_y_axis(float y) {
        int sort_order = 20 - round(y);
        return sort_order;
    }

    void set_tower_sorting_layer(GameObject tower, float y) {
        SpriteRenderer tower_renderer = get_tower_renderer(tower);
        int sort_order = calc_sort_order_for_y_axis(y);
        tower_renderer.sortingOrder = sort_order;
    }

    public SpriteRenderer get_tower_renderer(GameObject tower) {
        return tower.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    GameObject get_grid_field(float x, float y) {

        GridCoordinates coords = new GridCoordinates(x, y);
        x = coords.x();
        y = coords.y();

        GameObject grid_row = get_grid_row(y);
        if (!grid_row) {
            return null;
        }

        GameObject grid_field = get_grid_row_column(grid_row, x);
        if (!grid_field) {
            return null;
        }

        return grid_field;
    }

    GameObject get_grid_row(float y) {
        if (y < 0 || y >= gameObject.transform.childCount) {
            return null;
        }

        GameObject grid_row = gameObject.transform.GetChild(round(y)).gameObject;

        return grid_row;
    }

    GameObject get_grid_row_column(GameObject grid_row, float x) {
        if (x < 0 || x >= grid_row.transform.childCount) {
            return null;
        }

        GameObject grid_field = grid_row.transform.GetChild(round(x)).gameObject;
        return grid_field;
    }

    public bool delete_tower(float x, float y) {
        GameObject field = get_grid_field(x, y);

        if (!field) {
            return false;
        }

        if (field.transform.childCount > 0) {
            Destroy(field.transform.GetChild(0).gameObject);
            total_towers--;
            game_master.update_tower_display(total_towers);
            return true;
        }

        return false;
    }

    public void build_grid() {
        float start_x = -11;
        float start_y = 6;
        float max_x = 11;
        float max_y = -6;

        float grid_x = start_x;
        float grid_y = start_y;

        while (grid_y >= max_y) {

            GameObject new_grid_row = new GameObject("grid row " + grid_y);
            new_grid_row.transform.parent = gameObject.transform;

            while (grid_x <= max_x) {
                GameObject new_field = (GameObject)Instantiate(grid_field);
                new_field.transform.parent = new_grid_row.transform;
                new_field.transform.position = new Vector3(grid_x, grid_y, 0.1f);
                grid_x++;
            }

            grid_x = start_x;
            grid_y--;
        }
    }
}

class GridCoordinates {
    public float given_x;
    public float given_y;

    public GridCoordinates(float x, float y) {
        this.given_x = x;
        this.given_y = y;
    }

    public float x() {
        return given_x + 11;
    }

    public float y() {
        given_y += 6;
        return 12 - given_y;
    }
}