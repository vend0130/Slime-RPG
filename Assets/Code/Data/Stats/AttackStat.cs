using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = nameof(AttackStat), menuName = "Static Data/Stats/" + nameof(AttackStat))]
    public class AttackStat : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; } = "ATTACK";
        [field: SerializeField] public float DefaultAttack { get; private set; } = 25;
        [field: SerializeField] public int DefaultPrice { get; private set; } = 100;
        [field: SerializeField] public float EnhanceAttack { get; private set; } = 100;
        [field: SerializeField] public int EnhancePrice { get; private set; } = 200;
        [field: SerializeField] public int MaxLevel { get; private set; } = 10;
    }
}