using System;
using Oxygen;
using UnityEngine;
using Behaviour = Oxygen.Behaviour;

namespace Iron
{
    public class IronPlayerFlashlight : Behaviour, IReadableFlashlight
    {
        public event Action Enabled;
        public event Action Disabled;
        
        [SerializeField] private Energy _energy;
        [SerializeField] private float _cost;

        [SerializeField] private float _maxIntensity;
        [SerializeField] private Light _light;

        private Coroutine _crossFadeRoutine;

        public void StartCrossFade(float intensity)
        {
            if (_crossFadeRoutine != null)
            {
                Coroutines.Stop(_crossFadeRoutine);
            }

            _crossFadeRoutine = Coroutines.Run(ProcedureAnimation.CrossFadeLight(intensity, 2f, _light));
        }

        protected override void Update()
        {
            base.Update();

            var deltaTime = Time.deltaTime;

            if (!IsFlashlightEnabled)
            {
                return;
            }
            
            _energy.Remove(_cost * deltaTime);

            if(_energy.IsEmpty)
            {
                Disable();
            }
        }
        
        public void Enable()
        {
            StartCrossFade(_maxIntensity);

            IsFlashlightEnabled = true;
            
            Enabled?.Invoke();
        }

        public void Disable()
        {
            StartCrossFade(0f);
            
            IsFlashlightEnabled = false;

            Disabled?.Invoke();
        }

        public void AddEnergy(float value)
        {
            _energy.Add(value);
        }

        public void FillEnergy()
        {
            _energy.Fill();
        }
        
        public IReadableEnergy ReadableEnergy => _energy;
        public bool IsFlashlightEnabled { get; private set; }
    }
}