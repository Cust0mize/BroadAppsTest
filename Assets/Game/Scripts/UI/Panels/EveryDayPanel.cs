using Game.Scripts.Game;
using UnityEngine.UI;
using UnityEngine;
using Zenject;
using TMPro;

namespace Game.Scripts.UI.Panels {
    public class EveryDayPanel : UIPanel {
        [SerializeField] private DayCounterElement _dayCounterElement;
        [SerializeField] private TextMeshProUGUI _dayCounterTextUI;
        [SerializeField] private Button _takeRewardButton;

        private CurrenciesService _currenciesService;
        private GameData _gameData;

        [Inject]
        private void Construct(
            CurrenciesService currenciesService,
            GameData gameData
            ) {
            _currenciesService = currenciesService;
            _gameData = gameData;
        }

        public override void Show() {
            base.Show();
            _takeRewardButton.RemoveAllAndSubscribeButton(TakeRewardClick);
            _dayCounterTextUI.text = $"{_gameData.DayIndex} consecutive days";
            _dayCounterElement.Init();
        }

        private void TakeRewardClick() {
            _currenciesService.AddMoney(100);
            UIService.HidePanelBypassStack<EveryDayPanel>();
        }
    }
}
