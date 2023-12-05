using Game.Scripts.Game.Gamemodes;
using System.Collections.Generic;
using Game.Scripts.Signal;
using Game.Scripts.Game;
using System.Linq;
using UnityEngine;
using Zenject;
using TMPro;
using Enums;

namespace Game.Scripts.UI.Panels {
    public class GameController : MonoBehaviour {
        [SerializeField] private AnimationCurve _animationCurve;
        [SerializeField] private TextMeshProUGUI _downMultiplyTextUI;
        [SerializeField] private TextMeshProUGUI _endSityCityTextUI;
        [SerializeField] private TextMeshProUGUI _playerNameTextUI;
        [SerializeField] private TextMeshProUGUI _multiplyTextUI;
        [SerializeField] private TextMeshProUGUI _startCity;
        [SerializeField] private float maxValue = 0.85f;
        [SerializeField] private float minValue = 0.1f;
        [SerializeField] Player _player;

        private TwoPersonModeController _twoPersonModeController;
        private RouteController _routeController;

        private IEnumerable<BaseGame> _allGames;
        private GameData _gameData;
        private BaseGame _baseGame;

        [Inject]
        private void Construct(
        TwoPersonModeController twoPersonModeController,
        IEnumerable<BaseGame> _baseGames,
        RouteController routeController,
        SignalBus signalBus,
        GameData gameData
        ) {
            _twoPersonModeController = twoPersonModeController;
            _routeController = routeController;
            _gameData = gameData;
            signalBus.Subscribe<SignalDownMultipleUpdate>(DownMultipleUpdate);
            signalBus.Subscribe<OpenGamePanel>(OpenGameWindow);
            signalBus.Subscribe<SignalStartGame>(StartGame);
            signalBus.Subscribe<SignalStopGame>(StopGame);

            _allGames = _baseGames;
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
            _baseGame.GameUpdate();
        }

        private void DownMultipleUpdate(SignalDownMultipleUpdate signalDownMultipleUpdate) {
            _downMultiplyTextUI.text = signalDownMultipleUpdate.Text;
        }

        private void StopGame(SignalStopGame signalStopGame) {
            _baseGame.StopGame(signalStopGame);
            _player.UpdatePosition(0.1f);
            SetMultipleTextState(false);
            _playerNameTextUI.text = $"Player {2}\n{_twoPersonModeController.SecondPersonName}";
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

        private void OpenGameWindow() {
            UpdateUI();
            SetStateUI(_gameData.CurrentGamemode);
            _baseGame = _allGames.FirstOrDefault(x => x.GameMode == _gameData.CurrentGamemode);
            _baseGame.SetStartValue();
        }

        private void UpdateUI() {
            switch (_gameData.CurrentGamemode) {
                case Gamemode.Trip:
                    _startCity.text = _routeController.CurrentSelectedRouteConfig.StartCity;
                    _endSityCityTextUI.text = _routeController.CurrentSelectedRouteConfig.EndSityCity;
                    break;
                case Gamemode.Two:
                    _playerNameTextUI.text = $"Player {1}\n{_twoPersonModeController.FirstPersonName}";
                    break;
            }
        }

        private void SetStateUI(Gamemode gamemode) {
            _endSityCityTextUI.gameObject.SetActive(gamemode == Gamemode.Trip);
            _playerNameTextUI.gameObject.SetActive(gamemode == Gamemode.Two);
            _startCity.gameObject.SetActive(gamemode == Gamemode.Trip);
        }

        private void SetMultipleTextState(bool state) {
            _downMultiplyTextUI.gameObject.SetActive(_gameData.CurrentGamemode != Gamemode.Classic && state);
            _multiplyTextUI.gameObject.SetActive(state);
        }
    }
}
