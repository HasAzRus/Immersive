using UnityEngine.Events;
using UnityEngine;

namespace Oxygen
{
	public class MoverEventListener : Behaviour
	{
		[SerializeField] private Mover _target;

		[SerializeField] private UnityEvent _onMovedEvent;
		[SerializeField] private UnityEvent _onReversedEvent;

		private void OnMoved()
		{
			_onMovedEvent.Invoke();
		}

		private void OnReversed()
		{
			_onReversedEvent.Invoke();
		}

		protected override void OnEnable()
		{
			_target.Moved += OnMoved;
			_target.Reversed += OnReversed;
		}

		protected override void OnDisable()
		{
			_target.Moved -= OnMoved;
			_target.Reversed -= OnReversed;
		}
	}
}
