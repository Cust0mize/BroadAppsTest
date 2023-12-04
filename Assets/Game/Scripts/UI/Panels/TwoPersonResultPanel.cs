using System.Collections.Generic;
using Game.Scripts.Signal;
using UnityEngine.UI;
using UnityEngine;
using Zenject;

namespace Game.Scripts.UI.Panels {
    public class TwoPersonResultPanel : UIPanel {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _goOutButton;

        private NameAndRewardItem[] _nameAndRewardItems;
        private ResultIconItem[] _resultIconItems;

        [Inject]
        private void Construct(
            SignalBus signalBus
        ) {
            signalBus.Subscribe<SignalUpdateResultTwoGame>(UpdateResult);
        }

        private void Start() {
            _restartButton.RemoveAllAndSubscribeButton(RestartClick);
            _goOutButton.RemoveAllAndSubscribeButton(GoUotClick);
        }

        private void GoUotClick() {
            UIService.HideAllPanels(new List<UIPanel>() { UIService.GetPanel<MainScreenPanel>() });
        }

        private void RestartClick() {
            UIService.HideAllPanels(new List<UIPanel>() { UIService.GetPanel<MainScreenPanel>() });
            UIService.OpenPanel<GamePanel>();
        }

        private void UpdateResult(SignalUpdateResultTwoGame signalUpdateResultTwoGame) {
            if (_resultIconItems == null) {
                _nameAndRewardItems = transform.GetComponentsInChildren<NameAndRewardItem>(true);
                _resultIconItems = transform.GetComponentsInChildren<ResultIconItem>(true);
            }

            for (int i = 0; i < _nameAndRewardItems.Length; i++) {
                _nameAndRewardItems[i].Init(signalUpdateResultTwoGame.Names[i], signalUpdateResultTwoGame.Rewards[i]);
            }

            for (int i = 0; i < _resultIconItems.Length; i++) {
                _resultIconItems[i].Init(signalUpdateResultTwoGame.Names[i], signalUpdateResultTwoGame.WinPlayerIndex == i);
            }
        }
    }
}