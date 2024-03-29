using Oxygen;

namespace Iron
{
    public interface IIronItemCollector : IItemCollector
    {
        bool Drop(string name, int count);
        bool Interact(string name);
    }
}