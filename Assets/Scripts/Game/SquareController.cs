using UnityEngine.UI;
using UnityEngine;

public class SquareController : MonoBehaviour {
    public int hits;
    private SceneController scene_controller;
    private Text ui_Text;
    //[HideInInspector]
    public Gradient grad;
    private int grad_max =150;
    void Start () {
        ui_Text = GetComponentInChildren<Text>();
        scene_controller = GameObject.Find("Pannel").GetComponent<SceneController>();
    }

    void FixedUpdate()
    {
        ui_Text.fontSize = (ui_Text.text.Length > 2) ? 50 : 70;
        ui_Text.text = hits.ToString();
        //grad 0 - grad_max, grad_max == 0, 
        if (hits > grad_max)
        {
            GetComponent<SpriteRenderer>().color = grad.Evaluate((float)(hits % 150) / 150);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = grad.Evaluate((float)hits / 150);
        }
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
