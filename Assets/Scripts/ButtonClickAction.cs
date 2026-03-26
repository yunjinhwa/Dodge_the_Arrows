using UnityEngine;

public class ButtonClickAction : MonoBehaviour
{
    public void OnStartButton()
    {
        if (GameDirector.Instance != null)
        {
            GameDirector.Instance.RestartGame();
            return;
        }

        GameStateManager.Instance.ResetToPlaying();
    }
}