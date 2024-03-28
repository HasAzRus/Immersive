using Oxygen;
using UnityEngine;

namespace Iron
{
    public class IronUserInterface : UserInterface
    {
        [SerializeField] private IronInventoryWidget inventoryWidget;

        private void OnInventoryClosed()
        {
            inventoryWidget.HidePanel();
        }

        private void OnInventoryOpened()
        {
            inventoryWidget.ShowPanel();
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

            var inventory = ironPlayer.Inventory;
            
            inventoryWidget.Construct(inventory);
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
            
            inventoryWidget.Clear();
        }

        protected override void Start()
        {
            base.Start();
            
            inventoryWidget.HidePanel();
        }
    }
}