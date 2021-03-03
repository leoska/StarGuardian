using UnityEngine;

// SingleTon класс всего приложения
public class App
{
    #region public values
    public static App Instance 
    {
        get => _app ?? new App();
        private set => _app = value; 
    }

    public GameController gameController
    {
        get => _gameController;
    }
    #endregion

    #region private values
    private static App _app = null;
    private KeyboardController _keyboardController;
    private GameController _gameController;
    #endregion

    // Constructor
    private App()
    {
        _app = this;
        _keyboardController = new KeyboardController();
    }

    public void Initialize(GameController gameController)
    {
        _gameController = gameController;
    }

    public void EventUpdate()
    {
        _keyboardController.UpdateKeyboard();
    }
}
