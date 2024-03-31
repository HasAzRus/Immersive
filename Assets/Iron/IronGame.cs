using Oxygen;

namespace Iron
{
    public class IronGame : Game
    {
        protected override void OnLevelLoading()
        {
            base.OnLevelLoading();
            
            SaveLoad.Save(false);
        }

        protected override void OnBeginned(Player player)
        {
            base.OnBeginned(player);
            
            SaveLoad.Load();
        }
    }
}