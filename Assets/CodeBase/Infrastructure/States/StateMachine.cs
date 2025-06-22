using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.States
{
    public class StateMachine
    {
        private readonly Dictionary<Type, IExitableState> _registeredStates;
        private IExitableState _currentState;

        public StateMachine() =>
            _registeredStates = new Dictionary<Type, IExitableState>();

        public async UniTask Enter<TState>() where TState : class, IState
        {
            TState newState = await ChangeState<TState>();
            await newState.Enter();
        }

        public void RegisterState<TState>(TState state) where TState : IExitableState =>
            _registeredStates.Add(typeof(TState), state);


        private async UniTask<TState> ChangeState<TState>() where TState : class, IExitableState
        {
            if (_currentState != null)
                await _currentState.Exit();

            TState state = GetState<TState>();
            _currentState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState =>
            _registeredStates[typeof(TState)] as TState;
    }
}