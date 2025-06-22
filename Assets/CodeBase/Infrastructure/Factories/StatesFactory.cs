using CodeBase.Infrastructure.States;
using Zenject;

namespace CodeBase.Infrastructure.Factories
{
    public class StateFactory : IStateFactory
    {
        private IInstantiator _instantiator;

        [Inject]
        public void Construct(IInstantiator instantiator) => 
            _instantiator = instantiator;

        public TState Create<TState>() where TState : class, IExitableState => 
            _instantiator.Instantiate<TState>();
    }
}