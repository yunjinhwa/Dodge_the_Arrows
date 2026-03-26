using UnityEngine;

public class ItemController : FallingObjectBase
{
    public enum ItemType
    {
        Heal
    }

    [Header("Item")]
    [SerializeField] private ItemType itemType = ItemType.Heal;
    [SerializeField] private float healAmount = 0.1f;

    private ItemContext context;

    protected override void Awake()
    {
        base.Awake();
        context = new ItemContext(CreateStrategy(itemType));
    }

    public void SetStrategy(IItem item)
    {
        if (context == null)
            context = new ItemContext(item);
        else
            context.SetItem(item);
    }

    protected override void OnHitPlayer()
    {
        context?.Use(Director);
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
}