using System;
using System.Collections;
using UnityEngine;

namespace Oxygen
{
	public enum GameEndReason
	{
		Win,
		Loss
	}
	public class Game : Behaviour
	{
		public static event Action<Player> GlobalBeginned;

		public event Action<Player> PlayerConnected;
		public event Action<Player> PlayerDisconnected;

		public event Action Beginned;
		public event Action<GameEndReason> Ended;

		public event Action<bool> PauseChanged;

		public event Action Saving;
		public event Action Loaded;

		[SerializeField] private Player _defaultPlayer;
		[SerializeField] private UserInterface _defaultUserInterface;

		[SerializeField] private Launcher _launcher;

		private bool _isPause;

		private void SetPause(bool value)
		{
			_isPause = value;

			OnPauseChanged(value);
			PauseChanged?.Invoke(value);
		}

		private IEnumerator BeginPlayRoutine(Player player)
		{
			yield return null;

			Debug.Log("Игра началась");

			OnBeginned(player);
			
			Beginned?.Invoke();
			GlobalBeginned?.Invoke(player);
		}

		protected virtual void OnPauseChanged(bool value)
		{
			
		}

		protected virtual void OnBeginned(Player player)
		{
			
		}

		protected virtual void OnEnded(GameEndReason reason)
		{
			
		}

		protected virtual void OnPlayerConnected(Player player)
		{
			
		}

		protected virtual void OnPlayerDisconnected(Player player)
		{
			
		}

		protected virtual void OnLevelLoading()
		{
			
		}

		protected virtual void OnLevelLoaded(ILevel level, LoadLevelMode loadLevelMode)
		{
			Player player = null;

			if (loadLevelMode == LoadLevelMode.Single)
			{
				Transform spawnTransform;

				var playerStart = FindFirstObjectByType<PlayerStart>();

				if (playerStart == null)
				{
					Debug.LogError("Отсутствует старт игрока");

					return;
				}

				spawnTransform = playerStart.transform;
				
				Debug.Log("Спавн игрока");

				player = Instantiate(_defaultPlayer, spawnTransform.position,
					spawnTransform.rotation);

				player.Construct(this);
			}

			if (player != null)
			{
				Coroutines.Run(BeginPlayRoutine(player));
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			LevelManager.Loading += OnLevelLoading;
			LevelManager.Loaded += OnLevelLoaded;
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			LevelManager.Loading -= OnLevelLoading;
			LevelManager.Loaded -= OnLevelLoaded;
		}

		protected override void Start()
		{
			base.Start();

			Debug.Log("Создание экземпляра игры");

			var userInterface = Instantiate(_defaultUserInterface);
			userInterface.Construct(this);
			
			Debug.Log("Загрузка стартового уровня");
			LevelManager.Load(_launcher.GetLevel(), LoadLevelMode.Single);
		}

		public void ConnectPlayer(Player player)
		{
			Debug.Log("Подключение игрока");
			
			OnPlayerConnected(player);
			PlayerConnected?.Invoke(player);
		}

		public void DisconnectPlayer(Player player)
		{
			Debug.Log("Отключение игрока");
			
			OnPlayerDisconnected(player);
			PlayerDisconnected?.Invoke(player);
		}

		public void EndGame(GameEndReason reason)
		{
			Debug.Log($"Игра закончена: {reason}");
			
			OnEnded(reason);
			Ended?.Invoke(reason);
		}

		public void Load(int index)
		{
			SaveLoad.Load(index);
			
			Debug.Log("Загрузка данных");

			Loaded?.Invoke();
		}

		public void Save(int index)
		{
			Saving?.Invoke();
			
			Debug.Log("Сохранение данных");

			SaveLoad.Save(index);
		}

		public void Pause()
		{
			SetPause(true);
		}

		public void Unpause()
		{
			SetPause(false);
		}

		public bool IsPause => _isPause;
	}
}