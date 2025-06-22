using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.ObjPool
{
    public interface IObjectPool<T>
    {
        UniTask<T> GetFreeObject(string id);
    }
}