using Game.Scripts.UI.Panels;
using Game.Scripts.Signal;
using UnityEngine;
using Zenject;
using System;
using Enums;

namespace Game.Scripts.Game.Gamemodes {
    public class TaskGame : BaseGame {
        public override Gamemode GameMode => Gamemode.Task;
        private TimeSpan _targetGameTime;
        private float _targetTaskValue;

        private TaskModeController _taskModeController;
        private float _resultValue;

        public TaskGame(GameSettingsController gameSettingsController, CurrenciesService currenciesService, GameService gameService, UIService uIService, SignalBus signalBus, TaskModeController taskModeController) : base(gameSettingsController, currenciesService, gameService, uIService, signalBus) {
            _taskModeController = taskModeController;
        }

        public override void SetStartValue() {
            _targetGameTime = _taskModeController.TargetGameTime;
            _targetTaskValue = _taskModeController.TaskValue;
            _resultValue = 0;
        }

        public override void SetResult() {
            UIService.GetPanel<GamePanel>().UpdateButtonState(GameButtonType.Bid);

            if (_targetGameTime <= TimeSpan.FromSeconds(0)) {
                AddResultValue();
                bool isWin = _resultValue > _targetTaskValue;
                SignalBus.Fire(new SignalEndTaskGame(_targetTaskValue, isWin, _resultValue));
                UIService.OpenPanel<TaskResultPanel>();
            }
            else {
                AddResultValue();
            }
        }

        private void AddResultValue() {
            _resultValue += Amount * CurrentMultiply;
        }

        public override void StopGame(SignalStopGame signalStopGame) {
            if (signalStopGame.IsWin) {
                SetResult();
            }
            else {
                _resultValue -= Amount;
            }
            base.StopGame(signalStopGame);
        }

        public override void GameUpdate() {
            base.GameUpdate();

            _targetGameTime -= TimeSpan.FromSeconds(Time.deltaTime);
            float value = MultiplyAnimationCurve.Evaluate(StartTime);
            if (value > LooseValue) {
                SignalBus.Fire(new SignalStopGame(false));
            }
            if (_targetGameTime < TimeSpan.FromSeconds(0)) {
                SignalBus.Fire(new SignalStopGame(true));
            }
            SignalBus.Fire(new SignalDownMultipleUpdate(_targetGameTime.ToString(@"mm\:ss")));
        }
    }
}