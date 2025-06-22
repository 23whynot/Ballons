

namespace CodeBase.GamePlay.Level.Holder
{
    public class LevelHolder : ILevelHolder
    {
        private LevelConfig _config;

        public void SetConfig(LevelConfig config) => 
            _config = config;
        
        public LevelConfig GetConfig() => 
            _config;
    }
}