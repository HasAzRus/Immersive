using UnityEngine;
using UnityEngine.Events;

namespace Oxygen
{
	public class GameBeginPlayEventListener : Behaviour
	{
		[SerializeField] private UnityEvent<Player> _onBeginPlayEvent;

		private void OnGameGlobalBeginned(Player player)
		{
			_onBeginPlayEvent.Invoke(player);
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			Game.GlobalBeginned += OnGameGlobalBeginned;
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			Game.GlobalBeginned -= OnGameGlobalBeginned;
		}
	}
}