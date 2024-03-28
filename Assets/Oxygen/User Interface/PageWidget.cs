using UnityEngine;

namespace Oxygen
{
    public class PageWidget : Widget
    {
        [SerializeField] private GameObject _baseGameObject;

        protected override void OnActiveChanged(bool value)
        {
            base.OnActiveChanged(value);
            
            _baseGameObject.SetActive(value);
        }
    }
}