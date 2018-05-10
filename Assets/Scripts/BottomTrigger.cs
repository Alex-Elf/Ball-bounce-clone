using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomTrigger : MonoBehaviour {
    private SceneController scController;
	// Use this for initialization
	void Start () {
        scController = GameObject.Find("Pannel").GetComponent<SceneController>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}

    void CountBall()
    {
        scController.ballsCurrentCount += 1;
    }
}
