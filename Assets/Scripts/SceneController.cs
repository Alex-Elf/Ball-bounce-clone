using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class SceneController : MonoBehaviour {
    public GameObject ball;

    private GameObject Spawner;

    private Text ui_text;
    private int scores = 0;
	// Use this for initialization
	void Start () {
        ui_text = GameObject.Find("Score").GetComponent<Text>();
        Spawner = GameObject.Find("Spawner");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        ui_text.text = "Scores: " + scores;

        if (Input.GetMouseButton(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(
                Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,1000);


            //Instantiate(ball, hit.point, new Quaternion(), transform);
            //scores++;
            Debug.Log(hit.point);
        }

    }
   
}
