using UnityEngine;

// Attach this to item GameObjects. It handles movement, lifetime and collision
// detection (distance-based) and delegates item effects to an IItem strategy via ItemContext.
public class ItemController : MonoBehaviour
{
    public enum ItemType { Heal }

    [Header("Movement")]
    [SerializeField] private float fallSpeed = 0.1f;
    [SerializeField] private float destroyY = -5f;

    [Header("Item")]
    [SerializeField] private ItemType itemType = ItemType.Heal;
    [SerializeField] private float healAmount = 0.1f;

    [Header("Detection")]
    [SerializeField] private float itemRadius = 0.5f;
    [SerializeField] private float playerRadius = 1.0f;

    // Runtime strategy/context
    private ItemContext context;
    private GameObject player;

    void Start()
    {
        // Create concrete strategy based on selected type
        IItem strategy = CreateStrategy(itemType);
        context = new ItemContext(strategy);

        // Find player by tag
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        transform.Translate(0, -fallSpeed, 0);
        if (transform.position.y < destroyY)
        {
            Destroy(gameObject);
            return;
        }

        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            if (player == null) return;
        }

        Vector2 p1 = transform.position;
        Vector2 p2 = player.transform.position;
        float d = Vector2.Distance(p1, p2);

        if (d < itemRadius + playerRadius)
        {
            TriggerHitPlayer();
            Destroy(gameObject);
        }
    }

    // Allows changing the strategy at runtime
    public void SetStrategy(IItem item)
    {
        if (context == null) context = new ItemContext(item);
        else context.SetItem(item);
    }

    private IItem CreateStrategy(ItemType type)
    {
        switch (type)
        {
            case ItemType.Heal:
                return new HealItem(healAmount);
            default:
                return new HealItem(healAmount);
        }
    }

    private void TriggerHitPlayer()
    {
        if (context != null) context.Action();
        else Debug.LogWarning("ItemContext is null - no strategy to execute");
    }
}
