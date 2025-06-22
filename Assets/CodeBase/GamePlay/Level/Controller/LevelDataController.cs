using System.Collections.Generic;
using CodeBase.Infrastructure.Services.SaveLoad;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CodeBase.GamePlay.Level.Controller
{
    public class LevelDataController : ILevelDataController
    {
        private ISaveLoadService _saveLoadService;
        private List<int> _unlockedLevels = new List<int>();
        private Dictionary<int, int> _compleatedLevels = new();

        [Inject]
        public void Construct(ISaveLoadService saveLoadService) =>
            _saveLoadService = saveLoadService;

        public async UniTask InitializeAsync()
        {
            _unlockedLevels = _saveLoadService.GameData.UnlockedLevels;
            _compleatedLevels = _saveLoadService.GameData.CompleatedLevels;

            if (_saveLoadService.GameData.IsFirstLaunch)
            {
                MarkLevelAsUnlocked(0);

                UpdateData();
            }
        }

        public void MarkLevelAsUnlocked(int level)
        {
            _unlockedLevels.Add(level);

            UpdateData();
        }

        public void MarkLevelAsComplete(int level, int starsCount)
        {
            if (_compleatedLevels.TryGetValue(level, out var existingStars))
            {
                if (starsCount > existingStars)
                    _compleatedLevels[level] = starsCount;
            }
            else
            {
                _compleatedLevels.Add(level, starsCount);
            }
            
            UpdateData();
        }

        public bool IsLevelUnlocked(int level) => 
            _unlockedLevels.Contains(level);

        public int GetStarsCount(int level) => 
            _compleatedLevels.ContainsKey(level) ? _compleatedLevels[level] : 0;

        private void UpdateData()
        {
            _saveLoadService.GameData.UnlockedLevels = _unlockedLevels;
            _saveLoadService.GameData.CompleatedLevels = _compleatedLevels;

            _saveLoadService.Update(_saveLoadService.GameData);
        }
    }
}