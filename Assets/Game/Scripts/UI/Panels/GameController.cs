using Random = UnityEngine.Random;
using Game.Scripts.Signal;
using Game.Scripts.Game;
using UnityEngine;
using Zenject;
using System;
using TMPro;
using Enums;

namespace Game.Scripts.UI.Panels {
    public class GameController : MonoBehaviour {
        [SerializeField] private AnimationCurve anim;
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
        private GameService _gameService;
        private ClassicGame _classicGame;
        private RouteController _routeController;
        private CurrenciesService _currenciesService;
        private TwoPersonGame _twoPersonGame;
        private BaseGame _baseGame;
        private TwoPersonModeController _twoPersonModeController;

        private float _startDownMultiplyValue;
        private float _downMultiplyValue;

        [Inject]
        private void Construct(
        TwoPersonModeController twoPersonModeController,
        CurrenciesService currenciesService,
        RouteController routeController,
        TwoPersonGame twoPersonGame,
        ClassicGame classicGame,
        GameService gameService,
        SignalBus signalBus,
        GameData gameData
        ) {
            _twoPersonModeController = twoPersonModeController;
            _twoPersonGame = twoPersonGame;
            _currenciesService = currenciesService;
            _routeController = routeController;
            _gameService = gameService;
            _classicGame = classicGame;
            _signalBus = signalBus;
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
            _twoPersonGame.GameUpdate();

            if (_baseGame.IsStartGame) {
                UIUpdate();
                _downMultiplyTextUI.text = _twoPersonGame.CurrentTimeSpan.ToString(@"mm\:ss");
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
            _classicGame.GameUpdate();
            TryLose();
        }

        private void TryLose() {
            if (_baseGame.MultiplyAnimationCurve.Evaluate(_baseGame.StartTime) > _baseGame.LooseValue) {
                _signalBus.Fire<SignalLooseGame>();
                StopGame(new SignalStopGame(false));
            }
        }

        private void StopGame(SignalStopGame signalStopGame) {
            _baseGame.StopGame(signalStopGame);
            _player.UpdatePosition(0.1f);
            SetMultipleTextState(false);
            _playerNameTextUI.text = $"Player {_twoPersonGame.CurrentPlayerIndex}\n{_twoPersonModeController.FirstPersonName}";
        }

        private void UIUpdate() {
            _player.UpdatePosition(_baseGame.MultiplyAnimationCurve.Evaluate(_baseGame.StartTime));
            _multiplyTextUI.text = $"{_baseGame.CurrentMultiply:f2}X";
        }

        private void StartGame() {
            SetMultipleTextState(true);
            _baseGame.StartGame();
        }

        private void SetTripGameResult(float gameTime) {
            //float rewardValue = 0;
            //if (_currentMultiply > _multiply && _downMultiplyValue <= 0) {
            //    rewardValue = _amount * _currentMultiply;
            //    _signalBus.Fire(new SignalWinGame(rewardValue, gameTime));
            //    _downMultiplyValue = _startDownMultiplyValue;
            //}
        }

        private void AddResultValue() {
            if (_twoPersonGame.Multiply < _twoPersonGame.CurrentMultiply) {
                _twoPersonGame.AddResultValue();
            }
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
                    _startDownMultiplyValue = _routeController.CurrentSelectedRouteConfig.RouteDistance;
                    _downMultiplyValue = _routeController.CurrentSelectedRouteConfig.RouteDistance;
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

    public class TwoPersonGame : BaseGame {
        private TwoPersonModeController _twoPersonModeController;

        public TwoPersonGame(GameSettingsController gameSettingsController, CurrenciesService currenciesService, GameService gameService, UIService uIService, SignalBus signalBus, TwoPersonModeController twoPersonModeController) : base(gameSettingsController, currenciesService, gameService, uIService, signalBus) {
            _twoPersonModeController = twoPersonModeController;
        }

        protected override Gamemode GameMode => Gamemode.Two;
        public TimeSpan CurrentTimeSpan { get; private set; }
        public TimeSpan StartTimeSpan { get; private set; }
        public int PlayerCount { get; private set; }
        public int CurrentPlayerIndex { get; private set; }
        public float FirstResultValue { get; private set; }
        public float SecondResultValue { get; private set; }

        public override void SetStartValue(BaseStartGameValue startGameValue) {
            base.SetStartValue(startGameValue);

            if (BaseStartGameValue is StartTwoGameValue startTwoGameValue) {
                CurrentPlayerIndex = startTwoGameValue.CurrentPlayerIndex;
                SecondResultValue = startTwoGameValue.SecondResultValue;
                FirstResultValue = startTwoGameValue.FirstResultValue;
                StartTimeSpan = startTwoGameValue.StartTimeSpan;
                PlayerCount = startTwoGameValue.PlayerCount;
                CurrentTimeSpan = StartTimeSpan;
            }
        }

        public void GoNexPlayer() {
            if (CurrentPlayerIndex != PlayerCount) {
                CurrentPlayerIndex++;
                CurrentTimeSpan = StartTimeSpan;
            }
        }

        public override void GameUpdate() {
            base.GameUpdate();

            CurrentTimeSpan -= TimeSpan.FromSeconds(Time.deltaTime);
            float value = MultiplyAnimationCurve.Evaluate(StartTime);
            if (value > LooseValue) {
                SignalBus.Fire(new SignalStopGame(false));
            }
            if (CurrentTimeSpan < TimeSpan.FromSeconds(0)) {
                SignalBus.Fire(new SignalStopGame(true));
            }
        }

        public void AddResultValue() {
            if (CurrentPlayerIndex == 1) {
                FirstResultValue += Amount * CurrentMultiply;
            }
            else {
                SecondResultValue += Amount * CurrentMultiply;
            }
        }

        public override void SetResult() {
            UIService.GetPanel<GamePanel>().UpdateButtonState(GameButtonType.Bid);

            if (CurrentTimeSpan <= TimeSpan.FromSeconds(0) && CurrentPlayerIndex == PlayerCount) {
                AddResultValue();
                int winIndex;
                if (SecondResultValue > FirstResultValue) {
                    winIndex = 1;
                }
                else {
                    winIndex = 0;
                }
                SignalBus.Fire(new SignalUpdateResultTwoGame(winIndex, new float[] { FirstResultValue, SecondResultValue }, new string[] { _twoPersonModeController.FirstPersonName, _twoPersonModeController.SecondPersonName }));
                UIService.OpenPanel<TwoPersonResultPanel>();
            }
            else if (CurrentTimeSpan <= TimeSpan.FromSeconds(0)) {
                AddResultValue();
                GoNexPlayer();
            }
            else {
                AddResultValue();
            }
        }

        public override void StopGame(SignalStopGame signalStopGame) {
            if (signalStopGame.IsWin) {
                float gameTime = StartTimeGame - Time.time;
                SetResult();
            }
            else {
                if (CurrentPlayerIndex == 1) {
                    FirstResultValue -= Amount;
                }
                else {
                    SecondResultValue -= Amount;
                }
            }
            base.StopGame(signalStopGame);
        }
    }

    public class ClassicGame : BaseGame {
        public ClassicGame(GameSettingsController gameSettingsController, CurrenciesService currenciesService, GameService gameService, UIService uIService, SignalBus signalBus) : base(gameSettingsController, currenciesService, gameService, uIService, signalBus) {
        }

        protected override Gamemode GameMode => Gamemode.Classic;

        public override void GameUpdate() {
            base.GameUpdate();
            if (MultiplyAnimationCurve.Evaluate(StartTime) > LooseValue) {
                SignalBus.Fire<SignalLooseGame>();
                SignalBus.Fire(new SignalStopGame(false));
            }
        }

        public override void SetResult() {
            float rewardValue = 0;
            if (CurrentMultiply > Multiply) {
                rewardValue = Amount * CurrentMultiply;
                SignalBus.Fire(new SignalWinGame(rewardValue, EndTime));
            }
        }

        public override void StopGame(SignalStopGame signalStopGame) {
            if (signalStopGame.IsWin) {
                SetResult();
            }
            else {
                CurrenciesService.RemoveMoney(Amount);
            }
            base.StopGame(signalStopGame);
        }
    }

    public abstract class BaseGame {
        protected BaseStartGameValue BaseStartGameValue;
        protected abstract Gamemode GameMode { get; }

        private GameSettingsController _gameSettingsController;
        private GameService _gameService;

        protected CurrenciesService CurrenciesService { get; private set; }
        protected UIService UIService { get; private set; }
        protected SignalBus SignalBus { get; }
        public float LooseValue => 0.95f;
        protected float EndTime => StartTime - Time.time;

        public BaseGame(
            GameSettingsController gameSettingsController,
            CurrenciesService currenciesService,
            GameService gameService,
            UIService uIService,
            SignalBus signalBus
        ) {
            _gameSettingsController = gameSettingsController;
            CurrenciesService = currenciesService;
            _gameService = gameService;
            UIService = uIService;
            SignalBus = signalBus;
        }

        public virtual void SetStartValue(BaseStartGameValue startGameValue) {
            BaseStartGameValue = startGameValue;
        }

        public virtual void GameUpdate() {
            if (IsStartGame) {
                StartTime += Time.deltaTime;
                CurrentMultiply += Time.deltaTime / 2;
            }
        }

        public abstract void SetResult();

        public AnimationCurve MultiplyAnimationCurve { get; private set; }
        public float CurrentMultiply { get; private set; }
        public float StartTimeGame { get; private set; }
        public bool IsStartGame { get; private set; }
        public float StartTime { get; private set; }
        public float Multiply { get; private set; }
        public float Amount { get; private set; }

        public void StartGame() {
            CurrentMultiply = 0;
            StartTime = 0;
            IsStartGame = true;
            _gameSettingsController.ParceValue(out float multiply, out float amount);
            Multiply = multiply;
            Amount = amount;
            StartTimeGame = Time.time;
            MultiplyAnimationCurve = _gameService.GetNewCurve(0.1f, 0.95f);///////////
        }

        public virtual void StopGame(SignalStopGame signalStopGame) {
            IsStartGame = false;
            UIService.GetPanel<GamePanel>().UpdateButtonState(GameButtonType.Bid);
        }
    }

    public class TripGame : BaseGame {

        public TripGame(GameSettingsController gameSettingsController, CurrenciesService currenciesService, GameService gameService, UIService uIService, SignalBus signalBus) : base(gameSettingsController, currenciesService, gameService, uIService, signalBus) {
        }

        protected override Gamemode GameMode => Gamemode.Trip;

        public override void SetStartValue(BaseStartGameValue startGameValue) {
            base.SetStartValue(startGameValue);

            if (BaseStartGameValue is StartTripValue startTripValue) {

            }
        }

        public override void GameUpdate() {
            base.GameUpdate();

            CurrentTimeSpan -= TimeSpan.FromSeconds(Time.deltaTime);
            float value = MultiplyAnimationCurve.Evaluate(StartTime);
            if (value > LooseValue) {
                SignalBus.Fire(new SignalStopGame(false));
            }
            if (CurrentTimeSpan < TimeSpan.FromSeconds(0)) {
                SignalBus.Fire(new SignalStopGame(true));
            }
        }

        public override void SetResult() {
            UIService.GetPanel<GamePanel>().UpdateButtonState(GameButtonType.Bid);

            if (CurrentTimeSpan <= TimeSpan.FromSeconds(0) && CurrentPlayerIndex == PlayerCount) {
                AddResultValue();
                int winIndex;
                if (SecondResultValue > FirstResultValue) {
                    winIndex = 1;
                }
                else {
                    winIndex = 0;
                }
                SignalBus.Fire(new SignalUpdateResultTwoGame(winIndex, new float[] { FirstResultValue, SecondResultValue }, new string[] { _twoPersonModeController.FirstPersonName, _twoPersonModeController.SecondPersonName }));
                UIService.OpenPanel<TwoPersonResultPanel>();
            }
            else if (CurrentTimeSpan <= TimeSpan.FromSeconds(0)) {
                AddResultValue();
                GoNexPlayer();
            }
            else {
                AddResultValue();
            }
        }

        public override void StopGame(SignalStopGame signalStopGame) {
            if (signalStopGame.IsWin) {
                float gameTime = StartTimeGame - Time.time;
                SetResult();
            }
            else {
                if (CurrentPlayerIndex == 1) {
                    FirstResultValue -= Amount;
                }
                else {
                    SecondResultValue -= Amount;
                }
            }
            base.StopGame(signalStopGame);
        }
    }







    public class BaseStartGameValue {
        public BaseStartGameValue() {

        }
    }

    public class StartTwoGameValue : BaseStartGameValue {
        public TimeSpan CurrentTimeSpan { get; private set; }
        public TimeSpan StartTimeSpan { get; }
        public int PlayerCount { get; }
        public int CurrentPlayerIndex { get; private set; }
        public float FirstResultValue { get; }
        public float SecondResultValue { get; }

        public StartTwoGameValue(
            TimeSpan startTimeSpan,
            int playerCount
        ) : base() {
            PlayerCount = playerCount;
            CurrentPlayerIndex = 1;
            FirstResultValue = 0;
            SecondResultValue = 0;
            StartTimeSpan = startTimeSpan;
        }
    }

    public class StartTripValue : BaseStartGameValue {

    }
}