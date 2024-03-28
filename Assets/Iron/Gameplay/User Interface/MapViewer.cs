using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Behaviour = Oxygen.Behaviour;

namespace Iron
{
    public class MapViewer : Behaviour, 
        IScrollHandler, 
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler
    {
        [SerializeField] private Transform _targetTransform;

        [SerializeField] private float _speed;
        
        [SerializeField] private float _maxScale;
        [SerializeField] private float _minScale;

        private Vector2 _dragPosition;
        
        private float _scale;
        
        public void OnScroll(PointerEventData eventData)
        {
            _scale += eventData.scrollDelta.y * _speed;
            _scale = Mathf.Clamp(_scale, 0f, 1f);

            _targetTransform.localScale = Vector3.one * _minScale + Vector3.one * _scale * _maxScale;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            
        }

        public void OnDrag(PointerEventData eventData)
        {
            _dragPosition += eventData.delta;
            _dragPosition = Vector3.ClampMagnitude(_dragPosition, 2000f * _scale);

            _targetTransform.localPosition = _dragPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            
        }
    }
}