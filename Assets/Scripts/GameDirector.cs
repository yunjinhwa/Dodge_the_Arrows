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
    [SerializeField] private float scoreIncreaseRate = 10f; // 점수 증가 속도 (초당) -> 변경: scoreIncreaseRate 를 "몇 초마다 1점"으로 사용
    [SerializeField] private float scorePauseDuration = 0.5f; // 점수 타이머 일시정지 시간 (초)
    [SerializeField] private float scorePauseRemaining = 0f; // 점수 타이머 일시정지 잔여시간 (초)
    private float scoreTimer = 0f; // 점수 증가 타이머

    bool isGameOver = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(this.hpGauge == null) Debug.LogError("hpGauge is not assigned in the inspector.");
        if(this.hpText == null) Debug.LogError("hpText is not assigned in the inspector.");
        if(this.gameOverText == null) Debug.LogError("gameOverText is not assigned in the inspector.");
        if(this.start == null) Debug.LogError("start Button is not assigned in the inspector.");
        if(this.scoreText == null) Debug.LogError("scoreText is not assigned in the inspector.");

        if (GameStateManager.Instance.CurrentState == GameState.IsPlaying)
        {
            if (this.gameOverText != null) this.gameOverText.gameObject.SetActive(false);
            if (this.start != null) this.start.gameObject.SetActive(false);
        }

        ItemContext heal = new ItemContext();
        heal.SetItem(new HealItem());
    }

    public void DecreaseHp()
    {
        if (isGameOver) return;

        this.hpGauge.fillAmount = Mathf.Clamp01(this.hpGauge.fillAmount - 0.1f);
        if (this.hpGauge.fillAmount <= 0 && !isGameOver)
        {
            GameStateManager.Instance.SetGameState(GameState.GameOver);
            OnGameOver();
        }
    }

    public void IncreaseHp(float heal)
    {
        if (isGameOver) return;
        this.hpGauge.fillAmount = Mathf.Clamp01(this.hpGauge.fillAmount + heal);
    }

    // 점수 타이머를 지정된 초만큼 일시정지합니다.
    public void PauseScore()
    {
        if (scorePauseDuration <= 0f) return;
        // 새로 들어온 일시정지 시간이 현재 남은 시간보다 길면 덮어쓰기
        scorePauseRemaining = Mathf.Max(scorePauseRemaining, scorePauseDuration);
    }

    void OnGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // UI 표시
        if (this.gameOverText != null) this.gameOverText.gameObject.SetActive(true);
        if (this.start != null) this.start.gameObject.SetActive(true);

        // 스폰러 비활성화 (태그 'system' 사용)
        GameObject systemObj = null;
        try { systemObj = GameObject.FindWithTag("system"); } catch { systemObj = null; }
        if (systemObj != null)
        {
            ArrowGenerator gen = systemObj.GetComponent<ArrowGenerator>();
            if (gen != null) gen.enabled = false;
        }

        // 모든 화살 비활성화 (태그 'arrow' 사용)
        GameObject[] arrowObjs = GameObject.FindGameObjectsWithTag("arrow");
        foreach (var go in arrowObjs)
        {
            if (go == null) continue;
            ArrowController ac = go.GetComponent<ArrowController>();
            if (ac != null) ac.enabled = false;
        }

        // 플레이어 입력/이동 중지 (태그 'Player' 사용)
        GameObject playerObj = null;
        try { playerObj = GameObject.FindWithTag("Player"); } catch { playerObj = null; }
        if (playerObj != null)
        {
            PlayerController pc = playerObj.GetComponent<PlayerController>();
            if (pc != null) pc.enabled = false;
        }

        // 게임 정지
        Time.timeScale = 0f;

        Debug.Log("Game Over (tag-based)");
    }

    public void RestartGame()
    {
        // 시간 스케일 복원
        Time.timeScale = 1f;
        scoreTimer = 0f;
        scorePauseRemaining = 0f;
        ScoreManager.Instance.ResetScore();

        // HP 초기화
        if (this.hpGauge != null) this.hpGauge.fillAmount = 1f;

        // UI 초기화
        if (this.gameOverText != null) this.gameOverText.gameObject.SetActive(false);
        if (this.start != null) this.start.gameObject.SetActive(false);

        // 모든 화살 제거 (태그 'arrow' 사용)
        GameObject[] arrowObjs = GameObject.FindGameObjectsWithTag("arrow");
        foreach (var go in arrowObjs)
        {
            if (go != null) Destroy(go);
        }

        // 스폰러 재활성화
        GameObject systemObj = null;
        try { systemObj = GameObject.FindWithTag("system"); } catch { systemObj = null; }
        if (systemObj != null)
        {
            ArrowGenerator gen = systemObj.GetComponent<ArrowGenerator>();
            if (gen != null) gen.enabled = true;
        }

        // 플레이어 재활성화
        GameObject playerObj = null;
        try { playerObj = GameObject.FindWithTag("Player"); } catch { playerObj = null; }
        if (playerObj != null)
        {
            PlayerController pc = playerObj.GetComponent<PlayerController>();
            if (pc != null) pc.enabled = true;

            // 플레이어 위치 초기화 가능 (선택)
            playerObj.transform.position = new Vector3(0, playerObj.transform.position.y, playerObj.transform.position.z);
        }

        isGameOver = false;

        GameStateManager.Instance.SetGameState(GameState.IsPlaying);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameStateManager.Instance.CurrentState == GameState.IsPlaying)
        {
            if (this.hpText != null && this.hpGauge != null)
            {
                this.hpText.text = Mathf.RoundToInt(this.hpGauge.fillAmount * 100) + "%";

                if (this.scoreText != null)
                    this.scoreText.text = "Score: " + ScoreManager.Instance.CurrentScore;

                // scoreIncreaseRate 를 "몇 초마다 1점"으로 해석해서 처리
                if (scoreIncreaseRate > 0f)
                {
                    // 점수 타이머가 일시정지 상태면 카운트다운만 수행
                    if (scorePauseRemaining > 0f)
                    {
                        scorePauseRemaining -= Time.deltaTime;
                        if (scorePauseRemaining < 0f) scorePauseRemaining = 0f;
                    }
                    else
                    {
                        scoreTimer += Time.deltaTime;
                        // 한 프레임에 여러 점수를 줘야 할 경우를 처리
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
}
