using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    public static GameDirector Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private Image hpGauge;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Button startButton;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Scene References")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private ArrowGenerator arrowGenerator;
    [SerializeField] private ItemGenerator itemGenerator;

    [Header("Gameplay")]
    [SerializeField] private float maxHp = 1f;
    [SerializeField] private int addScore = 1;
    [SerializeField] private float scoreIncreaseRate = 10f;
    [SerializeField] private float scorePauseDuration = 0.5f;

    private float currentHp;
    private float scoreTimer;
    private float scorePauseRemaining;
    private bool isGameOver;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        ResolveReferences();
    }

    private void Start()
    {
        InitializeGame();
        RefreshUI();
    }

    private void Update()
    {
        if (GameStateManager.Instance.CurrentState != GameState.IsPlaying)
            return;

        UpdateScoreTimer();
        RefreshUI();
    }

    public void DamagePlayer(float damage)
    {
        if (isGameOver)
            return;

        currentHp = Mathf.Clamp(currentHp - damage, 0f, maxHp);

        if (currentHp <= 0f)
        {
            GameOver();
        }
    }

    public void HealPlayer(float amount)
    {
        if (isGameOver)
            return;

        currentHp = Mathf.Clamp(currentHp + amount, 0f, maxHp);
    }

    public void PauseScore()
    {
        scorePauseRemaining = Mathf.Max(scorePauseRemaining, scorePauseDuration);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        ScoreManager.Instance.ResetScore();
        GameStateManager.Instance.ResetToPlaying();

        DestroyAll<ArrowController>();
        DestroyAll<ItemController>();

        if (arrowGenerator != null)
        {
            arrowGenerator.enabled = true;
            arrowGenerator.ResetSpawner();
        }

        if (itemGenerator != null)
        {
            itemGenerator.enabled = true;
            itemGenerator.ResetSpawner();
        }

        if (playerController != null)
        {
            playerController.enabled = true;
            playerController.ResetPosition();
        }

        InitializeGame();
        RefreshUI();
    }

    private void GameOver()
    {
        if (isGameOver)
            return;

        isGameOver = true;
        GameStateManager.Instance.SetGameState(GameState.GameOver);

        if (arrowGenerator != null) arrowGenerator.enabled = false;
        if (itemGenerator != null) itemGenerator.enabled = false;
        if (playerController != null) playerController.enabled = false;

        SetEnabledForAll<ArrowController>(false);
        SetEnabledForAll<ItemController>(false);

        if (gameOverText != null) gameOverText.gameObject.SetActive(true);
        if (startButton != null) startButton.gameObject.SetActive(true);

        Time.timeScale = 0f;
    }

    private void InitializeGame()
    {
        currentHp = maxHp;
        scoreTimer = 0f;
        scorePauseRemaining = 0f;
        isGameOver = false;

        if (gameOverText != null) gameOverText.gameObject.SetActive(false);
        if (startButton != null) startButton.gameObject.SetActive(false);
    }

    private void UpdateScoreTimer()
    {
        if (scorePauseRemaining > 0f)
        {
            scorePauseRemaining -= Time.deltaTime;
            if (scorePauseRemaining < 0f)
                scorePauseRemaining = 0f;

            return;
        }

        if (scoreIncreaseRate <= 0f)
            return;

        scoreTimer += Time.deltaTime;

        while (scoreTimer >= scoreIncreaseRate)
        {
            ScoreManager.Instance.AddScore(addScore);
            scoreTimer -= scoreIncreaseRate;
        }
    }

    private void RefreshUI()
    {
        if (hpGauge != null)
            hpGauge.fillAmount = maxHp <= 0f ? 0f : currentHp / maxHp;

        if (hpText != null)
            hpText.text = Mathf.RoundToInt((currentHp / maxHp) * 100f) + "%";

        if (scoreText != null)
            scoreText.text = "Score: " + ScoreManager.Instance.CurrentScore;
    }

    private void ResolveReferences()
    {
        if (playerController == null)
            playerController = FindAnyObjectByType<PlayerController>();

        if (arrowGenerator == null)
            arrowGenerator = FindAnyObjectByType<ArrowGenerator>();

        if (itemGenerator == null)
            itemGenerator = FindAnyObjectByType<ItemGenerator>();
    }

    private void SetEnabledForAll<T>(bool enabled) where T : Behaviour
    {
        T[] objects = FindObjectsByType<T>(FindObjectsSortMode.None);
        foreach (T obj in objects)
        {
            obj.enabled = enabled;
        }
    }

    private void DestroyAll<T>() where T : Component
    {
        T[] objects = FindObjectsByType<T>(FindObjectsSortMode.None);
        foreach (T obj in objects)
        {
            Destroy(obj.gameObject);
        }
    }
}