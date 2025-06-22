using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CodeBase.Infrastructure.Services.SaveLoad.Data
{
    [Serializable]
    public class GameData
    {
        [JsonProperty] public string PlayerName { get; set; }
        [JsonProperty] public string AvatarPath { get; set; }

        [JsonProperty] public bool IsFirstLaunch { get; set; } = true;
        
        [JsonProperty] public int GemsAmount { get; set; }
        
        [JsonProperty] public List<string> BuyingBalloons { get; set; } = new List<string>();
        [JsonProperty] public List<int> UnlockedLevels { get; set; } = new();
        
        [JsonProperty] public Dictionary<int, int> CompleatedLevels { get; set; } = new();
    }
}