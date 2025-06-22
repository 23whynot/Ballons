using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.AssetManagement;
using UnityEngine;
using Zenject;

namespace CodeBase.GamePlay.ChoiseLevel
{
    public class LevelsInitializerComponent : MonoBehaviour
    {
        [SerializeField] private LevelStringUI levelStringUIPrefab;
        [SerializeField] private Transform container;

        private IAssetProvider _assetProvider;
        private DiContainer _container;

        [Inject]
        public void Construct(IAssetProvider assetProvider, DiContainer container)
        {
            _container = container;
            _assetProvider = assetProvider;
        }

        private async void Awake()
        {
            ListLevelConfig listConfig = await _assetProvider.Load<ListLevelConfig>(AssetPath.ListLevelConfig);
            List<LevelConfig> levels = listConfig.LevelConfigs;

            int totalLevels = levels.Count;
            int levelsPerRow = 3;
            int index = 1;

            for (int i = 0; i < totalLevels; i += levelsPerRow)
            {
                // Создание строки
                LevelStringUI levelString = _container.InstantiatePrefabForComponent<LevelStringUI>(
                    levelStringUIPrefab, container);

                LevelUI[] levelUIs = levelString.Levels;

                for (int j = 0; j < levelsPerRow; j++)
                {
                    int levelIndex = i + j;
                    if (levelIndex >= totalLevels)
                    {
                        levelUIs[j].gameObject.SetActive(false);
                        continue;
                    }

                    levelUIs[j].Initialize(index, levels[levelIndex]);
                    index++;
                }
            }
        }
    }
}