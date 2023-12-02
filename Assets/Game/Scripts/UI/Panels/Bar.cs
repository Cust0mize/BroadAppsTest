using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Game.Scripts.UI.Panels {
    public class Bar : MonoBehaviour {
        [SerializeField] private Image _fillImageValue;
        [SerializeField] private TextMeshProUGUI _startValueTextUI;
        [SerializeField] private TextMeshProUGUI _endValueTextUI;
        private int _startValue;
        private int _endValue;

        public void UpdateBar(int startValue, int endValue, int currentValue) {
            SetStartValue(startValue);
            SetEndValue(endValue);
            SetCurrentValue(currentValue);
        }

        public void SetCurrentValue(int currentValue) {
            int first = _endValue - _startValue;
            int second = currentValue - _startValue;
            float precent = Mathf.Abs((float)second / first);
            _fillImageValue.fillAmount = precent;
        }

        private void SetStartValue(int newValue) {
            _startValue = newValue;
            _startValueTextUI.text = _startValue.ToString();
        }

        private void SetEndValue(int newValue) {
            _endValue = newValue;
            _endValueTextUI.text = _endValue.ToString();
        }
    }
}
