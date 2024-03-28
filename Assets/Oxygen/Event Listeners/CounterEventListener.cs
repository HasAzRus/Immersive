using UnityEngine;
using UnityEngine.Events;

namespace Oxygen
{
	public class CounterEventListener : Behaviour
	{
		[SerializeField] private Counter _target;

		[SerializeField] private UnityEvent<int> _onNumberChangedEvent;
		[SerializeField] private UnityEvent _onCountedEvent;

		private void OnNumberChanged(int value)
		{
			_onNumberChangedEvent.Invoke(value);
		}

		private void OnCounted()
		{
			_onCountedEvent.Invoke();
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			_target.NumberChanged += OnNumberChanged;
			_target.Counted += OnCounted;
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			_target.NumberChanged -= OnNumberChanged;
			_target.Counted -= OnCounted;
		}
	}
}