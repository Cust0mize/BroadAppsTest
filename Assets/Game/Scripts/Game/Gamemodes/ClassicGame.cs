﻿using Game.Scripts.UI.Panels;
using Game.Scripts.Signal;
using Zenject;
using Enums;
namespace Game.Scripts.Game.Gamemodes {
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
            if (CurrentMultiply > Multiply) {
                float rewardValue = Amount * CurrentMultiply;
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
}