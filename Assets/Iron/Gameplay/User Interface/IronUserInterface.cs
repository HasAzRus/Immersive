using Oxygen;
using UnityEngine;

namespace Iron
{
    public class IronUserInterface : UserInterface
    {
        [SerializeField] private IronInventory _inventory;

        private void OnInventoryClosed()
        {
            _inventory.HidePanel();
        }

        private void OnInventoryOpened()
        {
            _inventory.ShowPanel();
        }
        
        protected override void OnGamePlayerConnected(Player player)
        {
            base.OnGamePlayerConnected(player);

            if (player is not IronPlayer ironPlayer)
            {
                return;
            }
            
            ironPlayer.InventoryOpened += OnInventoryOpened;
            ironPlayer.InventoryClosed += OnInventoryClosed;

            var inventory = ironPlayer.GetInventory();
            
            _inventory.Construct(inventory);
        }

        protected override void OnGamePlayerDisconnected(Player player)
        {
            base.OnGamePlayerDisconnected(player);
            
            if (player is not IronPlayer ironPlayer)
            {
                return;
            }
            
            ironPlayer.InventoryOpened += OnInventoryOpened;
            ironPlayer.InventoryClosed += OnInventoryClosed;
            
            _inventory.Clear();
        }

        protected override void Start()
        {
            base.Start();
            
            _inventory.HidePanel();
        }
    }
}