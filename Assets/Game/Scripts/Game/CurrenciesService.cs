using Game.Scripts.Signal;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Game {
    public class CurrenciesService : ILoadableElement {
        public float CurrentMoney;
        private SignalBus _signalBus;
        private GameData _gameData;

        public int Order => 1;

        public CurrenciesService(
        SignalBus signalBus,
        GameData gameData
        ) {
            _signalBus = signalBus;
            _gameData = gameData;
        }

        public void AddMoney(float addedValue) {
            CurrentMoney += Mathf.Abs(addedValue);
            UpdateMoney();
        }

        public bool RemoveMoney(float removedValue) {
            if (CurrentMoney - removedValue <= 0) {
                _signalBus.Fire(new SignalNoMoney());
                return false;
            }

            CurrentMoney -= Mathf.Abs(removedValue);
            UpdateMoney();
            return true;
        }

        public void Load() {
            CurrentMoney = _gameData.CurrentMoney;
            _signalBus.Fire(new SignalMoneyUpdate(CurrentMoney));
        }

        private void UpdateMoney() {
            _gameData.CurrentMoney = CurrentMoney;
            _signalBus.Fire(new SignalMoneyUpdate(CurrentMoney));
        }
    }
}
