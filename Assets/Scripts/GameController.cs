using System;
using System.Collections.Generic;
using Assets.Scripts.Controllers;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("GameState")] 
    public GameState startGameState = GameState.Menu;
    public bool developmentMode = false;

    [Header("Game Score")]
    public uint score = 0;

    [Header("Scene States")] 
    public GameObject mainMenu;
    public GameObject playingGame;

    [Header("Player")] 
    public GameObject player;

    [Header("HUD buttons")]
    public GameObject restartButton;
    public InputMoveTouch inputMoveTouch;

    [Header("Cameras")] 
    public GameObject mainCamera;
    public GameObject guiCamera;

    private GameState _state = GameState.Menu;

    public GameState state => _state;
    public Dictionary<int, GameObject> enemiesOnScreen = new Dictionary<int, GameObject>();

    private float timer = 0f;

    // Start is called before the first frame update
    private void Start()
    {
        DontDestroyOnLoad(this);
        App.Instance.Initialize(this);
        SwitchGameState(startGameState);
        timer = 0f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            // TODO: Карен попросил возвращаться в главное меню
            if (_state == GameState.Menu)
                Application.Quit();
            else
                ReturnToMainMenu();
        }

        if (_state == GameState.Game)
        {
            timer += 3 * Time.deltaTime;
            if (timer > 1)
            {
                score += 1;
                UpdateHud();
                timer = 0;
            }
        }
    }

    public void SwitchGameState(GameState newState)
    {
        switch (newState)
        {
            case GameState.Menu:
                // Переключаем контроллеры комнаты
                mainMenu.SetActive(true);
                playingGame.SetActive(false);
                
                // Выключаем главную камеру
                // mainCamera.SetActive(false);
                
                // Убираем кнопку рестарта
                restartButton.SetActive(false);
                break;
            
            case GameState.Game:
                mainMenu.SetActive(false);
                playingGame.SetActive(true);
                
                // Включаем главную камеру
                // mainCamera.SetActive(true);
                
                // Убираем кнопку рестарта
                restartButton.SetActive(false);
                break;
            
            case GameState.GameOver:
                // Показываем кнопку рестарта
                restartButton.SetActive(true);
                break;
        }

        _state = newState;
    }
    
    public void ReturnToMainMenu()
    {
        score = 0;
        Time.timeScale = 1f;
        SwitchGameState(GameState.Menu);
    }

    public void RestartGame()
    {
        Application.LoadLevel(Application.loadedLevel);
        ReturnToMainMenu();
    }
    
    public void GameOver()
    {
        SwitchGameState(GameState.GameOver);
        Time.timeScale = 0f;
    }
    
    public void AddScore(uint incScore)
    {
        score += incScore;
        UpdateHud();
    }
    
    public void UpdateHud()
    {
        var canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            GameObject.Find("Canvas/Text").GetComponent<Text>().text = "Score: " + Convert.ToString(score);
        }
    }

    [Serializable]
    public enum GameState
    {
        Menu = 0,
        Game = 1,
        GameOver = 2,
    }
}
