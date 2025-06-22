using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.GamePlay.Controllers.AvatarsController
{
    public interface IAvatarController
    {
        event Action<Sprite> OnAvatarChanged;
        UniTask InitializeAsync();
        Sprite GetAvatar();
        void SetAvatar(Texture2D texture);
    }
}