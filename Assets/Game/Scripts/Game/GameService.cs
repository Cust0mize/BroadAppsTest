using Random = UnityEngine.Random;
using Game.Scripts.Signal;
using UnityEngine;
using Zenject;
using Enums;

namespace Game.Scripts.Game {
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
                    value = 0.1f;
                }
                else if (_loseChaance >= Random.Range(0, 100)) {
                    value = 2;
                    isLose = true;
                }
                else if (i == numKeys - 1) {
                    time = randomTime;
                    value = 2;
                    isLose = true;
                }
                else {
                    value = Mathf.Clamp(value, 0.1f, 0.94f);
                }

                animationCurve.AddKey(time, value);

                if (isLose) {
                    animationCurve.keys[i].outTangent = 10000;
                }
            }
            return animationCurve;
        }

        private void Win(SignalWinGame signalWinGame) {
            _currenciesService.AddMoney(signalWinGame.RewardValue);
            _signalBus.Fire(new SignalUpdateAchivment(AchivmentType.Earn, signalWinGame.RewardValue));
            _signalBus.Fire(new SignalUpdateAchivment(AchivmentType.Fly, signalWinGame.GameTime / 60));
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