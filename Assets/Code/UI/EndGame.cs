using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class EndGame : MonoBehaviour
    {
        [field: SerializeField] public Button AgainButton { get; private set; }
        [SerializeField] private TextMeshProUGUI _text;

        public void ChangeText(string text) =>
            _text.text = text;
    }
}