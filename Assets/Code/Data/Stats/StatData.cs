using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = nameof(StatData), menuName = "Static Data/Stats/" + nameof(StatData))]
    public class StatData : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; } = "ATTACK";
        [field: SerializeField] public float DefaultNumber { get; private set; } = 25;
        [field: SerializeField] public int DefaultPrice { get; private set; } = 100;
        [field: SerializeField] public float EnhanceNumber { get; private set; } = 100;
        [field: SerializeField] public int EnhancePrice { get; private set; } = 200;
        [field: SerializeField] public int MaxNumber { get; private set; } = 10;
        [field: SerializeField] public bool IsPecents { get; private set; } = false;
    }
}