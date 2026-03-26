using UnityEngine;

public abstract class FallingObjectBase : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float fallSpeed = 6f;
    [SerializeField] private float destroyY = -5f;

    [Header("Collision")]
    [SerializeField] private float objectRadius = 0.5f;
    [SerializeField] private float playerRadius = 1.0f;

    protected Transform PlayerTransform { get; private set; }
    protected GameDirector Director => GameDirector.Instance;

    protected virtual void Awake()
    {
        CachePlayer();
    }

    protected virtual void Update()
    {
        MoveDown();

        if (transform.position.y < destroyY)
        {
            Destroy(gameObject);
            return;
        }

        if (!CachePlayer())
            return;

        float distance = Vector2.Distance(transform.position, PlayerTransform.position);
        if (distance <= objectRadius + playerRadius)
        {
            OnHitPlayer();
            Destroy(gameObject);
        }
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);
    }

    private bool CachePlayer()
    {
        if (PlayerTransform != null)
            return true;

        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player == null)
            return false;

        PlayerTransform = player.transform;
        return true;
    }

    protected abstract void OnHitPlayer();
}