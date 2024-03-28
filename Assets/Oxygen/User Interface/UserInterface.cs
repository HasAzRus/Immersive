using UnityEngine;

namespace Oxygen
{
	public class UserInterface : Behaviour
	{
		[SerializeField] private HeadUpDisplay _headUpDisplay;
		
		private Game _game;

		public void Construct(Game game)
		{
			Debug.Log("Создание пользовательского интерфейса");

			game.PlayerConnected += OnGamePlayerConnected;
			game.PlayerDisconnected += OnGamePlayerDisconnected;

			game.Beginned += OnGameBeginned; 
			
			OnConstruct(game);

			_game = game;
		}

		protected virtual void OnConstruct(Game game)
		{
			
		}

		protected virtual void OnGameBeginned()
		{
			
		}

		protected virtual void OnGamePlayerConnected(Player player)
		{
			_headUpDisplay.Construct(player);
		}

		protected virtual void OnGamePlayerDisconnected(Player player)
		{
			_headUpDisplay.Clear();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			_game.PlayerConnected -= OnGamePlayerConnected;
			_game.PlayerDisconnected -= OnGamePlayerDisconnected;

			_game.Beginned += OnGameBeginned;
		}

		protected Game GetGame()
		{
			return _game;
		}
	}
}