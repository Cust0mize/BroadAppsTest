using Game.Scripts.UI.Panels;
using Game.Scripts.Signal;
using Zenject;
using Enums;

namespace Game.Scripts.Game.Gamemodes {
    public class ClassicGame : BaseGame {
        public ClassicGame(GameSettingsController gameSettingsController, CurrenciesService currenciesService, LevelService levelService, GameService gameService, UIService uIService, SignalBus signalBus) : base(gameSettingsController, currenciesService, levelService, gameService, uIService, signalBus) {
        }

        public override Gamemode GameMode => Gamemode.Classic;

        public override void GameUpdate() {
            base.GameUpdate();
            if (MultiplyAnimationCurve.Evaluate(StartTime) > LooseValue) {
                SignalBus.Fire<SignalLooseGame>();
                SignalBus.Fire(new SignalStopGame(false));
            }
        }

        public override void SetResult() {
            if (IsAddValue) {
                float rewardValue = Amount * CurrentMultiply;
                SignalBus.Fire(new SignalWinGame(rewardValue, EndTime));
            }
        }

        public override void SetStartValue() {
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