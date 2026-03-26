using UnityEngine;

public abstract class TimedSpawner : MonoBehaviour
{
    [Header("Spawn Time")]
    [SerializeField] private float minSpan = 1f;
    [SerializeField] private float maxSpan = 2f;

    [Header("Spawn Position")]
    [SerializeField] private int minX = -6;
    [SerializeField] private int maxX = 6;
    [SerializeField] private float spawnY = 7f;

    private float timer;
    private float nextSpan;

    protected abstract GameObject Prefab { get; }

    protected virtual void Start()
    {
        ScheduleNextSpawn();
    }

    protected virtual void Update()
    {
        if (GameStateManager.Instance.CurrentState != GameState.IsPlaying)
            return;

        timer += Time.deltaTime;

        if (timer < nextSpan)
            return;

        timer = 0f;
        Spawn();
        ScheduleNextSpawn();
    }

    public void ResetSpawner()
    {
        timer = 0f;
        ScheduleNextSpawn();
    }

    private void Spawn()
    {
        if (Prefab == null)
        {
            Debug.LogWarning($"{GetType().Name}: prefab is not assigned.");
            return;
        }

        GameObject spawned = Instantiate(Prefab);

        int left = Mathf.Min(minX, maxX);
        int right = Mathf.Max(minX, maxX);
        int randomX = Random.Range(left, right + 1);

        spawned.transform.position = new Vector3(randomX, spawnY, 0f);
    }

    private void ScheduleNextSpawn()
    {
        float a = Mathf.Min(minSpan, maxSpan);
        float b = Mathf.Max(minSpan, maxSpan);
        nextSpan = Random.Range(a, b);
    }
}