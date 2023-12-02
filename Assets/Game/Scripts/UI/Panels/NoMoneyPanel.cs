using Game.Scripts.Signal;
using UnityEngine.UI;
using UnityEngine;
using Zenject;

namespace Game.Scripts.UI.Panels {
    public class NoMoneyPanel : UIPanel {
        [SerializeField] private Button _resetButton;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus) {
            _signalBus = signalBus;
            _signalBus.Subscribe<SignalNoMoney>(UIService.OpenPanel<NoMoneyPanel>);
        }

        private void Start() {
            _resetButton.RemoveAllAndSubscribeButton(ResetClick);
        }

        private void ResetClick() {
            _signalBus.Fire<SignalResetGame>();
        }
    }
}
