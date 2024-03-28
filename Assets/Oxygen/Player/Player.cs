using UnityEngine;

namespace Oxygen
{
	public class Player : Character
	{
		public void Construct(Game game)
		{
			game.Beginned += OnGameBeginned;
			
			OnConstruction(game);
			
			Game = game;
		}
		
		protected override void Start()
		{
			base.Start();
			
			InitializeComponent();

			Game.ConnectPlayer(this);
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
			Input = GetComponent<PlayerInput>();
			Camera = GetComponent<PlayerCamera>();
			
			Input.Construct(this);
		}

		protected override void OnDied(GameObject caller)
		{
			base.OnDied(caller);
			
			Game.EndGame(GameEndReason.Loss);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			
			Game.DisconnectPlayer(this);
			
			Game.Beginned -= OnGameBeginned;
		}

		public PlayerInput Input { get; private set; }
		public PlayerCamera Camera { get; private set; }
		public Game Game { get; private set; }
	}
}