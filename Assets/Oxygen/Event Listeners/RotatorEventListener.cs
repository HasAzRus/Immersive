using UnityEngine.Events;
using UnityEngine;

namespace Oxygen
{
	public class RotatorEventListener : Behaviour
	{
		[SerializeField] private Rotator _target;

		[SerializeField] private UnityEvent _onRotatedEvent;
		[SerializeField] private UnityEvent _onReversedEvent;

		[SerializeField] private UnityEvent<float> _onValueChangedEvent;

		private void OnRotated()
		{
			_onRotatedEvent.Invoke();
		}

		private void OnReversed()
		{
			_onReversedEvent.Invoke();
		}

		private void OnValueChanged(float value)
		{
			_onValueChangedEvent.Invoke(value);
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			_target.Rotated += OnRotated;
			_target.Reversed += OnReversed;

			_target.ValueChanged += OnValueChanged;
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			_target.Rotated -= OnRotated;
			_target.Reversed -= OnReversed;

			_target.ValueChanged -= OnValueChanged;
		}
	}
}
