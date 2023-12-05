using Game.Scripts.UI.Panels;
using Game.Scripts.Signal;
using UnityEngine;
using Zenject;
using Enums;

namespace Game.Scripts.Game.Gamemodes {
    public class TripGame : BaseGame {
        private RouteController _routeController;
        public float StartDownMultiplyValue;
        public float DownMultiplyValue;

        public TripGame(GameSettingsController gameSettingsController, CurrenciesService currenciesService, LevelService levelService, GameService gameService, UIService uIService, SignalBus signalBus, RouteController routeController) : base(gameSettingsController, currenciesService, levelService, gameService, uIService, signalBus) {
            _routeController = routeController;
        }

        public override Gamemode GameMode => Gamemode.Trip;

        public override void SetStartValue() {
            StartDownMultiplyValue = _routeController.CurrentSelectedRouteConfig.RouteDistance;
            DownMultiplyValue = _routeController.CurrentSelectedRouteConfig.RouteDistance;
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
            SignalBus.Fire(new SignalDownMultipleUpdate($"{DownMultiplyValue:f2} km"));
        }

        public override void SetResult() {
            if (IsAddValue && DownMultiplyValue <= 0) {
                float rewardValue = Amount * CurrentMultiply;
                SignalBus.Fire(new SignalWinGame(rewardValue, EndTime));
                DownMultiplyValue = StartDownMultiplyValue;
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
}