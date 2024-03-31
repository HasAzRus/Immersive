namespace Oxygen
{
    public class Item : IItem
    {
        public string Name { get; }
        public int Count { get; }

        public Item(string name, int count)
        {
            Name = name;
            Count = count;
        }
    }
}