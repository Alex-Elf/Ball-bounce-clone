using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class SceneController : MonoBehaviour {
    public GameObject ball;

    private GameObject Spawner;

    private Text ui_text;
    private int scores = 0;
    private Vector2 mouseScreenPosition;
    private Vector2 direction;
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
            //RaycastHit2D hit = Physics2D.Raycast(
            //    Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,1000);
            mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = (mouseScreenPosition - (Vector2)Spawner.transform.position).normalized;
            Spawner.transform.right = direction;
        }
        if (Input.GetMouseButtonUp(0)) { 
            var b = Instantiate(ball, Spawner.transform.position, new Quaternion(), transform);
            b.GetComponent<Rigidbody2D>().AddForce(direction * 600);
            //scores++;
            Debug.Log(mouseScreenPosition);
        }

    }
   
}
