using System;
using UnityEngine;

namespace Oxygen
{
	public interface IInteractive
	{
		bool Interact(Player player);
		void StopInteraction(Player player);
		bool IsEnabled { get; }
	}

	public abstract class BaseInteractive : Behaviour, IInteractive
	{
		public event Action<Player> Interacting;
		public event Action<Player> Failed;

		public event Action InteractionStopped;

		[Header("Interactive")]

		[SerializeField] private bool _isEnabled;
		[SerializeField] private bool _isSingle;

		protected abstract bool OnInteract(Player player);
		
		protected virtual void OnFailed(Player player)
		{

		}

		protected virtual void OnStopInteraction(Player player)
		{
			
		}
		
		public bool Interact(Player player)
		{
			if (!_isEnabled)
			{
				return false;
			}
			
			if (OnInteract(player))
			{
				Interacting?.Invoke(player);
				
				return true;
			}

			OnFailed(player);

			Failed?.Invoke(player);

			return false;
		}

		public void StopInteraction(Player player)
		{
			OnStopInteraction(player);
			InteractionStopped?.Invoke();
		}

		public void SetEnabled(bool value)
		{
			_isEnabled = value;
		}

		public bool IsEnabled => _isEnabled;
		public bool IsSingle => _isSingle;
	}
}