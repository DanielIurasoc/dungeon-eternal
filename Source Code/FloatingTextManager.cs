using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    public GameObject textContainer;
    public GameObject textPrefab;

    private List<FloatingText> floatingTexts = new List<FloatingText>();

    private FloatingText getFloatingText() {
        // find a FloatingText that is not active
        FloatingText txt = floatingTexts.Find(t => !t.active);

        if (txt == null) {
            txt = new FloatingText();
            txt.go = Instantiate(textPrefab);
            txt.go.transform.SetParent(textContainer.transform);
            txt.txt = txt.go.GetComponent<Text>();

            floatingTexts.Add(txt);
        }

        return txt;
    }

    public void show(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration) {
        FloatingText floatingText = getFloatingText();

        // assign values to variable to customize it
        floatingText.txt.text = msg;
        floatingText.txt.fontSize = fontSize;
        floatingText.txt.color = color;
        // get coordinates to the text grid
        floatingText.go.transform.position = Camera.main.WorldToScreenPoint(position);
        floatingText.motion = motion;
        floatingText.duration = duration;

        floatingText.show();
    }

    private void Update() {
        foreach (FloatingText txt in floatingTexts)
            txt.updateFloatingText();
    }
}
