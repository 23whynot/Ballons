using System;
using System.Collections.Generic;

namespace CodeBase.GamePlay.Services.NickName
{
    public class NicknameService : INicknameService
    {
        private static readonly List<string> _baseNicknames = new List<string>
        {
            "CoolPlayer", "StarRunner", "BlockMaster", "GemHunter", "PuzzleKing",
            "BlockNinja", "CrystalPro", "GameWizard", "MasterMind", "SpeedSolver",
            "ProGamer", "BlockChamp", "PuzzlePro", "StarPlayer", "GemMaster", "Whynot"
        };
        
        private static readonly string[] _playerNames = new string[]

        {
            "Liam Parker", "Noah Bennett", "Ethan Sullivan", "Mason Cooper", "Lucas Thompson", 
            "Oliver Mitchell", "Aiden Campbell", "Elijah Foster", "James Harrison", 
            "Benjamin Morgan", "William Brooks", "Alexander Reed", "Michael Carter", 
            "Daniel Murphy"
        };

        public List<string> GetEnemyNicknames(int count)
        {
            var result = new List<string>();
            var rnd = new Random();

            if (count > _baseNicknames.Count)
                throw new ArgumentException("Запрошено больше ников, чем доступно уникальных.");

            while (result.Count < count)
            {
                var nickname = _baseNicknames[rnd.Next(_baseNicknames.Count)];
                if (!result.Contains(nickname))
                {
                    result.Add(nickname);
                }
            }

            return result;
        }

        public string GetPlayerName() => 
            _playerNames[UnityEngine.Random.Range(0, _playerNames.Length)];
    }
}
