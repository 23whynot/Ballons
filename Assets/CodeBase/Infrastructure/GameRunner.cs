using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        private GameBootstrapper.Factory _bootstrapperFactory;

        [Inject]
        public void Construct(GameBootstrapper.Factory bootstrapperFactory) => 
            _bootstrapperFactory = bootstrapperFactory;

        private void Awake()
        {
            GameBootstrapper bootstrapper = FindObjectOfType<GameBootstrapper>();
            
            if (bootstrapper != null)
            {
                return;
            }
            
            GameBootstrapper obj = _bootstrapperFactory.Create();
            obj.transform.SetParent(null);
        }
    }
}
