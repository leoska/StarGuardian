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

    public GameController gameController => _gameController;

    #endregion

    #region private values
    private static App _app = null;
    private GameController _gameController;
    #endregion

    // Constructor
    private App()
    {
        _app = this;
    }

    public void Initialize(GameController controller)
    {
        _gameController = controller;
        Debug.Log(_gameController);
    }

    public void EventUpdate()
    {
    }
}
