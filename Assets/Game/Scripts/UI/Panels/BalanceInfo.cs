using Game.Scripts.Signal;
using UnityEngine;
using Zenject;
using TMPro;

namespace Game.Scripts.UI.Panels {
    public class BalanceInfo : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _balanceTextUI;

        [Inject]
        private void Construct(SignalBus signalBus) {
            signalBus.Subscribe<SignalMoneyUpdate>(UpdateBalanceValue);
        }

        public void UpdateBalanceValue(SignalMoneyUpdate signalMoneyUpdate) {
            string result = $"${signalMoneyUpdate.NewMoneyValue:f2}";
            _balanceTextUI.text = result;
        }
    }
}
