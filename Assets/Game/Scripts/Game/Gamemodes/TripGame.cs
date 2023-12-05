using Game.Scripts.Game.GameValue;
using Game.Scripts.UI.Panels;
using Game.Scripts.Signal;
using UnityEngine;
using Zenject;
using Enums;

namespace Game.Scripts.Game.Gamemodes {
    public class TripGame : BaseGame {
        public float StartDownMultiplyValue;
        public float DownMultiplyValue;

        public TripGame(GameSettingsController gameSettingsController, CurrenciesService currenciesService, GameService gameService, UIService uIService, SignalBus signalBus) : base(gameSettingsController, currenciesService, gameService, uIService, signalBus) {
        }

        protected override Gamemode GameMode => Gamemode.Trip;

        public override void SetStartValue(BaseStartGameValue startGameValue) {
            base.SetStartValue(startGameValue);

            if (BaseStartGameValue is StartTripValue startTripValue) {
                StartDownMultiplyValue = startTripValue.StartDownMultiplyValue;
                DownMultiplyValue = startTripValue.DownMultiplyValue;
            }
        }

        public override void GameUpdate() {
            base.GameUpdate();

            DownMultiplyValue -= Time.deltaTime;
            if (DownMultiplyValue <= 0) {
                DownMultiplyValue = 0;
            }

            if (MultiplyAnimationCurve.Evaluate(StartTime) > LooseValue) {
                SignalBus.Fire<SignalLooseGame>();
                SignalBus.Fire(new SignalStopGame(false));
            }
        }

        public override void SetResult() {
            if (CurrentMultiply > Multiply && DownMultiplyValue <= 0) {
                float rewardValue = Amount * CurrentMultiply;
                SignalBus.Fire(new SignalWinGame(rewardValue, EndTime));
                DownMultiplyValue = StartDownMultiplyValue;
            }
        }

        public override void StopGame(SignalStopGame signalStopGame) {
            base.StopGame(signalStopGame);

            if (signalStopGame.IsWin) {
                SetResult();
            }
            else {
                CurrenciesService.RemoveMoney(Amount);
            }
        }
    }
}