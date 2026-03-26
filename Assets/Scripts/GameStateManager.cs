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
    // 싱글톤 인스턴스
    private static readonly GameStateManager instance = new GameStateManager();

    // 싱글톤 인스턴스에 대한 공개 접근자
    public static GameStateManager Instance => instance;

    // 현재 게임 상태를 저장하는 변수(초기값은 Playing)
    private GameState currentState = GameState.IsPlaying;
    private ButtonState currentButton = ButtonState.None;

    // 읽기 전용
    public GameState CurrentState => currentState;
    public ButtonState CurrentButton => currentButton;

    // 생성자를 private으로 설정하여 외부에서 인스턴스를 생성하지 못하도록 함
    private GameStateManager() { }

    public void SetGameState(GameState newState)
    {
        if(currentState == newState)
            return; // 상태가 변경되지 않았으므로 아무 작업도 하지 않음

        currentState = newState;
        // 게임 상태에 따른 추가 로직을 여기에 작성할 수 있습니다.
    }

    public void SetButtonState(ButtonState newButton)
    {
        if(currentButton == newButton) return; // 버튼 상태가 변경되지 않았으므로 아무 작업도 하지 않음

        currentButton = newButton;
        // 버튼 상태에 따른 추가 로직을 여기에 작성할 수 있습니다.
    }
}


