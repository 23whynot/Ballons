namespace CodeBase.Infrastructure.ObjPool
{
    public interface IPoolableObject
    {
        public bool IsActive {get; }
        public void Activate();
        public void Deactivate();
    }
}