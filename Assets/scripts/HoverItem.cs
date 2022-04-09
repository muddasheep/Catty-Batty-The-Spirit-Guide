using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoverItem : MonoBehaviour
{
    float current_x;
    bool show = false;
    List<GameObject> LineContainers;

    private void Start() {
        LineContainers = new List<GameObject>();
    }

    public void Show() {
        transform.localScale = new Vector3(1f, 1f, 1f);
        TextMeshPro[] tmpros = GetComponentsInChildren<TextMeshPro>();
        foreach (TextMeshPro tmpro in tmpros) {

            // create parent obj
            GameObject LineContainer = new GameObject();
            LineContainers.Add(LineContainer);
            //LineContainer.transform.parent = this.transform;

            float x = tmpro.gameObject.transform.position.x;
            float y = 1f;

            string text = tmpro.text.Replace("\\n", "\n");
            string[] lines = text.Split('\n');

            // create copy for each line
            foreach (string line in lines) {
                GameObject tmpro_clone = (GameObject)Instantiate(tmpro.gameObject);
                tmpro_clone.transform.position = new Vector3(x, y, tmpro.transform.position.z);
                tmpro_clone.transform.parent = LineContainer.transform;
                TextMeshPro newtmpro = tmpro_clone.GetComponent<TextMeshPro>();
                newtmpro.richText = true;
                newtmpro.text = line;
                newtmpro.verticalAlignment = VerticalAlignmentOptions.Middle;

                y -= 0.6f;
            }

            tmpro.text = tmpro.text.Replace("\\n", "\n");
            tmpro.enabled = false;
        }

    }

    public void Hide() {
        transform.localScale = new Vector3(1f, 0, 1f);

        TextMeshPro[] tmpros = GetComponentsInChildren<TextMeshPro>();
        foreach (TextMeshPro tmpro in tmpros) {
            tmpro.enabled = true;
        }

        foreach (GameObject LineContainer in LineContainers) {
            Destroy(LineContainer);
        }

        LineContainers = new List<GameObject>();
    }
}
