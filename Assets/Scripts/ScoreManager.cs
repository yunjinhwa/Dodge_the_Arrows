using UnityEngine;

public sealed class ScoreManager
{
    // 싱글톤 인스턴스
    private static readonly ScoreManager instance = new ScoreManager();

    // 싱글톤 인스턴스에 대한 공개 접근자
    public static ScoreManager Instance => instance;

    private int score = 0;

    public int CurrentScore => score;

    private ScoreManager() { }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
    }

    public void ResetScore()
    {
        score = 0;
        Debug.Log("Score reset to 0.");
    }
}
