using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour
{
    Camera cam;
    static Camera backgroundCam;

    private Vector2 resolution;
    public Material mat;

/*    private RenderTexture _downscaledRenderTexture;

    private void OnEnable() {
        var camera = GetComponent<Camera>();
        int height = 360;
        int width = Mathf.RoundToInt(camera.aspect * height);
        _downscaledRenderTexture = new RenderTexture(width, height, 16);
        _downscaledRenderTexture.filterMode = FilterMode.Point;
    }

    private void OnDisable() {
        Destroy(_downscaledRenderTexture);
    }
*/

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        /*        Graphics.Blit(source, _downscaledRenderTexture, mat);
                Graphics.Blit(_downscaledRenderTexture, destination, mat);
        */

        Graphics.Blit(source, destination, mat);
    }

    // Start is called before the first frame update
    void Start()
    {
        // obtain camera component so we can modify its viewport
        cam = GetComponent<Camera>();

        resolution = new Vector2(Screen.width, Screen.height);

        adjustRatio();

        if (!backgroundCam) {
            // Make a new camera behind the normal camera which displays black; otherwise the unused space is undefined
            backgroundCam = new GameObject("BackgroundCam", typeof(Camera)).GetComponent<Camera>();
            backgroundCam.depth = int.MinValue;
            backgroundCam.clearFlags = CameraClearFlags.SolidColor;
            backgroundCam.backgroundColor = Color.black;
            backgroundCam.cullingMask = 0;
        }
    }

    private void FixedUpdate() {
        if (resolution.x != Screen.width || resolution.y != Screen.height) {
            adjustRatio();
            resolution.x = Screen.width;
            resolution.y = Screen.height;
        }
    }

    void adjustRatio() {
        // set the desired aspect ratio (the values in this example are
        // hard-coded for 16:9, but you could make them into public
        // variables instead so you can set them at design time)
        float targetaspect = 16.0f / 9.0f;

        // determine the game window's current aspect ratio
        float windowaspect = (float)Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetaspect;

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f) {
            Rect rect = cam.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            cam.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = cam.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            cam.rect = rect;
        }
    }

}
