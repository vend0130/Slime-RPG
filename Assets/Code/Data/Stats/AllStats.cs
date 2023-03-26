using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = nameof(AllStats), menuName = "Static Data/Stats/" + nameof(AllStats))]
    public class AllStats : ScriptableObject
    {
        [field: SerializeField] public StatData Attack { get; private set; }
        [field: SerializeField] public StatData HP { get; private set; }
    }
}