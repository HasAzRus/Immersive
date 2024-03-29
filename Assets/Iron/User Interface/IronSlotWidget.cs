using Oxygen;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Iron
{
    public class IronSlotWidget : SlotWidget,
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

        private IronInventoryWidget _ironInventoryWidget;
        
        protected override void OnConstruct(InventoryWidget inventoryWidget)
        {
            base.OnConstruct(inventoryWidget);
            
            _ironInventoryWidget = inventoryWidget as IronInventoryWidget;
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
            var selectedSlot = _ironInventoryWidget.GetSelectedSlot();

            if (selectedSlot == null)
            {
                _ironInventoryWidget.Drop(Name);
                
                return false;
            }

            if (selectedSlot == this)
            {
                return false;
            }

            if (!selectedSlot.IsAssigned)
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
            
            if (!_ironInventoryWidget.GetItemDatabase().TryGetValue(name, out var itemData))
            {
                return;
            }

            _iconImage.sprite = itemData.Sprite;
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
            
            if (!IsAssigned)
            {
                return;
            }

            _ironInventoryWidget.SetDraggingSlot(this);
            
            _countText.CrossFadeAlpha(0f, 0.2f, true);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
            
            if (!IsAssigned)
            {
                return;
            }

            _ironInventoryWidget.SetDraggingSlot(null);

            TryMoveToSlot();

            if (_isSelecting && !_isPointing)
            {
                SetSelection(false);
            }

            _iconTransform.localPosition = Vector3.zero;
            
            _countText.CrossFadeAlpha(1f, 0.2f, true);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
            
            if (!IsAssigned)
            {
                return;
            }
            
            _iconTransform.position = eventData.position;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isPointing = true;

            SetSelection(true);
            
            _ironInventoryWidget.SetSelectedSlot(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isPointing = false;

            SetSelection(false);

            _ironInventoryWidget.SetSelectedSlot(null);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
            
            if (IsLocked)
            {
                return;
            }

            if (!IsAssigned)
            {
                return;
            }

            if (eventData.clickCount >= 1)
            {
                _ironInventoryWidget.Interact(Name);
            }
        }
    }
}