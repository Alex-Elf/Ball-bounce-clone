using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BonusType
{
    maxBallsBonus
}

public class Bonus : MonoBehaviour {
    private SceneController scene_controller;
    [SerializeField]
    private List<Sprite> sprites;
    //0,1 - maxBallsBonus

    public bool active = true;
    public BonusType type;
    
    void Start()
    {
        scene_controller = GameObject.Find("Pannel").GetComponent<SceneController>();
        switch (type)
        {
            case BonusType.maxBallsBonus:
                GetComponent<SpriteRenderer>().sprite = sprites[0];
                break;
        }
        
    }
	
	void Update () {
		
	}
    void Hitted()
    {
        switch (type)
        {
            case BonusType.maxBallsBonus:
                MaxBallsBonus();
                break;

        }
        
    }
    void MaxBallsBonus()
    {
        active = false;
        GetComponent<SpriteRenderer>().sprite = sprites[1];
        GetComponent<Collider2D>().enabled = false;
        scene_controller.bonus += (int)Mathf.Max(scene_controller.ballsMaxCount * 0.02f, 1);
    }
}
