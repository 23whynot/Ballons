using System;
using CodeBase.GamePlay.Controllers.AvatarsController;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.GamePlay.UI
{
    public class AvatarPicker : MonoBehaviour
    {
        [SerializeField] private Button chooseBtn;
        [SerializeField] private Image avatarImage;

        private IAvatarController _avatarController;

        [Inject]
        public void Construct(IAvatarController avatarController) => 
            _avatarController = avatarController;

        private void OnEnable()
        {
            avatarImage.sprite = _avatarController.GetAvatar();
        }

        private async void Start()
        {
            chooseBtn.onClick.AddListener(PickFromGallery);

            await _avatarController.InitializeAsync();
            
            var sprite = _avatarController.GetAvatar();
            if (sprite != null)
                avatarImage.sprite = sprite;
        }

        private void PickFromGallery()
        {
            if (Application.isEditor) return;

            NativeGallery.GetImageFromGallery((path) =>
            {
                if (string.IsNullOrEmpty(path)) return;

                Texture2D tex = NativeGallery.LoadImageAtPath(path);
                if (tex == null) return;

                _avatarController.SetAvatar(tex);
            }, "Выберите фото", "image/*");
        }
    }
}