using System;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("GameState")] 
    public GameState startGameState = GameState.Menu;

    [Header("Game Score")]
    public uint score = 0;

    [Header("Scene States")] 
    public GameObject mainMenu;
    public GameObject playingGame;

    [Header("Player")] 
    public GameObject player;

    [Header("HUD buttons")] 
    public GameObject rocketButton;
    public GameObject dodgeButton;
    public GameObject shieldButton;
    public GameObject restartButton;
    public GameObject joystick;

    private GameState _state = GameState.Menu;

    public GameState state => _state;
    
    private float timer = 0f;

    // Start is called before the first frame update
    private void Start()
    {
        App.Instance.Initialize(this);
        SwitchGameState(startGameState);
        timer = 0f;
    }

    // Update is called once per frame
    private void Update()
    {
        App.Instance.EventUpdate();

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
                updateHUD();
                timer = 0;
            }
        }
    }

    public void SwitchGameState(GameState newState)
    {
        switch (newState)
        {
            case GameState.Menu:
                mainMenu.SetActive(true);
                playingGame.SetActive(false);
                
                // Убираем кнопку рестарта
                restartButton.SetActive(false);
                
                // Other HUD Buttons
                rocketButton.SetActive(false);
                dodgeButton.SetActive(false);
                shieldButton.SetActive(false);
                joystick.SetActive(false);
                break;
            
            case GameState.Game:
                mainMenu.SetActive(false);
                playingGame.SetActive(true);
                
                // Убираем кнопку рестарта
                restartButton.SetActive(false);
                
                // Other HUD Buttons
                rocketButton.SetActive(true);
                dodgeButton.SetActive(true);
                shieldButton.SetActive(true);
                joystick.SetActive(true);
                break;
            
            case GameState.GameOver:
                // Показываем кнопку рестарта
                restartButton.SetActive(true);
                
                // Other HUD Buttons
                rocketButton.SetActive(false);
                dodgeButton.SetActive(false);
                shieldButton.SetActive(false);
                joystick.SetActive(false);
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
        updateHUD();
    }
    
    public void updateHUD()
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
