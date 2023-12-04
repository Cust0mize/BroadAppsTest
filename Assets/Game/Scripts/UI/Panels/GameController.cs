using Random = UnityEngine.Random;
using Game.Scripts.Signal;
using Game.Scripts.Game;
using UnityEngine;
using Zenject;
using TMPro;
using Enums;
using System;

namespace Game.Scripts.UI.Panels {
    public class GameController : MonoBehaviour {
        [SerializeField] private AnimationCurve _multiplyAnimationCurve;
        [SerializeField] private GameSettingsController _settingsController;
        [SerializeField] private TextMeshProUGUI _downMultiplyTextUI;
        [SerializeField] private TextMeshProUGUI _endSityCityTextUI;
        [SerializeField] private TextMeshProUGUI _playerNameTextUI;
        [SerializeField] private TextMeshProUGUI _multiplyTextUI;
        [SerializeField] private TextMeshProUGUI _startCity;
        [SerializeField] private float maxValue = 0.85f;
        [SerializeField] private float minValue = 0.1f;
        [SerializeField] Player _player;

        private GameData _gameData;
        private SignalBus _signalBus;
        private UIService _uIService;
        private GameService _gameService;
        private RouteController _routeController;
        private CurrenciesService _currenciesService;
        private TwoPersonModeController _twoPersonModeController;

        private float _startTime;
        private float _startDownMultiplyValue;
        private float _downMultiplyValue;
        private float _currentMultiply;
        private float _startTimeGame;
        private float _multiply;
        private bool _startGame;
        private float _amount;
        private float _looseValue = 0.95f;

        private TimeSpan _currentTimeSpan;
        private int _playerCount;
        private int _currentPlayerIndex;
        private TimeSpan _startTimeSpan;

        private float _secondResultValue;
        private float _firstResultValue;

        [Inject]
        private void Construct(
        TwoPersonModeController twoPersonModeController,
        CurrenciesService currenciesService,
        RouteController routeController,
        GameService gameService,
        SignalBus signalBus,
        UIService uIService,
        GameData gameData
        ) {
            _twoPersonModeController = twoPersonModeController;
            _currenciesService = currenciesService;
            _routeController = routeController;
            _gameService = gameService;
            _signalBus = signalBus;
            _uIService = uIService;
            _gameData = gameData;
            signalBus.Subscribe<SignalStartGame>(StartGame);
            signalBus.Subscribe<SignalStopGame>(StopGame);
            signalBus.Subscribe<OpenGamePanel>(UpdateUI);
        }

        private void Start() {
            SetMultipleTextState(false);
            _player.UpdatePosition(0);
        }

        private void Update() {
            if (_startGame) {
                Game();
            }
        }

        private void Game() {
            if (_gameData.CurrentGamemode == Gamemode.Classic) {
                ClassicGame();
            }
            else if (_gameData.CurrentGamemode == Gamemode.Trip) {
                TripGame();
            }
            else if (_gameData.CurrentGamemode == Gamemode.Two) {
                TwoPersonGame();
            }
            else if (_gameData.CurrentGamemode == Gamemode.Task) {

            }
        }

        private void TwoPersonGame() {
            UIUpdate();
            _currentTimeSpan -= TimeSpan.FromSeconds(Time.deltaTime);

            if (_multiplyAnimationCurve.Evaluate(_startTime) > _looseValue) {
                StopGame(new SignalStopGame(false));
            }
            if (_currentTimeSpan < TimeSpan.FromSeconds(0)) {
                StopGame(new SignalStopGame(true));
            }
            _downMultiplyTextUI.text = _currentTimeSpan.ToString(@"mm\:ss");
        }

        private void GoNexPlayer() {
            if (_currentPlayerIndex != _playerCount) {
                _currentPlayerIndex++;
                _currentTimeSpan = _startTimeSpan;
                _playerNameTextUI.text = $"Player {_currentPlayerIndex}\n{_twoPersonModeController.SecondPersonName}";
            }
        }

        private void TripGame() {
            UIUpdate();

            _downMultiplyValue -= Time.deltaTime;
            if (_downMultiplyValue <= 0) {
                _downMultiplyValue = 0;
            }
            _downMultiplyTextUI.text = $"{_downMultiplyValue:f2} km";
            TryLose();
        }

        private void ClassicGame() {
            UIUpdate();
            TryLose();
        }

        private void TryLose() {
            if (_multiplyAnimationCurve.Evaluate(_startTime) > _looseValue) {
                _signalBus.Fire<SignalLooseGame>();
                StopGame(new SignalStopGame(false));
            }
        }

        private void UIUpdate() {
            _player.UpdatePosition(_multiplyAnimationCurve.Evaluate(_startTime));
            _currentMultiply += Time.deltaTime / 2;
            _multiplyTextUI.text = $"{_currentMultiply:f2}X";
            _startTime += Time.deltaTime;
        }

        private void StartGame() {
            _currentMultiply = 0;
            _startTime = 0;
            _startGame = true;
            SetMultipleTextState(true);
            _settingsController.ParceValue(out float multiply, out float amount);
            _multiply = multiply;
            _amount = amount;
            _startTimeGame = Time.time;
            _multiplyAnimationCurve = _gameService.GetNewCurve(minValue, maxValue);
        }

        private void StopGame(SignalStopGame signalStopGame) {
            if (signalStopGame.IsWin) {
                float gameTime = _startTimeGame - Time.time;
                SetWinResult(gameTime);
            }
            else {
                if (_gameData.CurrentGamemode != Gamemode.Two) {
                    _currenciesService.RemoveMoney(_amount);
                }
                else {
                    _uIService.GetPanel<GamePanel>().UpdateButtonState(GameButtonType.Bid);
                    if (_currentPlayerIndex == 1) {
                        _firstResultValue -= _amount;
                    }
                    else {
                        _secondResultValue -= _amount;
                    }
                }
            }

            _startGame = false;
            _player.UpdatePosition(0.1f);
            SetMultipleTextState(false);
        }

        private void SetWinResult(float gameTime) {
            if (_gameData.CurrentGamemode == Gamemode.Classic) {
                SetClassicGameResult(gameTime);
            }
            else if (_gameData.CurrentGamemode == Gamemode.Trip) {
                SetTripGameResult(gameTime);
            }
            else if (_gameData.CurrentGamemode == Gamemode.Two) {
                SetTwoGameResult();
            }
        }

        private void SetTwoGameResult() {
            _uIService.GetPanel<GamePanel>().UpdateButtonState(GameButtonType.Bid);

            if (_currentTimeSpan <= TimeSpan.FromSeconds(0) && _currentPlayerIndex == _playerCount) {
                AddResultValue();
                int winIndex;
                if (_secondResultValue > _firstResultValue) {
                    winIndex = 1;
                }
                else {
                    winIndex = 0;
                }
                _signalBus.Fire(new SignalUpdateResultTwoGame(winIndex, new float[] { _firstResultValue, _secondResultValue }, new string[] { _twoPersonModeController.FirstPersonName, _twoPersonModeController.SecondPersonName }));
                _uIService.OpenPanel<TwoPersonResultPanel>();
            }
            else if (_currentTimeSpan <= TimeSpan.FromSeconds(0)) {
                AddResultValue();
                GoNexPlayer();
            }
            else {
                AddResultValue();
            }
        }

        private void SetTripGameResult(float gameTime) {
            float rewardValue = 0;
            if (_currentMultiply > _multiply && _downMultiplyValue <= 0) {
                rewardValue = _amount * _currentMultiply;
                _signalBus.Fire(new SignalWinGame(rewardValue, gameTime));
                _downMultiplyValue = _startDownMultiplyValue;
            }
        }

        private void SetClassicGameResult(float gameTime) {
            float rewardValue = 0;
            if (_currentMultiply > _multiply) {
                rewardValue = _amount * _currentMultiply;
                _signalBus.Fire(new SignalWinGame(rewardValue, gameTime));
            }
        }

        private void AddResultValue() {
            if (_multiply < _currentMultiply) {
                if (_currentPlayerIndex == 1) {
                    _firstResultValue += _amount * _currentMultiply;
                }
                else {
                    _secondResultValue += _amount * _currentMultiply;
                }
            }
        }

        private void UpdateUI() {
            switch (_gameData.CurrentGamemode) {
                case Gamemode.Classic:
                    SetStateUI(Gamemode.Classic);
                    break;
                case Gamemode.Trip:
                    SetStateUI(Gamemode.Trip);
                    _startDownMultiplyValue = _routeController.CurrentSelectedRouteConfig.RouteDistance;
                    _downMultiplyValue = _routeController.CurrentSelectedRouteConfig.RouteDistance;
                    _startCity.text = _routeController.CurrentSelectedRouteConfig.StartCity;
                    _endSityCityTextUI.text = _routeController.CurrentSelectedRouteConfig.EndSityCity;
                    break;
                case Gamemode.Task: break;
                case Gamemode.Two:
                    SetStateUI(Gamemode.Two);
                    _startTimeSpan = _twoPersonModeController.TargetGameTime;
                    _currentTimeSpan = _startTimeSpan;
                    _playerCount = 2;
                    _currentPlayerIndex = 1;
                    _firstResultValue = 0;
                    _secondResultValue = 0;
                    _playerNameTextUI.text = $"Player {_currentPlayerIndex}\n{_twoPersonModeController.FirstPersonName}";
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

    public class GameService {
        private float _minTimeValue = 5;
        private float _maxTimeValue = 20;
        private int _loseChaance;

        private GameData _gameData;

        private CurrenciesService _currenciesService;
        private SignalBus _signalBus;

        public GameService(
            CurrenciesService currenciesService,
            SignalBus signalBus,
            GameData gameData
        ) {
            _gameData = gameData;
            _maxTimeValue = gameData.CurrentLevelPlane + _maxTimeValue;
            _currenciesService = currenciesService;
            _signalBus = signalBus;
            _signalBus.Subscribe<SignalUpdateAirplane>(UpdateTimeValue);
            _signalBus.Subscribe<SignalWinGame>(Win);
        }

        public AnimationCurve GetNewCurve(float minValue, float maxValue) {
            SetLoseChance();
            var animationCurve = new AnimationCurve();
            int numKeys = Random.Range(5, 10);
            float randomTime = Random.Range(_minTimeValue, _maxTimeValue);

            for (int i = 0; i < numKeys; i++) {
                float time = Random.Range(0.1f, randomTime - 1f);
                float value = Random.Range(minValue, maxValue);
                bool isLose = false;
                if (i == 0) {
                    time = 0;
                    value = 0f;
                }
                else if (_loseChaance >= Random.Range(0, 100)) {
                    value = 2;
                    isLose = true;
                }
                else if (i == numKeys - 1) {
                    time = randomTime;
                    value = 1;
                }
                else {
                    value = Mathf.Clamp(value, 0.1f, 0.94f);
                }

                animationCurve.AddKey(time, value);

                if (isLose) {
                    animationCurve.keys[i].outTangent = 1000;
                }
            }
            return animationCurve;
        }

        private void Win(SignalWinGame signalWinGame) {
            _currenciesService.AddMoney(signalWinGame.RewardValue);
            _signalBus.Fire(new SignalUpdateAchivment(AchivmentType.Earn, signalWinGame.RewardValue));
            _signalBus.Fire(new SignalUpdateAchivment(AchivmentType.Fly, signalWinGame.GameTime / 60));
            _signalBus.Fire(new SignalSaveRecord(signalWinGame.GameTime, signalWinGame.RewardValue));
        }

        private void SetLoseChance() {
            switch (_gameData.CurrentComplexity) {
                case Complexity.Easy:
                    _loseChaance = 1;
                    break;
                case Complexity.Average:
                    _loseChaance = 3;
                    break;
                case Complexity.Hard:
                    _loseChaance = 6;
                    break;
                case Complexity.Extreme:
                    _loseChaance = 9;
                    break;
            }
        }

        private void UpdateTimeValue() {
            _maxTimeValue += 1;
        }
    }
}