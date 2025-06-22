using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.UI.Window
{
    public interface IWindow
    {
        GameObject GetGameObject();
        
        public void BeforeClose();
        
        public UniTask BeforeCloseAsync();

        public void AfterClose();
        
        public UniTask CloseAsync();

        public UniTask AfterCloseAsync();

        public void BeforeOpen();

        public UniTask BeforeOpenAsync();

        public void AfterOpen();

        public UniTask AfterOpenAsync();

        public void Open();

        public UniTask OpenAsync();
    }
}