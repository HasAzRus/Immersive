using System.Collections;
using UnityEngine;

namespace Oxygen
{
	public class Coroutines : Behaviour
	{
		private static Coroutines _instance;

		public static void Initialize()
		{
			if (_instance != null)
			{
				return;
			}

			_instance = new GameObject("Coroutines").AddComponent<Coroutines>();
		}

		public static Coroutine Run(IEnumerator routine)
		{
			return _instance.StartCoroutine(routine);
		}

		public static void Stop(Coroutine routine)
		{
			_instance.StopCoroutine(routine);
		}

		public static void StopAll()
		{
			_instance.StopAllCoroutines();
		}
	}
}