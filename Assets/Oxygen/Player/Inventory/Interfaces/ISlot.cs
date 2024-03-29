namespace Oxygen
{
    public interface ISlot : IItem
    {
        bool IsAssigned { get; }
        bool IsLocked { get; }
    }
}