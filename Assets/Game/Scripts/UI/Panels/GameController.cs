using Game.Scripts.Game.GameValue;
using Game.Scripts.Game.Gamemodes;
using Game.Scripts.Signal;
using Game.Scripts.Game;
using UnityEngine;
using Zenject;
using TMPro;
using Enums;

namespace Game.Scripts.UI.Panels {
    public class GameController : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _downMultiplyTextUI;
        [SerializeField] private TextMeshProUGUI _endSityCityTextUI;
        [SerializeField] private TextMeshProUGUI _playerNameTextUI;
        [SerializeField] private TextMeshProUGUI _multiplyTextUI;
        [SerializeField] private TextMeshProUGUI _startCity;
        [SerializeField] private float maxValue = 0.85f;
        [SerializeField] private float minValue = 0.1f;
        [SerializeField] Player _player;

        private GameData _gameData;
        private RouteController _routeController;

        private BaseGame _baseGame;
        private TripGame _tripGame;
        private TwoPersonGame _twoPersonGame;
        private ClassicGame _classicGame;

        private TwoPersonModeController _twoPersonModeController;
        [SerializeField] private AnimationCurve _animationCurve;

        [Inject]
        private void Construct(
        TwoPersonModeController twoPersonModeController,
        RouteController routeController,
        TwoPersonGame twoPersonGame,
        ClassicGame classicGame,
        SignalBus signalBus,
        TripGame tripGame,
        GameData gameData
        ) {
            _twoPersonModeController = twoPersonModeController;
            _routeController = routeController;
            _twoPersonGame = twoPersonGame;
            _classicGame = classicGame;
            _tripGame = tripGame;
            _gameData = gameData;
            signalBus.Subscribe<SignalStartGame>(StartGame);
            signalBus.Subscribe<OpenGamePanel>(UpdateUI);
            signalBus.Subscribe<SignalStopGame>(StopGame);
        }

        private void Start() {
            SetMultipleTextState(false);
            _player.UpdatePosition(0.1f);
        }

        private void Update() {
            if (_baseGame != null && _baseGame.IsStartGame) {
                Game();
            }
        }

        private void Game() {
            UIUpdate();

            switch (_gameData.CurrentGamemode) {
                case Gamemode.Classic: ClassicGame(); break;
                case Gamemode.Trip: TripGame(); break;
                case Gamemode.Task: break;
                case Gamemode.Two: TwoPersonGame(); break;
            }
        }

        private void TwoPersonGame() {
            _twoPersonGame.GameUpdate();
            _downMultiplyTextUI.text = _twoPersonGame.CurrentTimeSpan.ToString(@"mm\:ss");
        }

        private void TripGame() {
            _tripGame.GameUpdate();
            _downMultiplyTextUI.text = $"{_tripGame.DownMultiplyValue:f2} km";
        }

        private void ClassicGame() {
            _classicGame.GameUpdate();
        }

        private void StopGame(SignalStopGame signalStopGame) {
            _baseGame.StopGame(signalStopGame);
            _player.UpdatePosition(0.1f);
            SetMultipleTextState(false);
            _playerNameTextUI.text = $"Player {_twoPersonGame.CurrentPlayerIndex}\n{_twoPersonModeController.SecondPersonName}";
        }

        private void UIUpdate() {
            _player.UpdatePosition(_baseGame.MultiplyAnimationCurve.Evaluate(_baseGame.StartTime));
            _multiplyTextUI.text = $"{_baseGame.CurrentMultiply:f2}X";
        }

        private void StartGame() {
            SetMultipleTextState(true);
            _baseGame.StartGame();
            _animationCurve = _baseGame.MultiplyAnimationCurve;
        }

        private void UpdateUI() {
            switch (_gameData.CurrentGamemode) {
                case Gamemode.Classic:
                    SetStateUI(Gamemode.Classic);
                    _classicGame.SetStartValue(new BaseStartGameValue());
                    _baseGame = _classicGame;
                    break;
                case Gamemode.Trip:
                    SetStateUI(Gamemode.Trip);
                    _tripGame.SetStartValue(new StartTripValue(_routeController.CurrentSelectedRouteConfig.RouteDistance));
                    _baseGame = _tripGame;
                    _startCity.text = _routeController.CurrentSelectedRouteConfig.StartCity;
                    _endSityCityTextUI.text = _routeController.CurrentSelectedRouteConfig.EndSityCity;
                    break;
                case Gamemode.Task: break;
                case Gamemode.Two:
                    SetStateUI(Gamemode.Two);
                    _twoPersonGame.SetStartValue(new StartTwoGameValue(_twoPersonModeController.TargetGameTime, 2));
                    _baseGame = _twoPersonGame;
                    _playerNameTextUI.text = $"Player {_twoPersonGame.CurrentPlayerIndex}\n{_twoPersonModeController.FirstPersonName}";
                    break;
            }
        }

        private void SetStateUI(Gamemode gamemode) {
            _endSityCityTextUI.gameObject.SetActive(gamemode == Gamemode.Trip);
            _playerNameTextUI.gameObject.SetActive(gamemode == Gamemode.Two);
            _startCity.gameObject.SetActive(gamemode == Gamemode.Trip);
        }

        private void SetMultipleTextState(bool state) {
            _downMultiplyTextUI.gameObject.SetActive(
            _gameData.CurrentGamemode == Gamemode.Trip && state ||
            _gameData.CurrentGamemode == Gamemode.Two && state);
            _multiplyTextUI.gameObject.SetActive(state);
        }
    }
}
