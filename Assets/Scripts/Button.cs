using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour {
    int ff = 1;
    public void FastForward()
    {
        ff = (ff < 16) ? ff * 2 : 1;
        Time.timeScale = ff;
    }
    public void StartGame()
    {
        GameObject.Find("GameController").SendMessage("StartGame");
    }
    public void MainMenu()
    {
        GameObject.Find("GameController").SendMessage("MainMenu");
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
