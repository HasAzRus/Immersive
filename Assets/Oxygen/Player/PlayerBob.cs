using UnityEngine;

namespace Oxygen
{
	public class PlayerBob : Behaviour
	{
		[SerializeField] private Transform _transform;

		[SerializeField] private bool _allowTilt;
		[SerializeField] private float _tiltIntensity;
		
		[SerializeField] private bool _invertHorizontalTilt;
		[SerializeField] private bool _invertVerticalTilt;

		[SerializeField] private bool _allowShake;
		
		[SerializeField] private float _shakeIntensity;
		[SerializeField] private float _shakeSpeed;

		[SerializeField] private bool _alwaysShake;
		[SerializeField] private float _minShakeIntensity;

		private Quaternion _originRotation;

		private float _vTilt;
		private float _hTilt;

		private float _vShake;
		private float _minShake;

		private Vector3 _velocity;
		private bool _isBobing;

		protected override void Start()
		{
			base.Start();

			_originRotation = _transform.localRotation;
		}

		protected override void Update()
		{
			base.Update();

			var deltaTime = Time.deltaTime;

			if (_allowTilt)
			{
				var invertHorizontalAmount = _invertHorizontalTilt ? -1 : 1;
				var invertVerticalAmount = _invertVerticalTilt ? -1 : 1;
				
				_vTilt = Mathf.Lerp(_vTilt, _isBobing ? _velocity.z * invertVerticalAmount: 0f, deltaTime * 2);
				_hTilt = Mathf.Lerp(_hTilt, _isBobing ? _velocity.x * invertHorizontalAmount: 0f, deltaTime * 2);
			}

			if (_allowShake)
			{
				if (_isBobing)
				{
					_vShake = Mathf.Sin(Time.time * _shakeSpeed) * _velocity.magnitude;
				}
				else
				{
					_vShake = Mathf.Lerp(_vShake, 0f, deltaTime * 8);
				}
			}

			_minShake = Mathf.Sin(Time.time) * _minShakeIntensity;

			_transform.localRotation =
				_originRotation * Quaternion.AngleAxis(_hTilt * _tiltIntensity, Vector3.forward) *
				Quaternion.AngleAxis(_vTilt * _tiltIntensity + _vShake * _shakeIntensity + _minShake,
					Vector3.right);
		}

		public void Bob(Vector3 velocity)
		{
			_velocity = velocity;
			
			_isBobing = true;
		}

		public void StopBobing()
		{
			_isBobing = false;
		}

		public bool CheckIsBobing()
		{
			return _isBobing;
		}
	}
}