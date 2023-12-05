using System.Collections.Generic;
using Game.Scripts.Signal;
using UnityEngine.UI;
using UnityEngine;
using Zenject;
using System;
using TMPro;

namespace Game.Scripts.UI.Panels {
    public class TaskGameModePanel : UIPanel {
        private const float _defaultTaskValue = 1000;

        [SerializeField] private TMP_InputField _taskValueField;
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
                float taskValue = string.IsNullOrEmpty(_taskValueField.text) ? _defaultTaskValue : float.Parse(_taskValueField.text);

                _signalBus.Fire(new SignalStartTaskModeGame(TimeSpan.FromSeconds(_timer.CurrentTime), taskValue)); ;
                UIService.HidePanelBypassStack<TaskGameModePanel>();
                UIService.OpenPanel<ComplexityPanel>();
            }
        }
    }
}