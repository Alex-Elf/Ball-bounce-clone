using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour {
    bool ff = false;
    public void FastForward()
    {
        ff = !ff;
        Time.timeScale =(ff)? 3: 1;
    }
    public void StartGame()
    {
        GameObject.Find("GameController").SendMessage("StartGame");
    }
    public void Quit()
    {
        Application.Quit();
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
