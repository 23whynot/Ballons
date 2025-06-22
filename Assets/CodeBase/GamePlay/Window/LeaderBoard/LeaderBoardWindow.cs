using System.Collections.Generic;
using System.Linq;
using CodeBase.GamePlay.ChoiseLevel;
using CodeBase.GamePlay.Controllers.AvatarsController;
using CodeBase.GamePlay.Level.Controller;
using CodeBase.GamePlay.Level.Holder;
using CodeBase.GamePlay.Score;
using CodeBase.Infrastructure.UI.Window;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace CodeBase.GamePlay.Window.LeaderBoard
{
    public class LeaderBoardWindow : MonoBehaviour, IWindow
    {
        [SerializeField] private StarControllerComponent starController;
        [SerializeField] private CanvasGroup canvas;
        [SerializeField] private LeaderBoardPlace place;
        [SerializeField] private Transform content;

        private readonly List<LeaderBoardPlace> _createdPlates = new();
        private const int PlateCount = 4;

        private Tween _tween;
        private IScoreController _scoreController;
        private IAvatarController _avatarController;
        private ILevelDataController _levelDataController;
        private ILevelHolder _levelHolder;
        private int starCount;

        [Inject]
        public void Construct(IScoreController scoreController,
            IAvatarController avatarController,
            ILevelDataController levelDataController,
            ILevelHolder levelHolder)
        {
            _levelDataController = levelDataController;
            _avatarController = avatarController;
            _scoreController = scoreController;
            _levelHolder = levelHolder;
        }

        public GameObject GetGameObject() => gameObject;

        public void BeforeOpen()
        {
            starCount = Random.Range(1, 4);
            
            starController.ShowStars(starCount);
            canvas.alpha = 0f;
            gameObject.SetActive(true);
            InitializeLeaderboard();
        }

        public UniTask BeforeOpenAsync() => UniTask.CompletedTask;
        public UniTask AfterOpenAsync() => UniTask.CompletedTask;
        public UniTask AfterCloseAsync() => UniTask.CompletedTask;
        public UniTask BeforeCloseAsync() => UniTask.CompletedTask;
        public UniTask OpenAsync() => UniTask.CompletedTask;

        public void Open()
        {
            _tween?.Kill();
            _tween = canvas.DOFade(1f, 0.5f).OnComplete(() => _tween.Kill());
        }

        public void AfterOpen()
        {
            _levelDataController.MarkLevelAsUnlocked(_levelHolder.GetConfig().LevelID+1);
            _levelDataController.MarkLevelAsComplete(_levelHolder.GetConfig().LevelID, starCount);
        }

        public void BeforeClose() { }

        public async UniTask CloseAsync()
        {
            _tween?.Kill();
            _tween = canvas.DOFade(0f, 0.5f).OnComplete(() => _tween.Kill());
        }

        public void AfterClose()
        {
            gameObject.SetActive(false);
        }

        private void InitializeLeaderboard()
        {
            int playerIndex = Random.Range(0, PlateCount);
            int playerScore = _scoreController.GetScore();
            Sprite playerAvatar = _avatarController.GetAvatar();

            var scores = GenerateScores(playerIndex, playerScore);
            var sorted = SortScoresDescending(scores, playerIndex);

            for (int i = 0; i < sorted.Count; i++)
            {
                LeaderBoardPlace plate = GetOrCreatePlate(i);
                bool isPlayer = sorted[i].Index == playerIndex;
                string name = isPlayer ? "You" : $"Bot #{i + 1}";
                int score = sorted[i].Score;

                plate.Initialize(name, score, isPlayer);
                if (isPlayer)
                    plate.SetAvatar(playerAvatar);
            }

            DeactivateUnusedPlates(sorted.Count);
        }

        private List<int> GenerateScores(int playerIndex, int playerScore)
        {
            var scores = new List<int>();

            for (int i = 0; i < PlateCount; i++)
            {
                if (i == playerIndex)
                {
                    scores.Add(playerScore);
                    continue;
                }

                int randomScore = i < playerIndex
                    ? Random.Range(playerScore + 1, playerScore + 1000)
                    : Random.Range(Mathf.Max(0, playerScore - 1000), playerScore);

                scores.Add(randomScore);
            }

            return scores;
        }

        private List<(int Score, int Index)> SortScoresDescending(List<int> scores, int playerIndex)
        {
            return scores
                .Select((score, index) => (Score: score, Index: index))
                .OrderByDescending(item => item.Score)
                .ToList();
        }

        private LeaderBoardPlace GetOrCreatePlate(int index)
        {
            if (index < _createdPlates.Count)
            {
                var plate = _createdPlates[index];
                plate.gameObject.SetActive(true);
                return plate;
            }

            var newPlate = Instantiate(place, content);
            _createdPlates.Add(newPlate);
            return newPlate;
        }

        private void DeactivateUnusedPlates(int activeCount)
        {
            for (int i = activeCount; i < _createdPlates.Count; i++)
                _createdPlates[i].gameObject.SetActive(false);
        }
    }
}
