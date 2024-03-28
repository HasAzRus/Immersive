namespace Oxygen
{
    public class HeadUpDisplay : PageWidget
    {
        public void Construct(Player player)
        {
            OnConstruct(player);
        }
        
        protected virtual void OnConstruct(Player player)
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