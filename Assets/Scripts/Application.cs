using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Application : MonoBehaviour
{
    static public uint score = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static public void GameOver()
    {
        Time.timeScale = 0f;
    }
    
    static public void AddScore (uint incScore)
    {
        score += incScore;
        updateHUD();
    }
    
    static public void updateHUD()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            GameObject.Find("Canvas/Text").GetComponent<Text>().text = "Score: " + Convert.ToString(score);
        }
    }
}
