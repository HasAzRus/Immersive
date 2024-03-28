using System;
using UnityEngine;

namespace Oxygen
{
	[RequireComponent(typeof(CounterEventListener))]
	public class Counter : Behaviour
	{
		public event Action<int> NumberChanged;
		public event Action Counted;

		[SerializeField] private int _defaultNumber;
		[SerializeField] private int _maxNumber;

		private int _number;

		protected override void Start()
		{
			base.Start();

			SetNumber(_defaultNumber);
		}

		public void SetNumber(int value)
		{
			if(value < 0 || value > _maxNumber)
			{
				throw new ArgumentOutOfRangeException();
			}

			_number = value;

			if(_number == _maxNumber)
			{
				Counted?.Invoke();
			}

			NumberChanged?.Invoke(value);
		}

		public void AddNumber(int value)
		{
			SetNumber(_number + value);
		}

		public void SubtractNumber(int value)
		{
			if(_number - value > 0)
			{
				SetNumber(_number - value);
			}
			else
			{
				SetNumber(0);
			}
		}
	}
}