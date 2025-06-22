using CodeBase.Infrastructure.States;

namespace CodeBase.Infrastructure.Factories
{
    public interface IStateFactory
    {
        public TState Create<TState>() where TState : class, IExitableState;
    }
}