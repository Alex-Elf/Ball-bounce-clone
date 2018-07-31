using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    static GameController Instance;
	// Use this for initialization
	void Awake () {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
	}
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void GameOver(int ballsMaxCount, int rounds, int scores)
    {
        
        SceneManager.LoadSceneAsync("GameOver", LoadSceneMode.Additive);
        
    }
	public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
	// Update is called once per frame
	void FixedUpdate () {
		
	}
}
