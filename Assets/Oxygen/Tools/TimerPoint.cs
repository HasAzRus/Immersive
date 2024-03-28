using System;
using UnityEngine;

namespace Oxygen
{
	public class TimerPoint : Behaviour
	{
		public event Action Finished;
		public event Action<float> ValueChanged;

		[SerializeField] private TimerType _type;

		private Timer _timer;

		private void OnTimerFinished()
		{
			Finished?.Invoke();
		}

		private void OnTimerValueChanged(float value)
		{
			ValueChanged?.Invoke(value);
		}

		protected override void Awake()
		{
			base.Awake();

			_timer = new Timer(_type);
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			_timer.Finished += OnTimerFinished;
			_timer.ValueChanged += OnTimerValueChanged;
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			_timer.Finished -= OnTimerFinished;
			_timer.ValueChanged -= OnTimerValueChanged;
		}

		public void StartTimer(float seconds)
		{
			_timer.Start(seconds);
		}

		public void SetTime(float seconds)
		{
			_timer.SetTime(seconds);
		}

		public void Stop()
		{
			_timer.Stop();
		}

		public void Pause()
		{
			_timer.Pause();
		}

		public void Unpause()
		{
			_timer.Unpause();
		}
	}
}
