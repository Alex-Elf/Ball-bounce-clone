using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class SceneController : MonoBehaviour {
    public GameObject ball;
    public float fireRate;
    public int ballsMaxCount = 100;
    public int ballsCurrentCount;
    public int bonus = 0;
    public int scores = 0;

    private GameObject Spawner;

    private Text ui_text;
    private Vector2 mouseWorldPosition;
    private Vector2 direction;

    private int loadedBalls;
    private bool firing = false;
    private float timer = 0;
    
    void Start () {
        ui_text = GameObject.Find("Score").GetComponent<Text>();
        Spawner = GameObject.Find("Spawner");
        ballsCurrentCount = ballsMaxCount;
	}

	void FixedUpdate () {
        timer += Time.deltaTime;
        ui_text.text = "Balls: " + ballsCurrentCount + 
                     "\nMax Balls: " + ballsMaxCount +
                     "\nScores: " + scores;
        if (firing && loadedBalls > 0 && timer >= fireRate)
        {
            var b = Instantiate(ball, Spawner.transform.position, new Quaternion(), transform);
            b.GetComponent<Rigidbody2D>().AddForce(direction * 600);
            timer = 0;
            loadedBalls--;
        }
        if(firing && ballsCurrentCount == ballsMaxCount)
        {
            ballsMaxCount += bonus;
            bonus = 0;
            ballsCurrentCount = ballsMaxCount;
            firing = false;
        }
        if (!firing && Input.GetMouseButton(0))
        {
            //RaycastHit2D hit = Physics2D.Raycast(
            //    Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,1000);

            Spawner.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = (mouseWorldPosition - (Vector2)Spawner.transform.position).normalized;
            Spawner.transform.right = direction;
        }
        if (!firing && Input.GetMouseButtonUp(0)) {
            loadedBalls = ballsCurrentCount;
            ballsCurrentCount = 0;
            firing = true;
            Spawner.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            //scores++;
            Debug.Log(mouseWorldPosition);
        }
    }
}
