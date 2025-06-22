using System;
using CodeBase.GamePlay.Level.Controller;
using CodeBase.GamePlay.Level.Holder;

using CodeBase.Infrastructure.States;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.GamePlay.ChoiseLevel
{
    public class LevelUI : MonoBehaviour
    {
        [SerializeField] private StarControllerComponent starController;
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image lockImage;
        
        private LevelConfig _level;
        private IGameStateMachine _stateMachine;
        private ILevelHolder _levelHolder;
        private ILevelDataController _levelDataController;
        private bool _isInitilized;

        [Inject]
        public void Construct(IGameStateMachine stateMachine,
            ILevelHolder levelHolder,
            ILevelDataController levelDataController)
        {
            _levelHolder = levelHolder;
            _stateMachine = stateMachine;
            _levelDataController = levelDataController;
        }

        private void Awake() => 
            button.onClick.AddListener(StartGame);

        private void OnEnable()
        {
            if (!_isInitilized)
                return;
            
            InitLevelLockAndStars();
        }

        private void InitLevelLockAndStars()
        {
            if (_levelDataController.IsLevelUnlocked(_level.LevelID))
            {
                lockImage.gameObject.SetActive(false);
                text.gameObject.SetActive(true);
                button.interactable = true;
            }
            else
            {
                text.gameObject.SetActive(false);
                lockImage.gameObject.SetActive(true);
                button.interactable = false;
            }
            
            starController.ShowStars(_levelDataController.GetStarsCount(_level.LevelID));
        }

        public void Initialize(int index, LevelConfig level)
        {
            text.text = index.ToString();
            _level = level;
            
            _isInitilized = true;
            
            InitLevelLockAndStars();
        }
        
        private void StartGame()
        {
            _levelHolder.SetConfig(_level);
            _stateMachine.Enter<GameplayState>().Forget();
        }

        private void OnDestroy() => 
            button.onClick.RemoveAllListeners();
    }
}