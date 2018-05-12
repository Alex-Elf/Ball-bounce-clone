using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SquareController : MonoBehaviour {
    public int hits;
    private SceneController scene_controller;
    private Text ui_Text;
	void Start () {
        ui_Text = GetComponentInChildren<Text>();
        scene_controller = GameObject.Find("Pannel").GetComponent<SceneController>();
	}

    void FixedUpdate()
    {
        ui_Text.fontSize = (ui_Text.text.Length > 2) ? 50 : 70;
        ui_Text.text = hits.ToString();
        if (hits <= 0) Destroy(gameObject);
    }
        
    void Hit()
    {
        ui_Text.fontSize += 10;
        hits--;
        scene_controller.scores++;
        ui_Text.fontSize -= 10;
    }
}
