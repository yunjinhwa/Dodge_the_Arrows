using UnityEngine;

public class ButtonClickAction : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartButton()
    {
        // Restart the game via GameDirector if available
        GameDirector director = GameObject.FindWithTag("director").GetComponent<GameDirector>();
        if (director != null)
        {
            director.RestartGame();
            return;
        }

        // Fallback: set state directly
        GameStateManager.Instance.SetGameState(GameState.IsPlaying);
    }
}
