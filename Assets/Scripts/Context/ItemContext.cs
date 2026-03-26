public class ItemContext
{
    private IItem item;

    public ItemContext() { }

    public ItemContext(IItem item)
    {
        this.item = item;
    }

    public void SetItem(IItem item)
    {
        this.item = item;
    }

    public void Use(GameDirector director)
    {
        item?.Apply(director);
    }
}