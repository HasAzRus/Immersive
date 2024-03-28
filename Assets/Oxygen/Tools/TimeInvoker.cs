using System;
using UnityEngine;

namespace Oxygen
{
    public class TimeInvoker : Behaviour
    {
	    public static event Action<float> UpdateTimeTicked;
        public static event Action<float> UpdateTimeUnscaledTicked;

        public static event Action OneSecondTicked;
        public static event Action OneSecondUnscaledTicked;

        private static TimeInvoker _instance;

        private float _oneSecondTimer;
        private float _oneSecondUnscaledTimer;

        public static void Initialize()
        {
	        if (_instance != null)
	        {
		        return;
	        }

	        _instance = new GameObject("Timer Invoker").AddComponent<TimeInvoker>();
        }

        protected override void Update()
		{
			base.Update();

            var deltaTime = Time.deltaTime;
            UpdateTimeTicked?.Invoke(deltaTime);

            _oneSecondTimer += deltaTime;

            if(_oneSecondTimer >= 1f)
			{
                _oneSecondTimer -= 1f;
                OneSecondTicked?.Invoke();
			}

            var unscaledDeltaTime = Time.unscaledDeltaTime;
            UpdateTimeUnscaledTicked?.Invoke(unscaledDeltaTime);

            _oneSecondUnscaledTimer += unscaledDeltaTime;

            if(_oneSecondUnscaledTimer >= 1f)
			{
                _oneSecondUnscaledTimer -= 1f;
                OneSecondUnscaledTicked?.Invoke();
			}
		}
	}
}
