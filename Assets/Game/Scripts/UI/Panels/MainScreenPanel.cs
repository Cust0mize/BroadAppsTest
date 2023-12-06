using Game.Scripts.UI.Panels.Customization;
using Game.Scripts.UI.Panels.Achivment;
using Game.Scripts.UI.Panels.Record;
using Game.Scripts.Signal;
using UnityEngine.UI;
using UnityEngine;
using Zenject;
using TMPro;

namespace Game.Scripts.UI.Panels {
    public class MainScreenPanel : UIPanel, ILoadableElement {
        [SerializeField] private TextMeshProUGUI _balanceValueTextUI;
        [SerializeField] private TextMeshProUGUI _levelNumberTextUI;
        [SerializeField] private Button _customizationButton;
        [SerializeField] private Button _achievementsButton;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Sprite[] _levelIcons;
        [SerializeField] private Button _recordButton;
        [SerializeField] private Button _moreButton;
        [SerializeField] private Image _levelImage;
        [SerializeField] private Bar _levelBar;

        public int Order => 2;

        [Inject]
        private void Construct(SignalBus signalBus) {
            signalBus.Subscribe<SignalUpdateLevel>(UpdateBar);
            signalBus.Subscribe<SignalUpdateLevel>(UpdateLevel);
            signalBus.Subscribe<SignalMoneyUpdate>(BalanceUpdate);
        }

        public void Load() {
            SubscribeButtons();
        }

        private void UpdateLevel(SignalUpdateLevel signalUpdateLevel) {
            _levelImage.sprite = _levelIcons[signalUpdateLevel.NewLevelIndex];
            _levelNumberTextUI.text = (signalUpdateLevel.NewLevelIndex + 1).ToString();
        }

        private void UpdateBar(SignalUpdateLevel signalUpdateLevel) {
            _levelBar.UpdateBar(signalUpdateLevel.NewStartValue, signalUpdateLevel.NewEndValue, signalUpdateLevel.NewLevelProgressValue);
        }

        private void BalanceUpdate(SignalMoneyUpdate signalMoneyUpdate) {
            _balanceValueTextUI.text = $"${signalMoneyUpdate.NewMoneyValue:f2}";
        }

        private void SubscribeButtons() {
            _customizationButton.RemoveAllAndSubscribeButton(() => UIService.OpenPanel<CustomizationPanel>());
            _startGameButton.RemoveAllAndSubscribeButton(() => UIService.OpenPanel<ChangeGameModePanel>());
            _achievementsButton.RemoveAllAndSubscribeButton(() => UIService.OpenPanel<AchivmentPanel>());
            _recordButton.RemoveAllAndSubscribeButton(() => UIService.OpenPanel<RecordPanel>());
            _moreButton.RemoveAllAndSubscribeButton(() => UIService.OpenPanel<MorePanel>());
        }
    }
}

public interface ILoadableElement {
    public int Order { get; }
    public void Load();
}
