using Oxygen;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Iron
{
    public class IronSlotDrawer : SlotDrawer,
        IBeginDragHandler,
        IEndDragHandler,
        IDragHandler,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler
    {
        [SerializeField] private float _selectionScaleAmount;

        [SerializeField] private Transform _baseTransform;
        [SerializeField] private Image _iconImage;

        [SerializeField] private Text _countText;
        
        private bool _isPointing;
        private bool _isSelecting;
        
        private Transform _iconTransform;

        private IronInventoryUserInterface _ironInventory;
        
        protected override void OnConstruct(InventoryUserInterface inventoryUserInterface)
        {
            base.OnConstruct(inventoryUserInterface);
            
            _ironInventory = inventoryUserInterface as IronInventoryUserInterface;
        }

        private void OnSelectionChanged(bool value)
        {
            if (value)
            {
                World.Scale(_baseTransform, _selectionScaleAmount, false);
            }
            else
            {
                World.Scale(_baseTransform, 1f, false);
            }
        }

        private bool TryMoveToSlot()
        {
            var selectedSlot = _ironInventory.GetSelectedSlot();

            if (selectedSlot == null)
            {
                _ironInventory.Drop(Name);
                
                return false;
            }

            if (selectedSlot == this)
            {
                return false;
            }

            if (!selectedSlot.CheckAssigned())
            {
                selectedSlot.Assign(Name);
            }
            else
            {
                if (selectedSlot.Name != Name)
                {
                    return false;
                }
            }

            selectedSlot.Add(Count);
            
            Clear();

            return true;
        }
        
        private void SetSelection(bool value)
        {
            _isSelecting = value;
            
            OnSelectionChanged(value);
        }

        protected override void OnAssigned(string name)
        {
            base.OnAssigned(name);
            
            if (!_ironInventory.GetItemIconDatabase().TryGetValue(name, out var icon))
            {
                return;
            }

            _iconImage.sprite = icon;
            _iconImage.color = Color.white;
        }

        protected override void OnClear()
        {
            base.OnClear();

            _iconImage.sprite = null;
            _iconImage.color = Color.clear;
            
            _countText.text = string.Empty;
        }

        protected override void OnCountChanged(int value)
        {
            base.OnCountChanged(value);

            _countText.text = value.ToString();
        }

        protected override void Start()
        {
            base.Start();

            _iconTransform = _iconImage.transform;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
            
            if (!CheckAssigned())
            {
                return;
            }

            _ironInventory.SetDraggingSlot(this);

            Debug.Log("Начало перетаскивания");
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
            
            if (!CheckAssigned())
            {
                return;
            }

            _ironInventory.SetDraggingSlot(null);

            TryMoveToSlot();

            if (_isSelecting && !_isPointing)
            {
                SetSelection(false);
            }

            _iconTransform.localPosition = Vector3.zero;
            
            Debug.Log("Завершение перетаскивания");
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
            
            if (!CheckAssigned())
            {
                return;
            }
            
            _iconTransform.position = eventData.position;
            
            Debug.Log("Перетаскивание");
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isPointing = true;

            SetSelection(true);
            
            _ironInventory.SetSelectedSlot(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isPointing = false;

            SetSelection(false);

            _ironInventory.SetSelectedSlot(null);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
            
            if (CheckLocked())
            {
                return;
            }

            if (!CheckAssigned())
            {
                return;
            }

            if (eventData.clickCount >= 1)
            {
                _ironInventory.Interact(Name);
            }
        }
    }
}