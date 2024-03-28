using System;
using UnityEngine;

namespace Oxygen
{
	public sealed class Mover : Behaviour
	{
		public event Action Moved;
		public event Action Reversed;

		[SerializeField] private Vector3 _position;
		[SerializeField] private bool _isLocal;

		[SerializeField] private float _speed;

		[SerializeField] private bool _allowToggle;
		[SerializeField] private bool _initialToggleState;

		[SerializeField] private bool _onlyOnce;

		private Vector3 _targetPosition;
		private Vector3 _movePosition;

		private Vector3 _initialPosition;

		private bool _isMoving;

		private bool _isToggled;

		protected override void Start()
		{
			base.Start();

			_initialPosition = _isLocal ? transform.localPosition : transform.position;
			_movePosition = _initialPosition;

			_targetPosition = _position;

			_isToggled = _initialToggleState;
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();

			if (_isMoving)
			{
				_movePosition = Vector3.MoveTowards(_movePosition, _targetPosition, _speed * Time.deltaTime);

				if (_movePosition == _targetPosition)
				{
					_isMoving = false;

					if (!_isToggled)
					{
						Moved?.Invoke();

						if(_onlyOnce)
						{
							enabled = false;
						}
					}
					else
					{
						Reversed?.Invoke();
					}
				}
			}

			if (_isLocal)
			{
				transform.localPosition = _movePosition;
			}
			else
			{
				transform.position = _movePosition;
			}
		}
		
		public bool StartMovement(bool reverse)
		{
			if (!_allowToggle)
			{
				if (_isMoving)
				{
					return false;
				}
			}
			
			_isMoving = true;

			_isToggled = reverse;
			_targetPosition = _isToggled ? _position : _initialPosition;

			return true;
		}

		public void StopMovement()
		{
			_isMoving = false;
		}
	}
}
