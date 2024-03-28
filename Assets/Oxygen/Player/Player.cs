using UnityEngine;

namespace Oxygen
{
	public class Player : Character
	{
		private Game _game;
		
		private PlayerInput _input;
		private PlayerCamera _camera;

		public void Construct(Game game)
		{
			game.Beginned += OnGameBeginned;
			
			OnConstruction(game);
			
			_game = game;
		}

		private void InitializeComponent()
		{
			OnInitializeComponent();
		}

		protected virtual void OnConstruction(Game game)
		{
			
		}

		protected virtual void OnGameBeginned()
		{
			
		}

		protected virtual void OnInitializeComponent()
		{
			_input = GetComponent<PlayerInput>();
			_camera = GetComponent<PlayerCamera>();
			
			_input.Construct(this);
		}

		protected override void OnDied(GameObject caller)
		{
			base.OnDied(caller);
			
			_game.EndGame(GameEndReason.Loss);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			
			_game.DisconnectPlayer(this);
			
			_game.Beginned -= OnGameBeginned;
		}

		protected override void Start()
		{
			base.Start();
			
			InitializeComponent();

			_game.ConnectPlayer(this);
		}

		public PlayerInput GetInput()
		{
			return _input;
		}

		public PlayerCamera GetCamera()
		{
			return _camera;
		}

		public Game GetGame()
		{
			return _game;
		}
	}
}