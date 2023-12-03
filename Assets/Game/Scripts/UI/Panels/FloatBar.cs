using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Game.Scripts.UI.Panels {
    public class FloatBar : MonoBehaviour {
        [SerializeField] private Image _fillImageValue;
        [SerializeField] private TextMeshProUGUI _startValueTextUI;
        [SerializeField] private TextMeshProUGUI _endValueTextUI;
        private float _startValue;
        private float _endValue;

        public void UpdateBar(float startValue, float endValue, float currentValue) {
            SetStartValue(startValue);
            SetEndValue(endValue);
            SetCurrentValue(currentValue);
        }

        public void UpdateStartValue(float newValue, float targetValue) {
            if (newValue > targetValue) {
                newValue = targetValue;
            }

            _startValueTextUI.text = $"{Mathf.Abs(newValue):f2}";
        }

        public void SetCurrentValue(float currentValue) {
            float first = _endValue - _startValue;
            float second = currentValue - _startValue;
            float precent = (float)second / first;
            _fillImageValue.fillAmount = precent;
        }

        private void SetStartValue(float newValue) {
            _startValue = newValue;
            _startValueTextUI.text = $"{Mathf.Abs(_startValue):f2}";
        }

        private void SetEndValue(float newValue) {
            _endValue = newValue;
            _endValueTextUI.text = $"{Mathf.Abs(_endValue):f2}";
        }
    }
}
