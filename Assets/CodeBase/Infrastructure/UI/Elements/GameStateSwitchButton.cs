using System;
using CodeBase.Infrastructure.States;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.Infrastructure.UI.Elements
{
    public class GameStateSwitchButton : MonoBehaviour
    {
        [SerializeField] private GameStateType gameStateType;
        [SerializeField] private Button button;
        
        private IGameStateMachine _gameStateMachine;

        [Inject]
        public void Construct(IGameStateMachine gameStateMachine) => 
            _gameStateMachine = gameStateMachine;

        private void Awake() => 
            button.onClick.AddListener(SwitchGameState);

        private void SwitchGameState()
        {
            switch (gameStateType)
            {
                case GameStateType.GameHUB:
                    _gameStateMachine.Enter<GameHubState>().Forget();
                    break;
                case GameStateType.GamePlay:
                    _gameStateMachine.Enter<GameplayState>().Forget();
                    break;
                default:
                    Debug.LogError($"{gameStateType} is not a game state");
                    break;
            }
        }

        private void OnDestroy() => 
            button.onClick.RemoveListener(SwitchGameState);
    }

    public enum GameStateType
    {
        None = 1,
        GameHUB = 2,
        GamePlay = 3,
    }
}