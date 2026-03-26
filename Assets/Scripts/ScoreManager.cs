public sealed class ScoreManager
{
    private static readonly ScoreManager instance = new ScoreManager();
    public static ScoreManager Instance => instance;

    private int score;
    public int CurrentScore => score;

    private ScoreManager() { }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void ResetScore()
    {
        score = 0;
    }
}