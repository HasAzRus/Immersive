using Oxygen;

namespace Iron
{
    public interface IIronItemCollector : IItemCollector
    {
        bool Drop(string name);
        bool Interact(string name);
    }
}