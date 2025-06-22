using System;
using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.UI.Window
{
    public interface IWindowManager
    {
        UniTask OpenInitializableWindowOnHudAsync<T>(string windowID, T parameter, bool closeCurrentWindow = false);

        UniTask SwitchWindowOnGuiAsync(string newWindowID, Action callback = null);
        
        UniTask OpenInitializableWindowOnGuiAsync<T>(string windowID, T parameter);

        UniTask InitializeAsync();
        
        UniTask ProcessQueueOnGui();

        void AddWindowToQueueOnGui(string windowID);
        
        UniTask OpenWithoutClosingCurrentOnHud(string windowID);
        
        UniTask CloseWindowByIdOnHud(string windowId);

        UniTask OpenWindowAsyncOnHUD(string windowID);
        
        UniTask CloseCurrentWindowAsyncOnHud();
        
        UniTask OpenWindowAsyncOnGui(string windowID, Action callback = null);
        
        UniTask CloseCurrentWindowAsyncOnGui();
    }
}