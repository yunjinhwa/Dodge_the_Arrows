public class ItemContext
{
    IItem item;

    public ItemContext() { }

    public ItemContext(IItem item)
    {
        this.item = item;
    }

    public void SetItem(IItem item)
    {
        this.item = item;
    }

    public void Action()
    {
        item?.hitPlayer();
    }
}