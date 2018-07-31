using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class SceneController : MonoBehaviour {
    public GameObject ball;
    public GameObject square;
    public GameObject bonusPref;
    public GameObject rowPref;
    public float fireRate;
    public int ballsMaxCount = 100;
    public int ballsCurrentCount;
    public int bonus = 0;
    public int scores = 0;
    public int rounds = 0;
    public bool gameOver;

    private GameObject Spawner;

    private Text ui_text;
    private Vector2 mouseWorldPosition;
    private Vector2 direction;
    private bool insidePannel;

    private int loadedBalls;
    private bool firing = false;
    private float timer = 0;

    private List<GameObject> rows = new List<GameObject>();
    private int maxRowLength = 9;
    private float startPosLeft = -0.44f;
    private float startPosTop = 0.44f;
    //private float width;

    public Gradient grad;
    private GradientColorKey[] gck;
    private GradientAlphaKey[] gak ;
    public Color[] colors;

    private GameController gameController;
    void Start () {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        colors = new Color[] {Color.blue, new Color(0,1,1),Color.green,
                              new Color(1,1,0),Color.red,new Color(1,0,1),Color.blue};

        SetGradient();
        ui_text = GameObject.Find("Score").GetComponent<Text>();
        Spawner = GameObject.Find("Spawner");
        ballsCurrentCount = ballsMaxCount;
        
        CreateNewRow();
        
        gameOver = false;
        Spawner.transform.GetComponentInChildren<SpriteRenderer>().enabled = false;
    }
    
	void FixedUpdate () {
        if(!gameOver)
        {
            Game();
        }
        else
        {
            var s = GameObject.Find("SUMUP");
            if(s)
            {
                var text =s.GetComponent<Text>().text;
                text = "You ended " + rounds + " rounds" +
                             "\nWith scores: " + scores +
                        "\nAnd " + ballsMaxCount + " balls";
                s.GetComponent<Text>().text = text;
            }
        }
    }
    void Game()
    {
        timer += Time.deltaTime;
        ui_text.text = "Balls: " + ballsCurrentCount +
                     "\nMax Balls: " + ballsMaxCount +
                     "\nRounds: " + rounds +
                     "\nScores: " + scores +
                     "\nTime: " + Time.timeScale;
        if (firing && loadedBalls > 0 && timer >= fireRate)//fire!
        {
            var b = Instantiate(ball, Spawner.transform.position, new Quaternion(), transform);
            b.GetComponent<Rigidbody2D>().AddForce(direction * 600);
            timer = 0;
            loadedBalls--;
        }
        if (firing && ballsCurrentCount == ballsMaxCount)//round end
        {
            ballsMaxCount += bonus;
            bonus = 0;
            ballsCurrentCount = ballsMaxCount;
            firing = false;

            var pos = Spawner.transform.localPosition;
            pos.x = Random.Range(-0.4f, 0.4f);
            Spawner.transform.localPosition = pos;

            ClearBonuses();
            if (rows.Count == 7)
            {
                CreateNewRow();
                GameOver();
            }
            else
            {
                CreateNewRow();
                rounds++;
            }
        }
        if (!firing && Input.GetMouseButton(0))//starting firing
        {

            Spawner.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = (mouseWorldPosition - (Vector2)Spawner.transform.position).normalized;
            Spawner.transform.right = direction;
            insidePannel = mouseWorldPosition.x <= transform.localScale.x / 2 &&
                 mouseWorldPosition.x >= -transform.localScale.x / 2 &&
                 mouseWorldPosition.y >= -transform.localScale.y / 2 &&
                 mouseWorldPosition.y <= transform.localScale.y / 2;
            Debug.Log(mouseWorldPosition);
        }
        if (!firing && Input.GetMouseButtonUp(0) && insidePannel)
        {
            loadedBalls = ballsCurrentCount;
            ballsCurrentCount = 0;
            firing = true;
            Spawner.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

        }
    }

    void LateUpdate()
    {
        //removing none list members
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

        var count = Random.Range(1, maxRowLength+1);
        bool[] cells = new bool[maxRowLength];


        while (count > 0)
        {
            var pos = Random.Range(0, cells.Length);
            if (cells[pos])
            {
                continue;
            }
            else
            {
                var val = Random.value;
                if (val <=0.1f) //Bonus chance
                {
                    var b = Instantiate(bonusPref, r.transform);
                    b.transform.localPosition = new Vector2(startPosLeft + 0.11f * pos, 0);
                    b.GetComponent<Bonus>().type = BonusType.maxBallsBonus;
                }
                else
                {
                    var sq = Instantiate(square, r.transform);
                    sq.transform.localPosition = new Vector2(startPosLeft + 0.11f * pos, 0);
                    sq.GetComponent<SquareController>().hits =
                        (int)Random.Range(ballsMaxCount / 2,ballsMaxCount * 1.25f);
                    sq.GetComponent<SquareController>().grad = grad;

                }
                cells[pos] = true;
                count--;
            }
        }

        foreach (GameObject row in rows)//move other rows down
        {
            var m = row.transform.localPosition;
            m.y -= 0.11f;
            row.transform.localPosition = m;
        }
        rows.Add(r);

    }

    void SetGradient()
    {
        gck = new GradientColorKey[7];
        gak = new GradientAlphaKey[7];
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
    void ClearBonuses()
    {
        foreach (GameObject row in rows)
        {
            if (row.transform.childCount != 0)
            {
                for (int i = 0; i < row.transform.childCount; i++)
                {
                    var child = row.transform.GetChild(i);
                    if (child.tag == "Bonus")
                    {
                        if(child.GetComponent<Bonus>().type == BonusType.maxBallsBonus
                            && child.GetComponent<Bonus>().active)
                        {
                            continue;
                        }
                            
                        Destroy(child.gameObject);
                    }
                }
            }
            
        }
    }
    void GameOver()
    {
        gameOver = true;
        gameController.GameOver(ballsMaxCount, rounds, scores);
        
    }
}
