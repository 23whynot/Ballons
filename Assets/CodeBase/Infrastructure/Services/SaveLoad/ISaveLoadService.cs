using System.Collections.Generic;
using CodeBase.Infrastructure.Services.SaveLoad.Data;
using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.Services.SaveLoad
{
    public interface ISaveLoadService
    {
        public UniTask InitializeAsync();
        
        public GameData GameData { get;  }
        
        public void Save();

        public void Update(GameData gameData);
    }
}