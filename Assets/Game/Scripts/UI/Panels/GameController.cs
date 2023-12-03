using Game.Scripts.Signal;
using Game.Scripts.Game;
using UnityEngine;
using Zenject;
using TMPro;

namespace Game.Scripts.UI.Panels {
    public class GameController : MonoBehaviour {
        [SerializeField] private AnimationCurve _multiplyAnimationCurve;
        [SerializeField] private GameSettingsController _settingsController;
        [SerializeField] private TextMeshProUGUI _multiplyTextUI;
        [SerializeField] private float maxValue = 0.85f;
        [SerializeField] private float minValue = 0.1f;
        [SerializeField] Player _player;
        private SignalBus _signalBus;
        private CurrenciesService _currenciesService;
        private float _startTime;
        private bool _startGame;
        private float _currentMultiply;
        private float _multiply;
        private float _amount;
        private float _startTimeGame;

        [Inject]
        private void Construct(
        CurrenciesService currenciesService,
        SignalBus signalBus
        ) {
            _signalBus = signalBus;
            _currenciesService = currenciesService;
            signalBus.Subscribe<SignalStartGame>(StartGame);
            signalBus.Subscribe<SignalStopGame>(StopGame);
        }

        private void Start() {
            _multiplyTextUI.gameObject.SetActive(false);
            _player.UpdatePosition(0);
        }

        private void Update() {
            if (_startGame) {
                _player.UpdatePosition(_multiplyAnimationCurve.Evaluate(_startTime));
                _currentMultiply += Time.deltaTime / 2;
                _multiplyTextUI.text = $"{_currentMultiply:f2}X";
                _startTime += Time.deltaTime;

                if (_multiplyAnimationCurve.Evaluate(_startTime) > 1) {
                    _signalBus.Fire<SignalLooseGame>();
                    StopGame(new SignalStopGame(false));
                }
            }
        }

        public void StartGame() {
            _currentMultiply = 0;
            _startTime = 0;
            _startGame = true;
            _multiplyTextUI.gameObject.SetActive(true);
            _settingsController.ParceValue(out float multiply, out float amount);
            _multiply = multiply;
            _amount = amount;
            _startTimeGame = Time.time;
            CreateCurve();
        }

        private void CreateCurve() {
            _multiplyAnimationCurve.ClearKeys();
            int numKeys = Random.Range(5, 10);
            float randomTime = Random.Range(5, 20);

            for (int i = 0; i < numKeys; i++) {
                float time = Random.Range(0f, randomTime - 1f);
                float value = Random.Range(minValue, maxValue);
                if (i == 0) {
                    time = 0; value = 0f;
                }
                if (i == numKeys - 1) {
                    time = randomTime;
                    value = 1;
                }

                _multiplyAnimationCurve.AddKey(time, value);
                _multiplyAnimationCurve.keys[i].outTangent = Mathf.Lerp(_multiplyAnimationCurve.keys[i].outTangent, 0, 0.5f);
                _multiplyAnimationCurve.keys[i].inTangent = Mathf.Lerp(_multiplyAnimationCurve.keys[i].inTangent, 0, 0.5f);
            }
        }

        public void StopGame(SignalStopGame signalStopGame) {
            if (signalStopGame.IsWin) {
                if (_currentMultiply > _multiply) {
                    _currenciesService.AddMoney(_amount * _currentMultiply);
                    float endTime = _startTimeGame - Time.time;
                    _signalBus.Fire(new SignalUpdateAchivment(AchivmentType.Earn, _amount * _currentMultiply));
                    _signalBus.Fire(new SignalUpdateAchivment(AchivmentType.Fly, endTime / 60));
                }
            }
            else {
                _currenciesService.RemoveMoney(_amount);
            }
            _startGame = false;
            _player.UpdatePosition(0);
            _multiplyTextUI.gameObject.SetActive(false);
        }
    }
}