using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/Level")]
public class LevelConfig : ScriptableObject
{
    public int Time;
    public int Prize;
    public int LevelID;
}