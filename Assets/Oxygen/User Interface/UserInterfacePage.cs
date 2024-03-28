using UnityEngine;

namespace Oxygen
{
    public class UserInterfacePage : UserInterfaceGameObject
    {
        [SerializeField] private GameObject _baseGameObject;

        protected override void OnActiveChanged(bool value)
        {
            base.OnActiveChanged(value);
            
            _baseGameObject.SetActive(value);
        }
    }
}