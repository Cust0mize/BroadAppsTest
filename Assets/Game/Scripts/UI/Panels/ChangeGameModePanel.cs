using UnityEngine.UI;
using UnityEngine;
using Zenject;
using Enums;

namespace Game.Scripts.UI.Panels {
    public class ChangeGameModePanel : UIPanel {
        [SerializeField] private Button _classicTwoPersonGame;
        [SerializeField] private Button _classicWithATaskGame;
        [SerializeField] private Button _classicGame;
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
            _tripGame.RemoveAllAndSubscribeButton(TripClick);
        }

        private void ClassicClick() {
            SelectGamemode<ComplexityPanel>(Gamemode.Classic);
        }

        private void TripClick() {
            SelectGamemode<TripModePanel>(Gamemode.Trip);
        }

        private void TaskGameClick() {
            SelectGamemode<TaskGameModePanel>(Gamemode.Task);
        }

        private void TwoGameClick() {
            SelectGamemode<TwoModePanel>(Gamemode.Two);
        }

        private void SelectGamemode<T>(Gamemode gamemode) where T : UIPanel {
            _gameData.CurrentGamemode = gamemode;
            UIService.HidePanelBypassStack<ChangeGameModePanel>();
            UIService.OpenPanel<T>();
        }
    }
}