namespace Oxygen
{
    public interface IWritableInventory : IReadableInventory
    {
        bool Place(string name, int count);
        bool Remove(string name, int count);
        bool Remove(string name);
    }
}