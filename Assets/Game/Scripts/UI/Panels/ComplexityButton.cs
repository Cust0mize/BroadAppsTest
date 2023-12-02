using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts.UI.Panels {
    [RequireComponent(typeof(Button))]
    public class ComplexityButton : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _buttonNameTextUI;
        [SerializeField] private Complexity _complexity;
        [SerializeField] private Image _selectionImage;
        private Button _button;
        private GameData _gameData;
        private Action _updateSelection;

        [Inject]
        private void Construct(GameData gameData) {
            _gameData = gameData;
        }

        public void Init(Action myEvent) {
            _updateSelection = myEvent;
            if (!_button) {
                _button = gameObject.GetComponent<Button>();
                _button.RemoveAllAndSubscribeButton(ButtonClick);
            }
            _buttonNameTextUI.text = _complexity.ToString();
            UpdateSelection();
        }

        public void UpdateSelection() {
            if (_complexity == _gameData.CurrentComplexity) {
                _selectionImage.gameObject.SetActive(true);
            }
            else {
                _selectionImage.gameObject.SetActive(false);
            }
        }

        private void ButtonClick() {
            _gameData.CurrentComplexity = _complexity;
            _updateSelection?.Invoke();
        }
    }
}