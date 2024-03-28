using System.Collections.Generic;

namespace Oxygen
{
	public class QueuesHint : Hint
	{
		private List<string> _texts;
		private List<float> _textsTimeAmount;

		protected override void Awake()
		{
			base.Awake();

			_texts = new List<string>();
			_textsTimeAmount = new List<float>();
		}

		protected override void OnTimerFinished()
		{
			if (_texts.Count > 0)
			{
				Show(_texts[0], _textsTimeAmount[0]);

				_texts.RemoveAt(0);
				_textsTimeAmount.RemoveAt(0);

				return;
			}

			base.OnTimerFinished();
		}

		public void AddText(string text, float timeAmount = 1)
		{
			if(CheckShowing())
			{
				_texts.Add(text);
				_textsTimeAmount.Add(timeAmount);

				return;
			}

			Show(text, timeAmount);
		}

		public void SetText(string text)
		{
			Show(text);
		}

		public void SetText(string text, float timeAmount)
		{
			Show(text, timeAmount);
		}
	}
}