using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject healItemPrefab;

    [Header("Spawn Time")]
    [SerializeField] private float minSpan = 3.0f;
    [SerializeField] private float maxSpan = 6.0f;

    [Header("Spawn Position")]
    [SerializeField] private int minX = -6;
    [SerializeField] private int maxX = 6;
    [SerializeField] private float spawnY = 7.0f;

    private float span;
    private float delta = 0f;

    void Start()
    {
        SetNextSpan();
    }

    void Update()
    {
        if (GameStateManager.Instance.CurrentState != GameState.IsPlaying)
            return;

        delta += Time.deltaTime;

        if (delta > span)
        {
            delta = 0f;
            SpawnHealItem();
            SetNextSpan();
        }
    }

    void SpawnHealItem()
    {
        if (healItemPrefab == null)
        {
            Debug.LogWarning("healItemPrefab is not assigned.");
            return;
        }

        GameObject go = Instantiate(healItemPrefab);
        int px = Random.Range(minX, maxX + 1);
        go.transform.position = new Vector3(px, spawnY, 0);

        // 아이템 정리용 태그가 필요하면 여기서 설정 가능
        // go.tag = "item";
    }

    void SetNextSpan()
    {
        float a = Mathf.Min(minSpan, maxSpan);
        float b = Mathf.Max(minSpan, maxSpan);
        span = Random.Range(a, b);
    }
}