using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class HPBar : MonoBehaviour
    {
        [SerializeField] private Image _currentImage;

        public void SetValue(float current, float max) =>
            _currentImage.fillAmount = current / max;
    }
}