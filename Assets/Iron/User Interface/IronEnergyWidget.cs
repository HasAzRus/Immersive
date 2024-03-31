using System;
using Oxygen;
using UnityEngine;
using UnityEngine.UI;

namespace Iron
{
    public class IronEnergyWidget : PageWidget
    {
        [SerializeField] private Image _energyBarImage;
        
        private IronPlayer _ironPlayer;
        
        public void Construct(IronPlayer ironPlayer)
        {
            ironPlayer.Flashlight.Enabled += OnFlashlightEnabled;
            ironPlayer.Flashlight.Disabled += OnFlashlightDisabled;
            
            ironPlayer.Flashlight.ReadableEnergy.ValueChanged += OnEnergyValueChanged;

            _ironPlayer = ironPlayer;
        }

        private void OnEnergyValueChanged(float value)
        {
            _energyBarImage.fillAmount = MathF.Round(value / _ironPlayer.Flashlight.ReadableEnergy.MaxValue, 2);
        }

        private void OnFlashlightEnabled()
        {
            Show();
        }
        
        private void OnFlashlightDisabled()
        {
            Hide();
        }

        public void Clear()
        {
            _ironPlayer.Flashlight.Enabled += OnFlashlightEnabled;
            _ironPlayer.Flashlight.Disabled += OnFlashlightDisabled;
            
            _ironPlayer.Flashlight.ReadableEnergy.ValueChanged += OnEnergyValueChanged;

            _ironPlayer = null;
        }
    }
}