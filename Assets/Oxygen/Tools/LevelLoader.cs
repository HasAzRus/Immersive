using UnityEngine;

namespace Oxygen
{
    public class LevelLoader : Behaviour
    {
        [SerializeField] private Level _level;
        [SerializeField] private LoadLevelMode _mode;
        
        public void Load()
        {
            LevelManager.Load(_level, _mode);
        }
    }
}