using System;
using UnityEngine;

namespace Oxygen
{
	public enum TimerType
	{
		UpdateTick,
		UpdateTickUnscaled,
		OneSecondTick,
		OneSecondTickUnscaled
	}
	
	public class Timer
	{
		public event Action<float> ValueChanged;
		public event Action Finished;
		
		private float _remainingSeconds;
		private readonly TimerType _type;

		private bool _isPaused;

		public Timer(TimerType type)
		{
			_type = type;
		}

		private void OnUpdateTick(float deltaTime)
		{
			if (_isPaused)
			{
				return;
			}

			_remainingSeconds -= deltaTime;

			CheckFinish();
		}

		private void OnOneSecondTick()
		{
			if (_isPaused)
			{
				return;
			}

			_remainingSeconds -= 1f;

			CheckFinish();
		}

		private void CheckFinish()
		{
			if(_remainingSeconds <= 0f)
			{
				Stop();
			}
			else
			{
				ValueChanged?.Invoke(_remainingSeconds);
			}
		}

		private void Subscribe()
		{
			switch (_type)
			{
				case TimerType.UpdateTick: TimeInvoker.UpdateTimeTicked += OnUpdateTick;
					break;
				case TimerType.UpdateTickUnscaled: TimeInvoker.UpdateTimeUnscaledTicked += OnUpdateTick;
					break;
				case TimerType.OneSecondTick: TimeInvoker.OneSecondTicked += OnOneSecondTick;
					break;
				case TimerType.OneSecondTickUnscaled: TimeInvoker.OneSecondUnscaledTicked += OnOneSecondTick;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void Unsubscribe()
		{
			switch (_type)
			{
				case TimerType.UpdateTick:
					TimeInvoker.UpdateTimeTicked -= OnUpdateTick;
					break;
				case TimerType.UpdateTickUnscaled:
					TimeInvoker.UpdateTimeUnscaledTicked -= OnUpdateTick;
					break;
				case TimerType.OneSecondTick:
					TimeInvoker.OneSecondTicked -= OnOneSecondTick;
					break;
				case TimerType.OneSecondTickUnscaled:
					TimeInvoker.OneSecondUnscaledTicked -= OnOneSecondTick;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void Start(float seconds)
		{
			if(seconds == 0)
			{
				Debug.LogError($"Начальное значение Timer-а ({this}) не может быть равен нулю");
				Finished?.Invoke();
			}

			SetTime(seconds);

			_isPaused = false;
			Subscribe();
		}

		public void SetTime(float seconds)
		{
			ValueChanged?.Invoke(seconds);
			_remainingSeconds = seconds;
		}

		public void Pause()
		{
			_isPaused = true;
			Unsubscribe();

			ValueChanged?.Invoke(_remainingSeconds);
		}

		public void Unpause()
		{
			_isPaused = false;
			Subscribe();

			ValueChanged?.Invoke(_remainingSeconds);
		}

		public void Stop()
		{
			Unsubscribe();

			_remainingSeconds = 0;

			ValueChanged?.Invoke(_remainingSeconds);
			Finished?.Invoke();
		}

		public bool CheckPaused()
		{
			return _isPaused;
		}
	}
}
