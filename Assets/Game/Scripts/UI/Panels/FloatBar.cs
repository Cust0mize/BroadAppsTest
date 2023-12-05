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
            newValue = Mathf.Abs(newValue);
            if (newValue > targetValue) {
                newValue = targetValue;
            }

            _startValueTextUI.text = $"{newValue:f2}";
        }

        public void SetCurrentValue(float currentValue) {
            currentValue = Mathf.Abs(currentValue);
            float first = _endValue - _startValue;
            float second = currentValue - _startValue;
            float precent = (float)second / first;
            _fillImageValue.fillAmount = precent;
        }

        private void SetStartValue(float newValue) {
            newValue = Mathf.Abs(newValue);
            _startValue = newValue;
            _startValueTextUI.text = $"{_startValue:f2}";
        }

        private void SetEndValue(float newValue) {
            newValue = Mathf.Abs(newValue);
            _endValue = newValue;
            _endValueTextUI.text = $"{_endValue:f2}";
        }
    }
}
