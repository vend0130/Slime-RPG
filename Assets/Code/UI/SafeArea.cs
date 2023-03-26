using UnityEngine;

namespace Code.UI
{
    public class SafeArea : MonoBehaviour
    {
        [SerializeField] private RectTransform _safeArea;

        private void Awake()
        {
            var safeAre = Screen.safeArea;
            var anchorMin = safeAre.position;
            var anchorMax = anchorMin + safeAre.size;

            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            _safeArea.anchorMax = anchorMax;
        }
    }
}