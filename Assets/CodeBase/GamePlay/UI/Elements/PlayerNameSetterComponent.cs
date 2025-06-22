using CodeBase.Infrastructure.Services.Player;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.GamePlay.UI.Elements
{
    public class PlayerNameSetterComponent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerName;
        private IPlayerNameService _playerNameService;

        [Inject]
        public void Construct(IPlayerNameService playerNameService) => 
            _playerNameService = playerNameService;

        private void Awake()
        {
            playerName.text = _playerNameService.GetName();
            _playerNameService.OnSetNewName += UpdateName;
        }

        private void UpdateName(string obj) => 
            playerName.text = obj;

        private void OnDestroy() => 
            _playerNameService.OnSetNewName -= UpdateName;
    }
}