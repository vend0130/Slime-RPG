using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = nameof(HeroDefaultData), menuName = "Static Data/" + nameof(HeroDefaultData))]
    public class HeroDefaultData : ScriptableObject
    {
        [field: SerializeField] public float Damage { get; private set; } = 25;
        [field: SerializeField] public float HP { get; private set; } = 100;
        [field: SerializeField] public float AttackSpeed { get; private set; } = 100;
    }
}