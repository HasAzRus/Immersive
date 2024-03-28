namespace Oxygen
{
	public interface IPointerTarget
	{
		bool Enter(Player player);
		void Exit(Player player);

		bool CheckEnabled();
	}

	public abstract class BasePointerTarget : Behaviour, IPointerTarget
	{
		protected abstract bool OnEnter(Player player);
		
		protected virtual void OnExit(Player player)
		{

		}

		public bool Enter(Player player)
		{
			return OnEnter(player);
		}

		public void Exit(Player player)
		{
			OnExit(player);
		}

		public bool CheckEnabled() 
		{
			return enabled;
		}
	}
}