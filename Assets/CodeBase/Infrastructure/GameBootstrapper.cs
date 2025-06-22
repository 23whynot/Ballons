using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.States;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour
    {
        private IGameStateMachine _gameStateMachine;
        private IStateFactory _stateFactory;

        [Inject]
        public void Construct(IGameStateMachine gameStateMachine, 
            IStateFactory stateFactory)
        {
            _gameStateMachine = gameStateMachine;
            _stateFactory = stateFactory;
        }


        private void Awake()
        {
            _gameStateMachine.RegisterState(_stateFactory.Create<GameBootstrapState>());
            _gameStateMachine.RegisterState(_stateFactory.Create<GameHubState>());
            _gameStateMachine.RegisterState(_stateFactory.Create<GameplayState>());

            _gameStateMachine.Enter<GameBootstrapState>();
            
            DontDestroyOnLoad(this);
        }

        public class Factory : PlaceholderFactory<GameBootstrapper>
        {
        }
    }
}
