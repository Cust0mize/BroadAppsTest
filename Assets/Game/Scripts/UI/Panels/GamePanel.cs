using UnityEngine.UI;
using UnityEngine;
using Zenject;
using System;
using TMPro;

namespace Game.Scripts.UI.Panels {
    public class GamePanel : UIPanel {
        [SerializeField] private TMP_InputField _multiplyField;
        [SerializeField] private TextMeshProUGUI _balanceTextUI;
        [SerializeField] private TMP_InputField _amountField;
        [SerializeField] private TextMeshProUGUI _levelTextUI;
        [SerializeField] Button _exitButton;
        MultiplyButton[] _multiplyButtons;
        private Action<MultiplyButton> _multiplyButtonClick;
        private LevelService _levelService;

        [Inject]
        private void Construct(SignalBus signalBus) {
            signalBus.Subscribe<SignalUpdateLevel>(UpdateLevelNumber);
        }

        public void UpdateLevelNumber(SignalUpdateLevel signalUpdateLevel) {
            _levelTextUI.text = (signalUpdateLevel.NewLevelIndex + 1).ToString();
        }

        public void UpdateBalanceValue() {
            _balanceTextUI.text = 0.ToString();
        }

        private void Start() {
            UpdateBalanceValue();
            _exitButton.onClick.AddListener(() => UIService.HidePanelBypassStack<GamePanel>());
            _multiplyButtons = transform.GetComponentsInChildren<MultiplyButton>();
            _multiplyButtonClick += UpdateSelectionMultiplyButton;
            for (int i = 0; i < _multiplyButtons.Length; i++) {
                _multiplyButtons[i].Init(_multiplyButtonClick);
            }

            _multiplyField.onEndEdit.AddListener(EndMultyplyEdit);
            _amountField.onEndEdit.AddListener(EndAmountEdit);
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

        private void OnDestroy() {
            _multiplyButtonClick -= UpdateSelectionMultiplyButton;
        }
    }
}