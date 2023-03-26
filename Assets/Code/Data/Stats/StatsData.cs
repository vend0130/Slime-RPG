using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = nameof(StatsData), menuName = "Static Data/Stats/" + nameof(StatsData))]
    public class StatsData : ScriptableObject
    {
        [field: SerializeField] public AttackStat AttackStat { get; private set; }
    }
}