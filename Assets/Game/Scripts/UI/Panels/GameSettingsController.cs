using UnityEngine;
using System;
using TMPro;

namespace Game.Scripts.UI.Panels {
    public class GameSettingsController : MonoBehaviour {
        [SerializeField] private TMP_InputField _multiplyField;
        [SerializeField] private TMP_InputField _amountField;
        private Action<MultiplyButton> _multiplyButtonClick;
        MultiplyButton[] _multiplyButtons;

        private void Start() {
            _multiplyButtons = transform.GetComponentsInChildren<MultiplyButton>();
            _multiplyField.onEndEdit.AddListener(EndMultyplyEdit);
            _amountField.onEndEdit.AddListener(EndAmountEdit);
            _multiplyButtonClick += UpdateSelectionMultiplyButton;
            for (int i = 0; i < _multiplyButtons.Length; i++) {
                _multiplyButtons[i].Init(_multiplyButtonClick);
            }
        }

        private void EndMultyplyEdit(string text) {
            _multiplyField.text = $"{_multiplyField.text:f1}X";
        }

        private void EndAmountEdit(string text) {

        }

        private void UpdateSelectionMultiplyButton(MultiplyButton multiplyButton) {
            _multiplyField.text = $"{multiplyButton.Multiply:f1}X";
            for (int i = 0; i < _multiplyButtons.Length; i++) {
                if (_multiplyButtons[i].Multiply == multiplyButton.Multiply) {
                    _multiplyButtons[i].Select();
                }
                else {
                    _multiplyButtons[i].Unselect();
                }
            }
        }

        public void ParceValue(out float multiply, out float amount) {
            float.TryParse(_multiplyField.text, out float parceMultiply);
            multiply = parceMultiply < 1.5f ? 1.5f : parceMultiply;
            float.TryParse(_amountField.text, out float parceAmount);
            amount = parceAmount < 100 ? 100 : parceAmount;
            UpdateField(multiply, amount);
        }

        private void UpdateField(float multiply, float amount) {
            _multiplyField.text = $"{multiply:f1}X";
            _amountField.text = $"${amount}";
        }

        private void OnDestroy() {
            _multiplyButtonClick -= UpdateSelectionMultiplyButton;
        }
    }
}