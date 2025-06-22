using System;
using CodeBase.GamePlay.Services.NickName;
using CodeBase.Infrastructure.Services.SaveLoad;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CodeBase.Infrastructure.Services.Player
{
    public class PlayerNameService : IPlayerNameService
    {
        private string _currentName;
        private ISaveLoadService _saveLoadService;
        private INicknameService _nicknameService;

        public event Action<string> OnSetNewName;

        [Inject]
        public void Construct(ISaveLoadService saveLoadService, 
            INicknameService nicknameService)
        {
            _saveLoadService = saveLoadService;
            _nicknameService = nicknameService;
        }

        public async UniTask InitializeAsync()
        {
            _currentName = _saveLoadService.GameData.PlayerName;

            if (_currentName == null)
            {
                _currentName = _nicknameService.GetPlayerName();
                UpdateData();
            }
        }


        public void SetNewName(string newName)
        {
            _currentName = newName;
            OnSetNewName?.Invoke(newName);

            UpdateData();
        }

        public string GetName() => 
            _currentName;

        private void UpdateData()
        {
            var data = _saveLoadService.GameData;
            data.PlayerName = _currentName;
            
            _saveLoadService.Update(data);
        }
    }
}