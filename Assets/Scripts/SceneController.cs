using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class SceneController : MonoBehaviour {
    public GameObject ball;
    public GameObject square;
    public GameObject rowPref;
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
    private float startPosLeft = -0.44f;
    private float startPosTop = 0.44f;
    //private float width;

    public Gradient grad;
    private GradientColorKey[] gck = new GradientColorKey[7];
    private GradientAlphaKey[] gak = new GradientAlphaKey[7];
    public Color[] colors;

    void Start () {
        colors = new Color[] {Color.blue, new Color(0,1,1),Color.green,
                              new Color(1,1,0),Color.red,new Color(1,0,1),Color.blue};

        SetGradient();
        ui_text = GameObject.Find("Score").GetComponent<Text>();
        Spawner = GameObject.Find("Spawner");
        ballsCurrentCount = ballsMaxCount;
        CreateNewRow();
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
            CreateNewRow();
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
            
            //Debug.Log(mouseWorldPosition);
        }
    }
    void LateUpdate()
    {
        //remowing none list members
        var l = new List<GameObject>(rows);
        foreach(GameObject row in rows)
        {
            if(!row ||row.transform.childCount == 0)
            {
                l.Remove(row);
                Destroy(row);
            }
        }
        rows = l;
    }
    void CreateNewRow()
    {
        var r = Instantiate(rowPref, transform);
        r.name = "row" + rows.Count;
        var r_pos = r.transform.localPosition;
        r_pos.y = startPosTop;
        r.transform.localPosition = r_pos;

        var count = Random.Range(1, maxRowLength -1);
        bool[] cells = new bool[maxRowLength-2];

        while (count > 0)
        {
            var pos = Random.Range(0, maxRowLength-2);
            if (cells[pos])
            {
                continue;
            }
            else
            {
                var sq = Instantiate(square, r.transform);
                sq.transform.localPosition = new Vector2(startPosLeft + 0.11f * pos, 0);
                sq.GetComponent<SquareController>().hits = Random.Range(ballsMaxCount/2, 
                                                                    ballsMaxCount * 2);
                sq.GetComponent<SquareController>().grad = grad;
                cells[pos] = true;
                count--;
            }
        }

        foreach (GameObject row in rows)
        {
            var m = row.transform.localPosition;
            m.y -= 0.11f;
            row.transform.localPosition = m;
        }
        rows.Add(r);

    }

    void SetGradient()
    {
        for (int i = 0; i < gck.Length; i++)
        {
            gck[i].color = colors[i];
            gck[i].time = (float)i / (gck.Length - 1);
        }
        for (int i = 0; i < gak.Length; i++)
        {
            gak[i].alpha = 1;
            gak[i].time = (float)i / (gak.Length - 1);
        }
        grad.SetKeys(gck, gak);
    }

}
