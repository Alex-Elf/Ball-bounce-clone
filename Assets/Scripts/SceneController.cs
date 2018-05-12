using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class SceneController : MonoBehaviour {
    public GameObject ball;
    public GameObject square;
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

    private List<GameObject> rows = new List<GameObject>();
    private int maxRowLength = 9;
    private float squareWidth = 0.12f;
    private float startPosLeft = -0.44f;
    private float startPosTop = 0.44f;
    //private float width;

    void Start () {
        ui_text = GameObject.Find("Score").GetComponent<Text>();
        Spawner = GameObject.Find("Spawner");
        ballsCurrentCount = ballsMaxCount;
        CreateNewRow();
    }

    void CreateNewRow()
    {

        var r = Instantiate(new GameObject("row" + rows.Count), transform);
        var r_pos = r.transform.localPosition;
        r_pos.y = startPosTop;
        r.transform.localPosition = r_pos;

        var count = Random.Range(1, maxRowLength+1);
        GameObject[] cells = new GameObject[maxRowLength];

        while (count > 0)
        {

            var pos = Random.Range(0, maxRowLength);
            if (cells[pos])
            {
                continue;
            }
            else
            {
                var sq = Instantiate(square, r.transform);
                sq.transform.localPosition = new Vector2(startPosLeft + 0.11f * pos, 0);
                count--;
            }
        }

        foreach (GameObject row in rows){
            var m = row.transform.position;
            m.y += 0.11f;
            row.transform.position = m;
        }
        rows.Add(r);

    }
	void FixedUpdate () {
        //TODO: Fix firing start on buttonClick


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
