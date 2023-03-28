using UnityEngine;

namespace Code.Data.Stats
{
    [CreateAssetMenu(fileName = nameof(AllStats), menuName = "Static Data/Stats/" + nameof(AllStats))]
    public class AllStats : ScriptableObject
    {
        [field: SerializeField] public StatData Attack { get; private set; }
        [field: SerializeField] public StatData HP { get; private set; }
        [field: SerializeField] public StatData ASPD { get; private set; }
        [field: SerializeField] public StatData CriticalChance { get; private set; }
        [field: SerializeField] public StatData CriticalHitDamage { get; private set; }
    }
}