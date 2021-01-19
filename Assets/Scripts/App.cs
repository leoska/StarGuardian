using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class App : MonoBehaviour
{
    public static App Instance { get; private set; }
    public uint score = 0;
    public float timer = 0;

    private GameState _state = GameState.Menu; 

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

        timer += 3 * Time.deltaTime;
        if (timer >1)
        {
            score += 1;
            updateHUD();
            timer = 0;
        }


    }

    public void GameOver()
    {
        Time.timeScale = 0f;
    }
    
    public void AddScore (uint incScore)
    {
        score += incScore;
        updateHUD();
    }
    
    public void updateHUD()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            GameObject.Find("Canvas/Text").GetComponent<Text>().text = "Score: " + Convert.ToString(score);
        }
    }

    public enum GameState
    {
        Menu = 0,
        Game = 1
    }

}
