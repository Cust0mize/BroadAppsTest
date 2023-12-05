using Game.Scripts.Signal;
using UnityEngine.UI;
using UnityEngine;
using Zenject;
using System;
using TMPro;

namespace Game.Scripts.UI.Panels {
    public class TwoModePanel : UIPanel {
        [SerializeField] private TMP_InputField _secondPlayerNameField;
        [SerializeField] private TMP_InputField _firstPlayerNameField;
        [SerializeField] private Button _startButton;
        [SerializeField] private Timer _timer;

        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus) {
            _signalBus = signalBus;
        }

        private void Start() {
            _startButton.RemoveAllAndSubscribeButton(StartGameClick);
        }

        private void StartGameClick() {
            if (_timer.TimerValueNotEmpty()) {
                string secondPlayerName = string.IsNullOrEmpty(_secondPlayerNameField.text) ? "NoName2" : _secondPlayerNameField.text;
                string firstPlayerName = string.IsNullOrEmpty(_firstPlayerNameField.text) ? "NoName1" : _firstPlayerNameField.text;

                _signalBus.Fire(new SignalStartTwoPersonModeGame(TimeSpan.FromSeconds(_timer.CurrentTime), firstPlayerName, secondPlayerName)); ;
                UIService.HidePanelBypassStack<TwoModePanel>();
                UIService.OpenPanel<ComplexityPanel>();
            }
        }
    }

    public class TwoPersonModeController {
        public string SecondPersonName { get; private set; }
        public TimeSpan TargetGameTime { get; private set; }
        public string FirstPersonName { get; private set; }

        public TwoPersonModeController(SignalBus signalBus) {
            signalBus.Subscribe<SignalStartTwoPersonModeGame>(UpdateValue);
        }

        private void UpdateValue(SignalStartTwoPersonModeGame signalStartTwoPersonModeGame) {
            SecondPersonName = signalStartTwoPersonModeGame.SecondPersonName;
            FirstPersonName = signalStartTwoPersonModeGame.FirstPersonName;
            TargetGameTime = signalStartTwoPersonModeGame.GameTime;
        }
    }
}