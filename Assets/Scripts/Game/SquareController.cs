using UnityEngine.UI;
using UnityEngine;

public class SquareController : MonoBehaviour {
    public int hits;
    private SceneController _scene_controller;
    private Text _ui_Text;
    //[HideInInspector]
    public Gradient grad;
    private int grad_max =150;
    void Start () {
        _ui_Text = GetComponentInChildren<Text>();
        _scene_controller = GameObject.Find("Pannel").GetComponent<SceneController>();
        _ui_Text.text = hits.ToString();
        UpdateSquare();
    }
    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ball")
        {
            _ui_Text.fontSize += 10;
            hits--;
            
            UpdateSquare();
            

            _scene_controller.scores++;
            _ui_Text.fontSize -= 10;

        }
    }
    private void UpdateSquare()
    {
        _ui_Text.text = hits.ToString();
        _ui_Text.fontSize = (_ui_Text.text.Length > 2) ? 50 : 70;
        if (hits <= 0) Destroy(gameObject);
        //grad 0 - grad_max, grad_max == 0, 
        if (hits > grad_max)
        {
            GetComponent<SpriteRenderer>().color = grad.Evaluate((float)(hits % 150) / 150);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = grad.Evaluate((float)hits / 150);
        }

    }

}
