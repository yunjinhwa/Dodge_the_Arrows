using UnityEngine;

public class ArrowController : MonoBehaviour
{
    GameObject player;
    GameDirector gameDirector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.player = GameObject.Find("Player");
        this.gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, -0.1f, 0);

        if (transform.position.y < -5.0f)
        {
            Destroy(gameObject);
        }

        Vector2 p1 = transform.position;
        Vector2 p2 = player.transform.position;
        Vector2 dir = p1 - p2;
        float d = dir.magnitude;
        float r1 = 0.5f;
        float r2 = 1.0f;

        if (d < r1 + r2)
        {
            // 일시정지: 맞으면 점수 타이머를 0.5초 동안 멈춤
            if (gameDirector != null)
            {
                gameDirector.PauseScore();
                gameDirector.DecreaseHp();
            }
            else
            {
                // 안전하게 데미지 적용
                GameObject gdObj = GameObject.Find("GameDirector");
                if (gdObj != null)
                {
                    GameDirector gd = gdObj.GetComponent<GameDirector>();
                    if (gd != null) gd.PauseScore();
                    if (gd != null) gd.DecreaseHp();
                }
            }

            Destroy(gameObject);
        }
    }
}
