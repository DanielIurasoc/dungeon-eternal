using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText
{
    // Text info
    public bool active;
    public GameObject go;
    public Text txt;
    public Vector3 motion;
    public float duration;
    public float lastShown;

    public void show() {
        active = true;
        lastShown = Time.time;
        go.SetActive(active);
    }

    public void hide() {
        active = false;
        go.SetActive(active);
    }

    public void updateFloatingText() {
        if (!active)
            return;

        if (Time.time - lastShown > duration)
            hide();

        go.transform.position += motion * Time.deltaTime;
    }
}
