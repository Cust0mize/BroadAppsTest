using Game.Scripts.Game.GameValue;
using Game.Scripts.UI.Panels;
using Game.Scripts.Signal;
using UnityEngine;
using Zenject;
using System;
using Enums;

namespace Game.Scripts.Game.Gamemodes {
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
}