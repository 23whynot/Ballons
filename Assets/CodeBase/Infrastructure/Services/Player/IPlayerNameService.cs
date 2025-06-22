using System;
using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.Services.Player
{
    public interface IPlayerNameService
    {
        public UniTask InitializeAsync();
        
        public event Action<string> OnSetNewName; 
        void SetNewName(string newName);
        string GetName();
    }
}