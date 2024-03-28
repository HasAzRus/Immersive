using UnityEngine;
using UnityEngine.UI;

namespace Oxygen
{
	public class Hint : Behaviour
	{
		[SerializeField] private float _maxTime;

		[SerializeField] private bool _allowFading;
		[SerializeField] private Text _text;

		private float _currentTimeAmount;

		private Color _color;

		private float _time;
		private bool _isShowing;

		private void SetText(string text)
		{
			_text.text = text;
		}

		protected virtual void OnTimerFinished()
		{
			Hide();
		}

		protected override void Start()
		{
			base.Start();

			_color = _text.color;
			_color.a = 0;

			if (_allowFading)
			{
				_text.color = _color;
			}
		}

		protected override void Update()
		{
			base.Update();

			var deltaTime = Time.deltaTime;

			if (_isShowing)
			{
				if (_time < _maxTime * _currentTimeAmount)
				{
					_time += deltaTime;
				}
				else
				{
					_time -= _maxTime;

					OnTimerFinished();
				}

				if (_allowFading)
				{
					_color.a = Mathf.PingPong(_time / _maxTime, 0.5f);

					_text.color = _color;
				}
			}
		}

		protected void ResetTimer()
		{
			_time = 0;
		}

		public void Show(string text)
		{
			Show(text, 1f);
		}

		public void Show(string text, float timeAmount)
		{
			SetText(text);
			
			Show(timeAmount);
		}

		public void Show(float timeAmount)
		{
			Show();
			
			_currentTimeAmount = timeAmount;
		}

		public void Show()
		{
			_currentTimeAmount = 1;
			
			_isShowing = true;
			_text.gameObject.SetActive(true);

			ResetTimer();
		}

		public void Hide()
		{
			_isShowing = false;
			_text.gameObject.SetActive(false);
		}

		public bool CheckShowing()
		{
			return _isShowing;
		}
	}
}