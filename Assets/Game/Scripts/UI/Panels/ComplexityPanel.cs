using UnityEngine.UI;
using UnityEngine;
using System;

namespace Game.Scripts.UI.Panels {
    public class ComplexityPanel : UIPanel {
        [SerializeField] private Button _startGameButton;
        private ComplexityButton[] _complexities;
        private event Action _updateSelection;

        private void Start() {
            _startGameButton.RemoveAllAndSubscribeButton(StartGameClick);
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
            UIService.HidePanelBypassStack<ComplexityPanel>();
            UIService.OpenPanel<GamePanel>();
        }
    }

    public enum Complexity {
        Easy,
        Average,
        Hard,
        Extreme,
    }
}