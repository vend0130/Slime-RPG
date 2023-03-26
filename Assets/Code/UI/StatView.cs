using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class StatView : MonoBehaviour
    {
        [field: SerializeField] public Button EnhanceButton { get; private set; }
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _statNameText;
        [SerializeField] private TextMeshProUGUI _currentStatNumberText;
        [SerializeField] private TextMeshProUGUI _coinsText;
        [SerializeField] private GameObject _lockButton;

        public void Init(string statName)
        {
            _statNameText.text = statName.ToUpper();
        }

        public void UpdateDate(string levelText, string number, string coins, bool isLock)
        {
            _levelText.text = $"Lv {levelText}";
            _currentStatNumberText.text = number;
            _coinsText.text = coins;

            _lockButton.SetActive(isLock);
        }
    }
}