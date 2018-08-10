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

    private GameObject _Spawner;

    private Text _ui_text;
    private Vector2 _mouseWorldPosition;
    private Vector2 _direction;
    private bool _insidePannel;

    private int _loadedBalls;
    private bool _firing = false;
    private float _timer = 0;

    private List<GameObject> _rows = new List<GameObject>();
    private int _maxRowLength = 9;
    private float _startPosLeft = -0.44f;
    private float _startPosTop = 0.44f;
    //private float width;

    public Gradient grad;
    private GradientColorKey[] _gck;
    private GradientAlphaKey[] _gak ;
    public Color[] colors;

    private GameController gameController;
    private void Start () {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        colors = new Color[] {Color.blue, new Color(0,1,1),Color.green,
                              new Color(1,1,0),Color.red,new Color(1,0,1),Color.blue};

        SetGradient();
        _ui_text = GameObject.Find("Score").GetComponent<Text>();
        _Spawner = GameObject.Find("Spawner");
        ballsCurrentCount = ballsMaxCount;
        
        CreateNewRow();
        
        gameOver = false;
        _Spawner.transform.GetComponentInChildren<SpriteRenderer>().enabled = false;
    }
    
	private void FixedUpdate () {
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
    private void Game()
    {
        _timer += Time.deltaTime;
        _ui_text.text = "Balls: " + ballsCurrentCount +
                     "\nMax Balls: " + ballsMaxCount +
                     "\nRounds: " + rounds +
                     "\nScores: " + scores +
                     "\nTime: " + Time.timeScale;
        if (_firing && _loadedBalls > 0 && _timer >= fireRate)//fire!
        {
            var b = Instantiate(ball, _Spawner.transform.position, new Quaternion(), transform);
            b.GetComponent<Rigidbody2D>().AddForce(_direction * 600);
            _timer = 0;
            _loadedBalls--;
        }
        if (_firing && ballsCurrentCount == ballsMaxCount)//round end
        {
            ballsMaxCount += bonus;
            bonus = 0;
            ballsCurrentCount = ballsMaxCount;
            _firing = false;

            var pos = _Spawner.transform.localPosition;
            pos.x = Random.Range(-0.4f, 0.4f);
            _Spawner.transform.localPosition = pos;

            ClearBonuses();
            if (_rows.Count == 7)
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
        if (!_firing && Input.GetMouseButton(0))//starting firing
        {

            _Spawner.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            _mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _direction = (_mouseWorldPosition - (Vector2)_Spawner.transform.position).normalized;
            _Spawner.transform.right = _direction;
            _insidePannel = _mouseWorldPosition.x <= transform.localScale.x / 2 &&
                 _mouseWorldPosition.x >= -transform.localScale.x / 2 &&
                 _mouseWorldPosition.y >= -transform.localScale.y / 2 &&
                 _mouseWorldPosition.y <= transform.localScale.y / 2;
            //Debug.Log(mouseWorldPosition);
        }
        if (!_firing && Input.GetMouseButtonUp(0) && _insidePannel)
        {
            _loadedBalls = ballsCurrentCount;
            ballsCurrentCount = 0;
            _firing = true;
            _Spawner.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

        }
    }

    private void LateUpdate()
    {
        //removing none list members
        var l = new List<GameObject>(_rows);
        foreach(GameObject row in _rows)
        {
            if(!row ||row.transform.childCount == 0)
            {
                l.Remove(row);
                Destroy(row);
            }
        }
        _rows = l;
    }
    private void CreateNewRow()
    {
        var r = Instantiate(rowPref, transform);
        r.name = "row" + _rows.Count;
        var r_pos = r.transform.localPosition;
        r_pos.y = _startPosTop;
        r.transform.localPosition = r_pos;

        var count = Random.Range(1, _maxRowLength+1);
        bool[] cells = new bool[_maxRowLength];


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
                    b.transform.localPosition = new Vector2(_startPosLeft + 0.11f * pos, 0);
                    b.GetComponent<Bonus>().type = BonusType.maxBallsBonus;
                }
                else
                {
                    var sq = Instantiate(square, r.transform);
                    sq.transform.localPosition = new Vector2(_startPosLeft + 0.11f * pos, 0);
                    sq.GetComponent<SquareController>().hits =
                        (int)Random.Range(ballsMaxCount / 2,ballsMaxCount * 1.25f);
                    sq.GetComponent<SquareController>().grad = grad;

                }
                cells[pos] = true;
                count--;
            }
        }

        foreach (GameObject row in _rows)//move other rows down
        {
            var m = row.transform.localPosition;
            m.y -= 0.11f;
            row.transform.localPosition = m;
        }
        _rows.Add(r);

    }

    private void SetGradient()
    {
        _gck = new GradientColorKey[7];
        _gak = new GradientAlphaKey[7];
        for (int i = 0; i < _gck.Length; i++)
        {
            _gck[i].color = colors[i];
            _gck[i].time = (float)i / (_gck.Length - 1);
        }
        for (int i = 0; i < _gak.Length; i++)
        {
            _gak[i].alpha = 1;
            _gak[i].time = (float)i / (_gak.Length - 1);
        }
        grad.SetKeys(_gck, _gak);

    }
    void ClearBonuses()
    {
        foreach (GameObject row in _rows)
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
    private void GameOver()
    {
        gameOver = true;
        gameController.GameOver(ballsMaxCount, rounds, scores);
        
    }
}
