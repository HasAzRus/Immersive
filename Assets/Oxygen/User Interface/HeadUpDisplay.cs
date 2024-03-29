namespace Oxygen
{
    public class HeadUpDisplay : PageWidget
    {
        public void Construct(Player player)
        {
            OnConstruction(player);
        }
        
        protected virtual void OnConstruction(Player player)
        {
            
        }

        protected virtual void OnClear()
        {
            
        }

        public void Clear()
        {
            OnClear();
        }
    }
}