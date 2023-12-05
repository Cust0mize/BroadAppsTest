using Game.Scripts.Signal;
using UnityEngine.UI;
using UnityEngine;
using Zenject;

namespace Game.Scripts.UI.Panels {
    public class ConfirmRemoveBalancePanel : UIPanel {
        [SerializeField] private Button[] _hideButtons;
        [SerializeField] private Button _resetButton;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus) {
            _signalBus = signalBus;
        }

        private void Start() {
            for (int i = 0; i < _hideButtons.Length; i++) {
                _hideButtons[i].RemoveAllAndSubscribeButton(() => UIService.HidePanelBypassStack<ConfirmRemoveBalancePanel>());
            }
            _resetButton.RemoveAllAndSubscribeButton(() => _signalBus.Fire(new SignalResetGame()));
        }
    }
}