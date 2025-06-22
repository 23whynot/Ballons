using System;
using System.IO;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.Services.SaveLoad.Data;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.GamePlay.Controllers.AvatarsController
{
    public class AvatarController : IAvatarController
    {
        private ISaveLoadService _saveLoadService;
        private Sprite _currentAvatar;

        [Inject]
        public void Construct(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        public event Action<Sprite> OnAvatarChanged;

        public async UniTask InitializeAsync()
        {
            var data = _saveLoadService.GameData;
            if (data == null)
            {
                Debug.LogError("GameData is null in AvatarController.InitializeAsync()");
                return;
            }

            if (!string.IsNullOrEmpty(data.AvatarPath))
            {
                string fullPath = Path.Combine(Application.persistentDataPath, data.AvatarPath);
                if (File.Exists(fullPath))
                {
                    byte[] bytes = await UniTask.Run(() => File.ReadAllBytes(fullPath));
                    Texture2D tex = new Texture2D(2, 2);
                    tex.LoadImage(bytes);
                    _currentAvatar = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
                    _currentAvatar.name = Path.GetFileNameWithoutExtension(fullPath);
                    OnAvatarChanged?.Invoke(_currentAvatar);
                }
                else
                {
                    Debug.LogWarning($"Avatar file not found at path: {fullPath}");
                }
            }
        }

        public void SetAvatar(Texture2D texture)
        {
            Texture2D readable = MakeTextureReadable(texture);
            Texture2D cropped = CropCenterSquare(readable, 1024);

            _currentAvatar = Sprite.Create(cropped, new Rect(0, 0, cropped.width, cropped.height), Vector2.one * 0.5f);
            SaveAvatarToDisk(cropped);
            OnAvatarChanged?.Invoke(_currentAvatar);
        }


        private Texture2D CropCenterSquare(Texture2D source, int size)
        {
            int cropSize = Mathf.Min(size, Mathf.Min(source.width, source.height));

            int x = (source.width - cropSize) / 2;
            int y = (source.height - cropSize) / 2;

            Color[] pixels = source.GetPixels(x, y, cropSize, cropSize);

            Texture2D result = new Texture2D(cropSize, cropSize);
            result.SetPixels(pixels);
            result.Apply();

            return result;
        }
        
        private Texture2D MakeTextureReadable(Texture2D texture)
        {
            RenderTexture rt = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            Graphics.Blit(texture, rt);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = rt;

            Texture2D readableTexture = new Texture2D(texture.width, texture.height);
            readableTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            readableTexture.Apply();

            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(rt);

            return readableTexture;
        }
        
        private void SaveAvatarToDisk(Texture2D texture)
        {
            string filename = $"avatar_{Guid.NewGuid()}.png";
            string fullPath = Path.Combine(Application.persistentDataPath, filename);
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(fullPath, bytes);

            GameData data = _saveLoadService.GameData;
            data.AvatarPath = filename; // Сохраняем только имя файла
            _saveLoadService.Update(data);

            _currentAvatar.name = Path.GetFileNameWithoutExtension(filename);
        }

        public Sprite GetAvatar() =>
            _currentAvatar;
    }
}
