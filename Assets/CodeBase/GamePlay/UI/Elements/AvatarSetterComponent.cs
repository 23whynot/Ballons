using CodeBase.GamePlay.Controllers.AvatarsController;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.GamePlay.UI.Elements
{
    public class AvatarSetterComponent : MonoBehaviour
    {
        [SerializeField] private Image avatar;
        private IAvatarController _avatarController;

        [Inject]
        public void Construct(IAvatarController avatarController) => 
            _avatarController = avatarController;

        private void Awake()
        {
            avatar.sprite = _avatarController.GetAvatar();
            _avatarController.OnAvatarChanged += ChangeAvatar;
        }

        private void ChangeAvatar(Sprite obj) => 
            avatar.sprite = obj;
        
        private void OnDestroy() => 
            _avatarController.OnAvatarChanged -= ChangeAvatar;
    }
}