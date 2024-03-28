using Oxygen;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Iron
{
    public class IronInventory : Inventory
    {
        [SerializeField] private GameObject _panelGameObject;
        
        [SerializeField] private Text _itemText;
        [FormerlySerializedAs("_itemIconDatabase")] [SerializeField] private SpriteDatabase spriteDatabase;
        
        private SlotWidget _selectedSlotWidget;
        private SlotWidget _draggingSlotWidget;

        private void UpdateDisplayItem(IItem item)
        {
            _itemText.text = item == null ? string.Empty : item.Name;
        }

        private void OnSelectedSlotChanged(IItem value)
        {
            if (_draggingSlotWidget != null)
            {
                return;
            }
            
            UpdateDisplayItem(value);
        }

        private void OnDraggingSlotChanged(IItem value)
        {
            UpdateDisplayItem(value);
        }

        public void SetSelectedSlot(SlotWidget value)
        {
            _selectedSlotWidget = value;
            
            OnSelectedSlotChanged(value);
        }

        public void SetDraggingSlot(SlotWidget value)
        {
            _draggingSlotWidget = value;
            
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

        public SlotWidget GetSelectedSlot()
        {
            return _selectedSlotWidget;
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