using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

namespace Game.Scripts.UI.Panels {
    public class ComplexityPanel : UIPanel {
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _backButton;
        private ComplexityButton[] _complexities;
        private event Action _updateSelection;

        private void Start() {
            _startGameButton.RemoveAllAndSubscribeButton(StartGameClick);
            _backButton.RemoveAllAndSubscribeButton(() => UIService.HidePanelBypassStack<ComplexityPanel>());
            _updateSelection += StartUpdateSelection;
            _complexities = transform.GetComponentsInChildren<ComplexityButton>();
            for (int i = 0; i < _complexities.Length; i++) {
                _complexities[i].Init(_updateSelection);
            }
        }

        private void StartUpdateSelection() {
            for (int i = 0; i < _complexities.Length; i++) {
                _complexities[i].UpdateSelection();
            }
        }

        private void OnDestroy() {
            _updateSelection -= StartUpdateSelection;
        }

        private void StartGameClick() {
            UIService.OpenPanel<GamePanel>();
            UIService.HidePanelBypassStack<ComplexityPanel>();
        }
    }
}