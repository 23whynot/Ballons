using System.Collections.Generic;
using System.Linq;
using CodeBase.GamePlay.UI.Animation;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.SaveLoad;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.GamePlay.Ballon.Controller
{
    public class BuyingBalloonController : IBuyingBalloonController
    {
        private ISaveLoadService _saveLoadService;
        private IAssetProvider _assetProvider;
        
        private BalloonConfigArray _ballonConfigs;

        [Inject]
        public void Construct(ISaveLoadService saveLoadService,
            IAssetProvider assetProvider)
        {
            _saveLoadService = saveLoadService;
            _assetProvider = assetProvider;
        }

        public async UniTask InitializeAsync()
        {
            _ballonConfigs = await _assetProvider.Load<BalloonConfigArray>(AssetPath.BalloonConfigArray);

            if (_saveLoadService.GameData.IsFirstLaunch)
            {
                MarkBalloonBuying(_ballonConfigs.ballonConfigs[0].Icon.name);
            }
        }

        public BalloonConfigArray GetBalloonConfigArray() => 
            _ballonConfigs;

        public BalloonConfig GetRandomBoughtBalloonConfig()
        {
            if (_ballonConfigs == null || _ballonConfigs.ballonConfigs == null)
                return null;

            var boughtConfigs = _ballonConfigs.ballonConfigs
                .Where(config => config != null &&
                                 config.Icon != null &&
                                 _saveLoadService.GameData.BuyingBalloons.Contains(config.Icon.name))
                .ToList();

            if (boughtConfigs.Count == 0)
                return null;

            int randomIndex = Random.Range(0, boughtConfigs.Count);
            return boughtConfigs[randomIndex];
        }

        public bool IsBuyingBalloon(string id) => 
            _saveLoadService.GameData.BuyingBalloons.Contains(id);

        public void MarkBalloonBuying(string id)
        {
            _saveLoadService.GameData.BuyingBalloons.Add(id);
            _saveLoadService.Update(_saveLoadService.GameData);
        }
    }
}