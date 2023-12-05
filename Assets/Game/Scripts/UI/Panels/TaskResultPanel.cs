using System.Collections.Generic;
using Game.Scripts.Signal;
using Game.Scripts.Game;
using UnityEngine.UI;
using UnityEngine;
using Zenject;
using TMPro;

namespace Game.Scripts.UI.Panels {
    public class TaskResultPanel : UIPanel {
        [SerializeField] private TextMeshProUGUI _resultValueTextUI;
        [SerializeField] private TextMeshProUGUI _yourBidTextUI;
        [SerializeField] private TextMeshProUGUI _resultTextUI;
        [SerializeField] private TextMeshProUGUI _buttonTextUI;
        [SerializeField] private Sprite[] _resultSprites;
        [SerializeField] private Image _resultImage;
        [SerializeField] private Button _upButton;
        private float _rewardValue;
        private bool _isWin;

        private CurrenciesService _currenciesService;

        [Inject]
        private void Construct(
            CurrenciesService currenciesService,
            SignalBus signalBus
        ) {
            _currenciesService = currenciesService;
            signalBus.Subscribe<SignalEndTaskGame>(UpdateResult);
        }

        private void Start() {
            _upButton.RemoveAllAndSubscribeButton(UpButtonClick);
        }

        private void UpButtonClick() {
            if (_isWin) {
                _currenciesService.AddMoney(_rewardValue);
                UIService.HideAllPanels(new List<UIPanel>() { UIService.GetPanel<MainScreenPanel>() });
            }
            else {
                UIService.HideAllPanels(new List<UIPanel>() { UIService.GetPanel<MainScreenPanel>() });
                UIService.OpenPanel<GamePanel>();
            }
        }

        private void UpdateResult(SignalEndTaskGame signalEndTaskGame) {
            _rewardValue = signalEndTaskGame.RewardValue;
            _isWin = signalEndTaskGame.IsWin;
            _yourBidTextUI.text = $"Your bid: ${_rewardValue:f2}";
            _resultValueTextUI.text = $"Result: ${signalEndTaskGame.ResultValue:f2}";
            _resultImage.sprite = _isWin ? _resultSprites[0] : _resultSprites[1];
            _resultTextUI.text = _isWin ? "Victory!" : "Lose";
            _buttonTextUI.text = _isWin ? $"Pick up award +${signalEndTaskGame.RewardValue:f2}" : "Restart";
        }
    }
}