using Oxygen;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Iron
{
    public class IronInventoryUserInterface : InventoryUserInterface
    {
        [SerializeField] private GameObject _panelGameObject;
        
        [SerializeField] private Text _itemText;
        [FormerlySerializedAs("_itemIconDatabase")] [SerializeField] private SpriteDatabase spriteDatabase;
        
        private SlotDrawer _selectedSlotDrawer;
        private SlotDrawer _draggingSlotDrawer;

        private void UpdateDisplayItem(IItem item)
        {
            _itemText.text = item == null ? string.Empty : item.Name;
        }

        private void OnSelectedSlotChanged(IItem value)
        {
            if (_draggingSlotDrawer != null)
            {
                return;
            }
            
            UpdateDisplayItem(value);
        }

        private void OnDraggingSlotChanged(IItem value)
        {
            UpdateDisplayItem(value);
        }

        public void SetSelectedSlot(SlotDrawer value)
        {
            _selectedSlotDrawer = value;
            
            OnSelectedSlotChanged(value);
        }

        public void SetDraggingSlot(SlotDrawer value)
        {
            _draggingSlotDrawer = value;
            
            OnDraggingSlotChanged(value);
        }

        public void RemoveFromPlayer(string name)
        {
            GetInventory().Remove(name, 1);
        }

        public void Drop(string name)
        {
            if(GetInventory() is not IronPlayerInventory ironPlayerInventory)
            {
                return;
            }

            ironPlayerInventory.Drop(name, 1);
        }

        public void Interact(string name)
        {
            if(GetInventory() is not IronPlayerInventory ironPlayerInventory)
            {
                return;
            }

            if (ironPlayerInventory.Interact(name))
            {
                UpdateDisplayItem(null);
            }
        }

        public SlotDrawer GetSelectedSlot()
        {
            return _selectedSlotDrawer;
        }

        public SpriteDatabase GetItemIconDatabase()
        {
            return spriteDatabase;
        }

        public void ShowPanel()
        {
            _panelGameObject.SetActive(true);
        }

        public void HidePanel()
        {
            _panelGameObject.SetActive(false);
        }
    }
}