public enum GameState
{
    IsPlaying,
    GameOver
}

public enum ButtonState
{
    None,
    Left,
    Right
}

public sealed class GameStateManager
{
    private static readonly GameStateManager instance = new GameStateManager();
    public static GameStateManager Instance => instance;

    private GameState currentState = GameState.IsPlaying;
    private ButtonState currentButton = ButtonState.None;

    public GameState CurrentState => currentState;
    public ButtonState CurrentButton => currentButton;

    private GameStateManager() { }

    public void SetGameState(GameState newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;
    }

    public void SetButtonState(ButtonState newButton)
    {
        if (currentButton == newButton)
            return;

        currentButton = newButton;
    }

    public void ResetToPlaying()
    {
        currentState = GameState.IsPlaying;
        currentButton = ButtonState.None;
    }
}