using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
    bool ff = false;
    public void FastForward()
    {
        ff = !ff;
        Time.timeScale =(ff)? 3: 1;
    }

    public void ReturnBalls()
    {
        var balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach(GameObject ball in balls)
        {
            ball.SendMessage("ReturnBack");
        }
    }
}
