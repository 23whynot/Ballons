

namespace CodeBase.GamePlay.Level.Holder
{
    public interface ILevelHolder
    {
        public void SetConfig(LevelConfig config);

        public LevelConfig GetConfig();
    }
}