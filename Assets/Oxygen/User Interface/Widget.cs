namespace Oxygen
{
    public class Widget : Behaviour
    {
        private void SetActive(bool value)
        {
            OnActiveChanged(value);
        }

        protected virtual void OnActiveChanged(bool value)
        {
            
        }
        
        public void Show()
        {
            SetActive(true);
        }

        public void Hide()
        {
            SetActive(false);
        }
    }
}