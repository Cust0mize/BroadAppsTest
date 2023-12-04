using Game.Scripts.UI.Panels.Customization;
using Game.Scripts.UI.Panels.Achivment;
using Game.Scripts.Signal;
using UnityEngine.UI;
using UnityEngine;
using Zenject;
using TMPro;
using Game.Scripts.UI.Panels.Record;

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
            _balanceValueTextUI.text = signalMoneyUpdate.NewMoneyValue.ToString();
        }

        private void SubscribeButtons() {
            _customizationButton.RemoveAllAndSubscribeButton(CustomizationClick);
            _achievementsButton.RemoveAllAndSubscribeButton(AchievementsClick);
            _startGameButton.RemoveAllAndSubscribeButton(StartGameClick);
            _recordButton.RemoveAllAndSubscribeButton(RecordClick);
            _moreButton.RemoveAllAndSubscribeButton(MoreClick);
        }

        private void AchievementsClick() {
            UIService.OpenPanel<AchivmentPanel>();
        }

        private void CustomizationClick() {
            UIService.OpenPanel<CustomizationPanel>();
        }

        private void StartGameClick() {
            UIService.OpenPanel<ChangeGameModePanel>();
        }

        private void RecordClick() {
            UIService.OpenPanel<RecordPanel>();
        }

        private void MoreClick() {

        }
    }
}

public interface ILoadableElement {
    public int Order { get; }
    public void Load();
}
