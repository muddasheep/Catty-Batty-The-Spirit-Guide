using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class overworld_location : MonoBehaviour
{
    public string spirit_guide_scene;
    UniverseMaster universemaster;
    public bool revealed = false;
    SpriteRenderer spriterenderer;
    public GameObject pawprint;
    bool spawned_pawprints = false;

    public int level_number;
    public string level_name;
    public int coordinate_x;
    public int coordinate_y;

    overworld_cattybatty cattybatty;
    overworld_location next_location;
    overworld_location prev_location;

    public bool level_first_revealed = false;
    public OverworldMap overworldmap;
    public SpriteRenderer checkmark;

    // Start is called before the first frame update
    private IEnumerator Start() {
        setUniverseMaster();
        cattybatty = FindObjectOfType<overworld_cattybatty>();
        SpriteRenderer spriterenderer = gameObject.GetComponent<SpriteRenderer>();
        next_location = getNextLocation();
        prev_location = getPrevLocation();

        level_first_revealed = universemaster.check_level_first_revealed(level_number - 1);
        SpiritGuideLevel level = universemaster.get_level(level_number - 1);
        if (level.total_times_finished > 0) {
            checkmark.enabled = true;
        }

        revealed = universemaster.is_level_revealed(level_number - 1);

        if (revealed) {
            hide_clouds();
        }

        if (revealed && !level_first_revealed) {
            spriterenderer.enabled = true;
        }
        else {
            spriterenderer.enabled = false;
        }

        Animator animator = GetComponent<Animator>();
        animator.enabled = false;

        if (level_first_revealed) {
            yield return new WaitForSeconds(1.5f);
            spriterenderer.enabled = true;
            animator.enabled = true;
            animator.StopPlayback();
            animator.Play("overworld_location_reveal", -1, 0f);
        }
        else {
            animator.enabled = false;
        }

        yield return new WaitForSeconds(0.6f);

        if (!spawned_pawprints && next_location && next_location.revealed) {
            spawned_pawprints = true;

            if (next_location.level_first_revealed) {
                spawnPawPrintsToNextLocation(0.4f);
            }
            else {
                spawnPawPrintsToNextLocation(0f);
            }
        }

        yield return new WaitForSeconds(1f);
    }

    void hide_clouds() {
        overworldmap.removeCloudsAt(transform.position, level_number);
    }

    public void spawnPawPrintsToNextLocation(float delay) {

        if (next_location == null) {
            return;
        }

        GameObject pawprints_container = new GameObject();
        pawprints_container.transform.position = transform.position;
        float distance = Vector3.Distance(transform.position, next_location.transform.position);

        float current_distance = 0.6f;
        distance -= 0.4f;
        int total_prints = 0;
        while (current_distance < distance) {
            GameObject newpawprint = (GameObject)Instantiate(pawprint);
            newpawprint.transform.parent = pawprints_container.transform;

            float x = 0.2f;
            newpawprint.transform.rotation = Quaternion.Euler(0, 0, -30f);
            if (total_prints % 2 == 0) {
                x = -0.2f;
                newpawprint.transform.rotation = Quaternion.Euler(0, 0, 30f);
            }
            newpawprint.transform.localPosition = new Vector3(
                x, current_distance, 0
            );
            newpawprint.GetComponent<PawPrints>().delay = delay > 0 ? (delay + (total_prints / 10f)) : 0;

            current_distance += 0.4f;
            total_prints++;
        }

        // rotate towards target
        Vector2 targetDirection = next_location.transform.position - pawprints_container.transform.position;

        // get angle that we will rotate towards
        float angle = Vector3.SignedAngle(
            targetDirection, pawprints_container.transform.forward, pawprints_container.transform.up
        );

        // rotate now
        pawprints_container.transform.up = targetDirection;
    }

    overworld_location getNextLocation() {
        overworld_location[] locations = FindObjectsOfType<overworld_location>();
        foreach (overworld_location location in locations) {
            if (location.level_number == level_number + 1) {
                return location;
            }
        }

        return null;
    }

    overworld_location getPrevLocation() {
        overworld_location[] locations = FindObjectsOfType<overworld_location>();
        foreach (overworld_location location in locations) {
            if (location.level_number == level_number - 1) {
                return location;
            }
        }

        return null;
    }

    private void OnMouseEnter() {
        if (cattybatty.is_loading) {
            return;
        }
        cattybatty.fly_to_coordinates(coordinate_x, coordinate_y);
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
}
