using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelLayout : MonoBehaviour
{
    public Sprite[] grass_sprites;
    public GameObject grass_prototype;
    public TextMeshPro levelhint;
    public TextMeshPro console;
    int total_grass = 24;

    public GameObject audiolines;
    public int spectrumi = 20;
    public float[] spectrum_average;

    // Start is called before the first frame update
    void Start()
    {
        distributeGrass();
    }

    private void FixedUpdate() {
        update_bars();
    }

    void update_bars() {
        float[] spectrum = new float[256];

        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

        update_box_lines(spectrum);
    }

    void update_box_lines(float[] spectrum) {
        for (int i = 0; i < audiolines.transform.childCount; i++) {
            Transform boxline = audiolines.transform.GetChild(i);
            float spectrumdata = spectrum[i*spectrumi];
            float pastdata = spectrum_average[i];
            spectrumdata = Mathf.SmoothStep(spectrumdata, pastdata, 0.5f);
            spectrum_average[i] = spectrumdata;

            boxline.localScale = new Vector3(1f, 0.1f + spectrumdata * 5f, 0.02f);
        }
    }

    void distributeGrass() {

        // 22x10
        float grass_grid_start_x = -11f;
        float grass_grid_start_y = -5f;
        float grass_grid_x = grass_grid_start_x;
        float grass_grid_y = grass_grid_start_y;
        float grass_grid_max_x = 11f;
        float grass_grid_max_y = 5f;
        float grass_grid_step_x = 22f / 4f;
        float grass_grid_step_y = 10f / 4f;

        int grass_sprite_counter = 0;
        for (int i = 0; i < total_grass; i++) {
            GameObject newgrass = (GameObject)Instantiate(grass_prototype);

            // x & y should be inside current grass grid x & y
            float current_grass_grid_x = grass_grid_x;
            float current_grass_grid_x_range = grass_grid_x + grass_grid_step_x;
            float current_grass_grid_y = grass_grid_y;
            float current_grass_grid_y_range = grass_grid_y + grass_grid_step_y;

            float x = Random.Range(current_grass_grid_x, current_grass_grid_x_range);
            float y = Random.Range(current_grass_grid_y, current_grass_grid_y_range);

            newgrass.transform.position = new Vector3(
                x, y, 0
            );

            grass_grid_x += grass_grid_step_x;
            if (grass_grid_x >= grass_grid_max_x) {
                grass_grid_x = grass_grid_start_x;

                grass_grid_y += grass_grid_step_y;
                if (grass_grid_y >= grass_grid_max_y) {
                    grass_grid_y = grass_grid_start_y;
                }
            }

            newgrass.GetComponent<SpriteRenderer>().sprite = grass_sprites[grass_sprite_counter];

            grass_sprite_counter++;
            if (grass_sprite_counter >= grass_sprites.Length) {
                grass_sprite_counter = 0;
            }
        }
    }

    public void removeLevelHint() {
        levelhint.text = "";
    }
}
