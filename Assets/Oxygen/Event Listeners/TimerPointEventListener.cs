using UnityEngine.Events;
using UnityEngine;

namespace Oxygen
{
	public class TimerPointEventListener : Behaviour
	{
		[SerializeField] private TimerPoint _target;

		[SerializeField] private UnityEvent _onFinishedEvent;
		[SerializeField] private UnityEvent<float> _onValueChangedEvent;

		private void OnFinished()
		{
			_onFinishedEvent.Invoke();
		}

		private void OnValueChanged(float value)
		{
			_onValueChangedEvent.Invoke(value);
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			_target.Finished += OnFinished;
			_target.ValueChanged += OnValueChanged;
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			_target.Finished -= OnFinished;
			_target.ValueChanged -= OnValueChanged;
		}
	}
}
