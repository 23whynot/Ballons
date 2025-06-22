using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListLevelConfig", menuName = "Configs/Level List")]
public class ListLevelConfig : ScriptableObject
{
    public List<LevelConfig> LevelConfigs;
}