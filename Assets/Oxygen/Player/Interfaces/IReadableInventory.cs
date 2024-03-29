using System;

namespace Oxygen
{
    public interface IReadableInventory
    {
        event Action<string, int> Placed;
        event Action<string, int> Removed;
        
        event Action<string> RemovedAll;

        int CheckExists(string name);
        bool CheckExists(string name, int count);

        IItem[] GetItems();
    }
}