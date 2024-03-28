using UnityEngine;
using Behaviour = Oxygen.Behaviour;

namespace Iron
{
    public class ItemReceiver : Behaviour
    {
        [SerializeField] private Oxygen.Item[] _items;
    }
}