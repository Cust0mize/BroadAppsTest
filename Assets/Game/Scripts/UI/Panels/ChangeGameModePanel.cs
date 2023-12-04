using UnityEngine.UI;
using UnityEngine;
using Zenject;
using Enums;

namespace Game.Scripts.UI.Panels {
    public class ChangeGameModePanel : UIPanel {
        [SerializeField] private Button _classicTwoPersonGame;
        [SerializeField] private Button _classicWithATaskGame;
        [SerializeField] private Button _classicGame;
        [SerializeField] private Button _hideButton;
        [SerializeField] private Button _tripGame;
        private GameData _gameData;

        [Inject]
        private void Construct(GameData gameData) {
            _gameData = gameData;
        }

        private void Start() {
            _classicTwoPersonGame.RemoveAllAndSubscribeButton(TwoGameClick);
            _classicWithATaskGame.RemoveAllAndSubscribeButton(TaskGameClick);
            _classicGame.RemoveAllAndSubscribeButton(ClassicClick);
            _hideButton.RemoveAllAndSubscribeButton(HideClick);
            _tripGame.RemoveAllAndSubscribeButton(TripClick);
        }

        private void ClassicClick() {
            SelectGamemode(Gamemode.Classic);
        }

        private void TripClick() {
            SelectGamemode(Gamemode.Trip);
        }

        private void TaskGameClick() {
            SelectGamemode(Gamemode.Task);
        }

        private void TwoGameClick() {
            SelectGamemode(Gamemode.Two);
        }

        private void HideClick() {
            UIService.HidePanelBypassStack<ChangeGameModePanel>();
        }

        private void SelectGamemode(Gamemode gamemode) {
            _gameData.CurrentGamemode = gamemode;
            UIService.HidePanelBypassStack<ChangeGameModePanel>();
            UIService.OpenPanel<ComplexityPanel>();
        }
    }
}

