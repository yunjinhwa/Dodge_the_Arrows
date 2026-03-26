using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameDirector : MonoBehaviour
{
    [SerializeField] private Image hpGauge;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Button start;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Game_difficulty")]
    [SerializeField] private int addScore = 1;
    [SerializeField] private float scoreIncreaseRate = 10f;   // 몇 초마다 1점
    [SerializeField] private float scorePauseDuration = 0.5f; // 피격/회복 시 점수 일시정지 시간
    [SerializeField] private float scorePauseRemaining = 0f;  // 남은 정지 시간

    private float scoreTimer = 0f;
    private bool isGameOver = false;

    void Start()
    {
        if (hpGauge == null) Debug.LogError("hpGauge is not assigned in the inspector.");
        if (hpText == null) Debug.LogError("hpText is not assigned in the inspector.");
        if (gameOverText == null) Debug.LogError("gameOverText is not assigned in the inspector.");
        if (start == null) Debug.LogError("start Button is not assigned in the inspector.");
        if (scoreText == null) Debug.LogError("scoreText is not assigned in the inspector.");

        if (GameStateManager.Instance.CurrentState == GameState.IsPlaying)
        {
            if (gameOverText != null) gameOverText.gameObject.SetActive(false);
            if (start != null) start.gameObject.SetActive(false);
        }

        // 시작 시 UI/상태 초기화
        if (hpGauge != null) hpGauge.fillAmount = 1f;
        isGameOver = false;
        scoreTimer = 0f;
        scorePauseRemaining = 0f;
    }

    public void DecreaseHp()
    {
        if (isGameOver) return;
        if (hpGauge == null) return;

        hpGauge.fillAmount = Mathf.Clamp01(hpGauge.fillAmount - 0.1f);

        if (hpGauge.fillAmount <= 0f && !isGameOver)
        {
            GameStateManager.Instance.SetGameState(GameState.GameOver);
            OnGameOver();
        }
    }

    public void IncreaseHp(float heal)
    {
        if (isGameOver) return;
        if (hpGauge == null) return;

        hpGauge.fillAmount = Mathf.Clamp01(hpGauge.fillAmount + heal);
    }

    public void PauseScore()
    {
        if (scorePauseDuration <= 0f) return;
        scorePauseRemaining = Mathf.Max(scorePauseRemaining, scorePauseDuration);
    }

    void OnGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // UI 표시
        if (gameOverText != null) gameOverText.gameObject.SetActive(true);
        if (start != null) start.gameObject.SetActive(true);

        // system 오브젝트의 생성기들 비활성화
        GameObject systemObj = null;
        try { systemObj = GameObject.FindWithTag("system"); }
        catch { systemObj = null; }

        if (systemObj != null)
        {
            ArrowGenerator arrowGen = systemObj.GetComponent<ArrowGenerator>();
            if (arrowGen != null) arrowGen.enabled = false;

            ItemGenerator itemGen = systemObj.GetComponent<ItemGenerator>();
            if (itemGen != null) itemGen.enabled = false;
        }

        // 모든 화살 비활성화
        GameObject[] arrowObjs = GameObject.FindGameObjectsWithTag("arrow");
        foreach (var go in arrowObjs)
        {
            if (go == null) continue;

            ArrowController ac = go.GetComponent<ArrowController>();
            if (ac != null) ac.enabled = false;
        }

        // 모든 아이템 비활성화
        // 힐 아이템 프리팹 태그를 "item" 으로 설정해둬야 함
        GameObject[] itemObjs = GameObject.FindGameObjectsWithTag("Item");
        foreach (var go in itemObjs)
        {
            if (go == null) continue;

            ItemController ic = go.GetComponent<ItemController>();
            if (ic != null) ic.enabled = false;
        }

        // 플레이어 입력/이동 중지
        GameObject playerObj = null;
        try { playerObj = GameObject.FindWithTag("Player"); }
        catch { playerObj = null; }

        if (playerObj != null)
        {
            PlayerController pc = playerObj.GetComponent<PlayerController>();
            if (pc != null) pc.enabled = false;
        }

        // 게임 정지
        Time.timeScale = 0f;

        Debug.Log("Game Over");
    }

    public void RestartGame()
    {
        // 시간 복원
        Time.timeScale = 1f;

        // 점수 초기화
        scoreTimer = 0f;
        scorePauseRemaining = 0f;
        ScoreManager.Instance.ResetScore();

        // HP 초기화
        if (hpGauge != null) hpGauge.fillAmount = 1f;

        // UI 초기화
        if (gameOverText != null) gameOverText.gameObject.SetActive(false);
        if (start != null) start.gameObject.SetActive(false);

        // 모든 화살 제거
        GameObject[] arrowObjs = GameObject.FindGameObjectsWithTag("arrow");
        foreach (var go in arrowObjs)
        {
            if (go != null) Destroy(go);
        }

        // 모든 아이템 제거
        // 힐 아이템 프리팹 태그를 "item" 으로 설정해둬야 함
        GameObject[] itemObjs = GameObject.FindGameObjectsWithTag("Item");
        foreach (var go in itemObjs)
        {
            if (go != null) Destroy(go);
        }

        // 생성기 재활성화
        GameObject systemObj = null;
        try { systemObj = GameObject.FindWithTag("system"); }
        catch { systemObj = null; }

        if (systemObj != null)
        {
            ArrowGenerator arrowGen = systemObj.GetComponent<ArrowGenerator>();
            if (arrowGen != null) arrowGen.enabled = true;

            ItemGenerator itemGen = systemObj.GetComponent<ItemGenerator>();
            if (itemGen != null) itemGen.enabled = true;
        }

        // 플레이어 재활성화 + 위치 초기화
        GameObject playerObj = null;
        try { playerObj = GameObject.FindWithTag("Player"); }
        catch { playerObj = null; }

        if (playerObj != null)
        {
            PlayerController pc = playerObj.GetComponent<PlayerController>();
            if (pc != null) pc.enabled = true;

            playerObj.transform.position = new Vector3(
                0,
                playerObj.transform.position.y,
                playerObj.transform.position.z
            );
        }

        isGameOver = false;
        GameStateManager.Instance.SetGameState(GameState.IsPlaying);
    }

    void Update()
    {
        if (GameStateManager.Instance.CurrentState == GameState.IsPlaying)
        {
            if (hpText != null && hpGauge != null)
            {
                hpText.text = Mathf.RoundToInt(hpGauge.fillAmount * 100) + "%";
            }

            if (scoreText != null)
            {
                scoreText.text = "Score: " + ScoreManager.Instance.CurrentScore;
            }

            if (scoreIncreaseRate > 0f)
            {
                if (scorePauseRemaining > 0f)
                {
                    scorePauseRemaining -= Time.deltaTime;
                    if (scorePauseRemaining < 0f) scorePauseRemaining = 0f;
                }
                else
                {
                    scoreTimer += Time.deltaTime;

                    while (scoreTimer >= scoreIncreaseRate)
                    {
                        ScoreManager.Instance.AddScore(addScore);
                        scoreTimer -= scoreIncreaseRate;
                    }
                }
            }
        }
    }
}