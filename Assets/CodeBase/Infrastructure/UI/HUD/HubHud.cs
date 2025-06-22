using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.UI.HUD
{
    public class HubHud : MonoBehaviour, IHUD
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        
        public UniTask ActivateAsync()
        {
            return UniTask.CompletedTask;
        }
    }
}