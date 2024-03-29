namespace Oxygen
{
    public interface IItemCollector
    {
        public IReadableInventory Inventory { get; }

        bool GiveItem(string name, int count);
        
        bool RemoveItem(string name);
        bool RemoveItem(string name, int count);

        int CheckExists(string name);
        bool CheckExists(string name, int count);
    }
}