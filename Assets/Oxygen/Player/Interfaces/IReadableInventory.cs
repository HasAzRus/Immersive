using System;

namespace Oxygen
{
    public interface IReadableInventory
    {
        event Action<string, int> Placed;
        event Action<string, int> Removed;
        
        event Action<string> RemovedAll;

        bool CheckExists(string name);
        bool CheckExists(string name, int count);

        IItem[] GetItems();
    }
}