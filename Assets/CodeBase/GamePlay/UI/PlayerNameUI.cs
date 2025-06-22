using CodeBase.Infrastructure.Services.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.GamePlay.UI
{
    public class PlayerNameUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button saveButton;

        private IPlayerNameService _playerNameService;

        [Inject]
        public void Construct(IPlayerNameService playerNameService) => 
            _playerNameService = playerNameService;

        private void Start()
        {
            inputField.text = _playerNameService.GetName();
            saveButton.onClick.AddListener(OnSaveClicked);
        }

        private void OnSaveClicked()
        {
            string newName = inputField.text;
            _playerNameService.SetNewName(newName);
        }

        private void OnDestroy() => 
            saveButton.onClick.RemoveAllListeners();
    }
}