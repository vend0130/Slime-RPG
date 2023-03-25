using UnityEngine;

namespace Code.Data.Level
{
    [CreateAssetMenu(fileName = nameof(LevelConfig), menuName = "Static Data/" + nameof(LevelConfig))]
    public class LevelConfig : ScriptableObject
    {
        [field: SerializeField] public WaveData[] Waves { get; private set; }
    }
}