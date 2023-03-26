using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class StatsUI : MonoBehaviour
    {
        [field: SerializeField] public Transform Parent { get; private set; }
        [SerializeField] private ScrollRect _scroll;

        public void Reset() =>
            _scroll.enabled = false;
    }
}