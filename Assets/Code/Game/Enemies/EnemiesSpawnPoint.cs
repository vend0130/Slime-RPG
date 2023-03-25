using UnityEngine;

namespace Code.Game.Enemies
{
    public class EnemiesSpawnPoint : MonoBehaviour
    {
        [field: SerializeField] public Transform DefaultSpawnPoint { get; private set; }
        [field: SerializeField] public float XPointRange { get; private set; } = .3f;
        [field: SerializeField] public float ZPointRange { get; private set; } = 3;
        [field: SerializeField] public float DistanceBetweenWave { get; private set; } = 10;
    }
}