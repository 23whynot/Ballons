using System.Collections.Generic;
using CodeBase.Infrastructure.AssetManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.ObjPool
{
    public abstract class ObjectPool<T> : IObjectPool<T> where T : class, IPoolableObject
    {
        private readonly Dictionary<string, List<GameObject>> _instantiatedObject = new();
        private readonly Dictionary<string, GameObject> _prefabs = new();
        private readonly DiContainer _container;
        private readonly IAssetProvider _assetsProvider;

        public ObjectPool(DiContainer container, IAssetProvider assetsProvider)
        {
            _container = container;
            _assetsProvider = assetsProvider;
        }

        public virtual async UniTask<T> GetFreeObject(string id)
        {
            if (_instantiatedObject.TryGetValue(id, out var listOfUsedObjects))
            {
                foreach (GameObject obj in listOfUsedObjects)
                {
                    var poolableObj = obj.GetComponent<IPoolableObject>();

                    if (!poolableObj.IsActive)
                    {
                        poolableObj.Activate();
                        return obj.GetComponent<T>();
                    }
                }
            }

            return await CreateNewObject(id);
        }

        private async UniTask<T> CreateNewObject(string id)
        {
            GameObject prefab;

            if (_prefabs.TryGetValue(id, out var cachedPrefab))
            {
                prefab = cachedPrefab;
            }
            else
            {
                prefab = await _assetsProvider.Load<GameObject>(id);
                _prefabs[id] = prefab;
            }

            GameObject instantiatedObj = _container.InstantiatePrefab(prefab);

            var poolableObj = instantiatedObj.GetComponent<IPoolableObject>();
            poolableObj?.Activate();

            if (!_instantiatedObject.ContainsKey(id))
                _instantiatedObject[id] = new List<GameObject>();

            _instantiatedObject[id].Add(instantiatedObj);
            return instantiatedObj.GetComponent<T>();
        }
    }
}