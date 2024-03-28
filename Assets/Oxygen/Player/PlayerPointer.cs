using System;
using UnityEngine;

namespace Oxygen
{
	public enum PlayerPointerMode
	{
		Camera,
		Cursor
	}

	public class PlayerPointer : Behaviour
	{
		public event Action<GameObject> Pointed;
		public event Action<GameObject> Unpointed;

		[SerializeField] private float _distance;
		[SerializeField] private LayerMask _layerMask;

		[SerializeField] private Camera _camera;

		private bool _isEnabled;

		private PlayerPointerMode _mode;

		private GameObject _currentPointerGameObject;
		private IPointerTarget _currentTarget;

		private Transform _cameraTransform;
		
		private Player _player;

		private bool TraceByMode(float distance, PlayerPointerMode mode, out RaycastHit hitInfo)
		{
			switch (mode)
			{
				case PlayerPointerMode.Camera:
					return World.Trace(new Ray(_cameraTransform.position, _cameraTransform.forward), distance,
						_layerMask, out hitInfo);
				case PlayerPointerMode.Cursor:
					return World.Trace(_camera.ScreenPointToRay(Input.mousePosition), distance, _layerMask, out hitInfo);
				default:
					hitInfo = default;

					return false;
			}
		}

		private bool TraceByMode(PlayerPointerMode mode, out RaycastHit hitInfo)
		{
			return TraceByMode(_distance, mode, out hitInfo);
		}
		
		private void Clear()
		{
			_currentTarget.Exit(_player);

			Unpointed?.Invoke(_currentPointerGameObject);

			_currentTarget = null;
			_currentPointerGameObject = null;
		}

		public void Construct(Player player)
		{
			_player = player;
		}

		protected override void Start()
		{
			base.Start();

			_cameraTransform = _camera.transform;
		}

		protected override void Update()
		{
			base.Update();

			if (!_isEnabled)
			{
				TryClear();
				
				return;
			}

			if(!TraceByMode(_mode, out var hit))
			{
				TryClear();

				return;
			}

			if(hit.collider.gameObject == _currentPointerGameObject)
			{
				return;
			}

			if(!hit.collider.TryGetComponent<IPointerTarget>(out var target))
			{
				TryClear();

				return;
			}

			if(!target.CheckEnabled())
			{
				TryClear();

				return;
			}

			TryClear();

			if (!target.Enter(_player))
			{
				return;
			}
			
			_currentPointerGameObject = hit.collider.gameObject;
			_currentTarget = target;

			Pointed?.Invoke(_currentPointerGameObject);
		}

		public bool TryClear()
		{
			if (_currentTarget == null)
			{
				return false;
			}

			Clear();

			return true;
		}

		public void SetMode(PlayerPointerMode value)
		{
			_mode = value;
		}

		public void SetDistance(float value)
		{
			_distance = value;
		}

		public void SetEnabled(bool value)
		{
			_isEnabled = value;
		}
	}
}