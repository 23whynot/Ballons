using System.Collections.Generic;

namespace CodeBase.GamePlay.Services.NickName
{
    public interface INicknameService
    {
        public List<string> GetEnemyNicknames(int count);
        string GetPlayerName();
    }
}