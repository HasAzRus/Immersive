using UnityEngine;
using UnityEngine.UI;

namespace Oxygen
{
	public class FlashingOutline : Behaviour
	{
		[SerializeField] private float _maxTime;

		private float _time;

		private bool _isFlashing;

		private Outline _outline;

		protected override void Start()
		{
			base.Start();

			_outline = GetComponent<Outline>();
		}

		protected override void Update() 
		{
			if (_isFlashing) 
			{
				if (_time < _maxTime) 
				{
					_time += Time.deltaTime;
				}
				else
				{
					_time -= _maxTime;

					_isFlashing = false;
				}

				Color color = _outline.effectColor;
				color.a = Mathf.PingPong(_time / _maxTime, 0.5f);

				_outline.effectColor = color;
			}
		}

		public void Flash(bool force = false)
		{
			if(_isFlashing && !force)
			{
				return;
			}

			_time = 0f;

			_isFlashing = true;
		}

		public Outline GetOutline()
		{
			return _outline;
		}
	}
}