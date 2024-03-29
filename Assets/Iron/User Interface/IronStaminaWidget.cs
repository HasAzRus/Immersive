using System;
using Oxygen;
using UnityEngine;
using UnityEngine.UI;

namespace Iron
{
    public class IronStaminaWidget : PageWidget
    {
        [SerializeField] private Image _staminaBarImage;
        
        private IronPlayer _ironPlayer;
        
        public void Construct(IronPlayer ironPlayer)
        {
            ironPlayer.Stamina.ValueChanged += OnStaminaChanged;

            _ironPlayer = ironPlayer;
        }
        
        private void OnStaminaChanged(float value)
        {
            _staminaBarImage.fillAmount = MathF.Round(value / _ironPlayer.Stamina.MaxValue, 2);
            _staminaBarImage.color = value > _ironPlayer.Stamina.MinEnoughValue ? Color.white : Color.red;
        }

        public void Clear()
        {
            _ironPlayer.Stamina.ValueChanged -= OnStaminaChanged;
        }
    }
}