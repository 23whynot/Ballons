using System;
using System.Collections.Generic;
using CodeBase.GamePlay.UI.Windows;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factories;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.UI.Window
{
    public class WindowManager : IWindowManager
    {
        private Dictionary<string, IWindow> _windowInstance = new Dictionary<string, IWindow>();
        private IWindow _activeWindowOnGui;
        private IWindow _activeWindowOnHud;
        private IWindow _secondActiveWindowOnHud;

        private GameObject _gui;
        private GameObject _hud;

        private IAssetProvider _assetProvider;
        private DiContainer _container;
        private IUIFactory _uiFactory;

        private Queue<string> _windowQueue = new Queue<string>(); 
        
        [Inject]
        public void Construct(IAssetProvider assetProvider,
            DiContainer container,
            IUIFactory uiFactory)
        {
            _assetProvider = assetProvider;
            _container = container;
            _uiFactory = uiFactory;
        }

        public async UniTask InitializeAsync()
        {
            _gui = _uiFactory.GetGUI();
            _hud = _uiFactory.GetHUD();
        }
        
        public void AddWindowToQueueOnGui(string windowID)
        {
            if (!_windowQueue.Contains(windowID))
            {
                _windowQueue.Enqueue(windowID);
            }
        }
        
        public async UniTask ProcessQueueOnGui()
        {
            while (_windowQueue.Count > 0)
            {
                string nextWindowID = _windowQueue.Dequeue();

                await OpenWindowAsyncOnGui(nextWindowID);
                
                await UniTask.WaitUntil(() => _activeWindowOnGui == null);
            }
        }


        public async UniTask OpenWindowAsyncOnGui(string windowID, Action callback = null)
        {
            if (_windowInstance.TryGetValue(windowID, out IWindow window))
            {
                await OpenOnGui(window, callback);
            }
            else
            {
                IWindow newWindow = await InstantiateWindowOnGuiAsync(windowID);

                await OpenOnGui(newWindow, callback);
            }
        }

        public async UniTask OpenInitializableWindowOnHudAsync<T>(string windowID, T parameter,
            bool closeCurrentWindow = false)
        {
            if (_windowInstance.TryGetValue(windowID, out IWindow window))
            {
                InitializeWindow(parameter, window);

                if (closeCurrentWindow)
                {
                    await OpenOnHud(window);
                }
                else
                {
                    await OpenWithoutClosingCurrentOnHud(windowID);
                }
            }
            else
            {
                IWindow newWindow = await InstantiateWindowOnHudAsync(windowID);
                InitializeWindow(parameter, newWindow);

                if (closeCurrentWindow)
                {
                    await OpenOnHud(newWindow);
                }
                else
                {
                    await OpenWithoutClosingCurrentOnHud(windowID);
                }
            }
        }

        public async UniTask OpenInitializableWindowOnGuiAsync<T>(string windowID, T parameter)
        {
            if (_windowInstance.TryGetValue(windowID, out IWindow window))
            {
                InitializeWindow(parameter, window);
                await OpenOnGui(window);
            }
            else
            {
                IWindow newWindow = await InstantiateWindowOnGuiAsync(windowID);
                InitializeWindow(parameter, newWindow);


                await OpenOnGui(newWindow);
            }
        }

        private static void InitializeWindow<T>(T parameter, IWindow newWindow)
        {
            GameObject x = newWindow.GetGameObject();
            IInitializableWindow<T> initializer = x.GetComponent<IInitializableWindow<T>>();
            initializer.Initialize(parameter);
        }

       
        

        public async UniTask OpenWithoutClosingCurrentOnHud(string windowID)
        {
            if (_windowInstance.TryGetValue(windowID, out IWindow window))
            {
                await OpenOnHudWitchOutClosing(window);
            }
            else
            {
                IWindow newWindow = await InstantiateWindowOnHudAsync(windowID);
                await OpenOnHudWitchOutClosing(newWindow);
            }
        }

        public async UniTask CloseWindowByIdOnHud(string windowID)
        {
            if (_windowInstance.TryGetValue(windowID, out IWindow window))
            {
                await CloseWindowOnHud(window);
            }
        }

        public async UniTask CloseCurrentWindowAsyncOnGui() =>
            await TryCloseCurrentWindowOnGui();

        private async UniTask OpenOnHudWitchOutClosing(IWindow window)
        {
            _hud.SetActive(true);

            _secondActiveWindowOnHud = window;

            window.BeforeOpen();

            window.Open();

            window.AfterOpen();
        }

        private async UniTask OpenOnGui(IWindow window, Action callback = null)
        {
            _gui.SetActive(true);

            await TryCloseCurrentWindowOnGui();

            _activeWindowOnGui = window;

            await window.BeforeOpenAsync();

            window.BeforeOpen();

            await window.OpenAsync();

            window.Open();
            
            await window.AfterOpenAsync();

            window.AfterOpen();
            
            callback?.Invoke();
        }

        private async UniTask<IWindow> InstantiateWindowOnGuiAsync(string windowID)
        {
            GameObject loadedWindow = await _assetProvider.Load<GameObject>(windowID);
            GameObject instantiatedWindow = _container.InstantiatePrefab(loadedWindow);
            instantiatedWindow.gameObject.SetActive(false);

            instantiatedWindow.transform.SetParent(_gui.transform, false);
            instantiatedWindow.transform.SetSiblingIndex(0);

            IWindow newWindow = instantiatedWindow.GetComponent<IWindow>();

            if (!_windowInstance.ContainsKey(windowID))
            {
                _windowInstance.Add(windowID, newWindow);
            }

            return newWindow;
        }

        private async UniTask TryCloseCurrentWindowOnGui()
        {
            if (_activeWindowOnGui != null)
            {
                await _activeWindowOnGui.BeforeCloseAsync();
                _activeWindowOnGui.BeforeClose();
                await _activeWindowOnGui.CloseAsync();
                await _activeWindowOnGui.AfterCloseAsync();
                _activeWindowOnGui.AfterClose();
            }

            _activeWindowOnGui = null;
        }
        
        public async UniTask SwitchWindowOnGuiAsync(string newWindowID, Action callback = null)
        {
            IWindow newWindow;

            if (_windowInstance.TryGetValue(newWindowID, out IWindow existingWindow))
            {
                newWindow = existingWindow;
            }
            else
            {
                newWindow = await InstantiateWindowOnGuiAsync(newWindowID);
            }

            _gui.SetActive(true);

            // Сохраняем старое окно
            IWindow previousWindow = _activeWindowOnGui;
            _activeWindowOnGui = newWindow;

            // Параллельно закрываем старое и подготавливаем новое
            await UniTask.WhenAll(
                previousWindow != null ? CloseWindowAsync(previousWindow) : UniTask.CompletedTask,
                PrepareAndOpenWindowAsync(newWindow)
            );

            callback?.Invoke();
        }

        private async UniTask CloseWindowAsync(IWindow window)
        {
            await window.BeforeCloseAsync();
            window.BeforeClose();
            await window.CloseAsync();
            await window.AfterCloseAsync();
            window.AfterClose();
        }

        private async UniTask PrepareAndOpenWindowAsync(IWindow window)
        {
            await window.BeforeOpenAsync();
            window.BeforeOpen();
            await window.OpenAsync();
            window.Open();
            await window.AfterOpenAsync();
            window.AfterOpen();
        }

        public async UniTask OpenWindowAsyncOnHUD(string windowID)
        {
            if (_windowInstance.TryGetValue(windowID, out IWindow window))
            {
                await OpenOnHud(window);
            }
            else
            {
                IWindow newWindow = await InstantiateWindowOnHudAsync(windowID);
                await OpenOnHud(newWindow);
            }
        }

        private async UniTask<IWindow> InstantiateWindowOnHudAsync(string windowID)
        {
            GameObject loadedWindow = await _assetProvider.Load<GameObject>(windowID);
            GameObject instantiatedWindow = _container.InstantiatePrefab(loadedWindow);
            instantiatedWindow.gameObject.SetActive(false);

            instantiatedWindow.transform.SetParent(_hud.transform, false);

            IWindow newWindow = instantiatedWindow.GetComponent<IWindow>();

            if (!_windowInstance.ContainsKey(windowID))
            {
                _windowInstance.Add(windowID, newWindow);
            }

            return newWindow;
        }

        private async UniTask OpenOnHud(IWindow window)
        {
            _hud.SetActive(true);

            await TryCloseCurrentWindowOnHud();

            _activeWindowOnHud = window;

            await window.BeforeOpenAsync();

            window.BeforeOpen();

            await window.OpenAsync();

            window.Open();
            
            await window.AfterOpenAsync();

            window.AfterOpen();
        }

        public async UniTask CloseCurrentWindowAsyncOnHud() =>
            await TryCloseCurrentWindowOnHud();

        private async UniTask TryCloseCurrentWindowOnHud()
        {
            if (_activeWindowOnHud != null)
            {
                _activeWindowOnHud.BeforeClose();
                await _activeWindowOnHud.CloseAsync();
                _activeWindowOnHud.AfterClose();
            }

            _activeWindowOnHud = null;
        }

        private async UniTask CloseWindowOnHud(IWindow window)
        {
            if (_activeWindowOnHud == window)
            {
                _activeWindowOnHud = null;
            }

            await window.BeforeCloseAsync();
            window.BeforeClose();
            await window.CloseAsync();
            await window.AfterCloseAsync();
            window.AfterClose();
        }
    }
}
