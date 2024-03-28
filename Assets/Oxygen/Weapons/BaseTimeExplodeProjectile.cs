using UnityEngine;

namespace Oxygen
{
	public abstract class BaseTimeExplodeProjectile : BaseExplodeProjectile
	{
		[SerializeField] private int _maxTime;

		private bool _isTimer;
		private float _time;

		protected override void Update()
		{
			base.Update();

			if (_isTimer)
			{
				if (_time < _maxTime)
				{
					_time += Time.deltaTime;
				}
				else
				{
					_time -= _maxTime;

					Explode();
				}
			}
		}

		public void StartTimer()
		{
			_isTimer = true;
		}

		public void PauseTimer()
		{
			_isTimer = false;
		}

		public void StopTimer() 
		{
			_isTimer = false;
			_time = 0;
		}
	}
}