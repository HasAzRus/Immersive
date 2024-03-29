using System;
using Oxygen;
using UnityEngine;
using UnityEngine.UI;

namespace Iron
{
    public class IronHealthWidget : PageWidget
    {
        [SerializeField] private float _healthMinValue;
        [SerializeField] private Image _healthBarImage;
        
        private IronPlayer _ironPlayer;
        
        public void Construct(IronPlayer ironPlayer)
        {
            ironPlayer.ReadableHealth.ValueChanged += OnHealthChanged;
            ironPlayer.DamageApplied += OnDamageApplied;

            _ironPlayer = ironPlayer;
        }
        
        private void OnHealthChanged(float value)
        {
            _healthBarImage.fillAmount = MathF.Round(value / _ironPlayer.ReadableHealth.MaxValue, 2);
            _healthBarImage.color = value > _healthMinValue ? Color.white : Color.red;
        }
        
        private void OnDamageApplied(GameObject caller, float amount)
        {
            
        }

        public void Clear()
        {
            _ironPlayer.ReadableHealth.ValueChanged -= OnHealthChanged;
            _ironPlayer.DamageApplied -= OnDamageApplied;
        }
    }
}