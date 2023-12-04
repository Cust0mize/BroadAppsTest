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
        [SerializeField] private TextMeshProUGUI _currentTimeTextUI;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _removeTime;
        [SerializeField] private Button _addTime;
        private SignalBus _signalBus;
        private float _currentTime;

        [Inject]
        private void Construct(SignalBus signalBus) {
            _signalBus = signalBus;
        }

        private void Start() {
            _startButton.RemoveAllAndSubscribeButton(StartGameClick);
            _removeTime.RemoveAllAndSubscribeButton(Remove10SecClick);
            _addTime.RemoveAllAndSubscribeButton(Add10SecClick);
            _currentTimeTextUI.text = TimeSpan.FromSeconds(_currentTime).ToString(@"mm\:ss");
        }

        private void Add10SecClick() {
            if ((TimeSpan.FromSeconds(_currentTime) < TimeSpan.FromMinutes(10))) {
                _currentTime += 10;
                _currentTimeTextUI.text = TimeSpan.FromSeconds(_currentTime).ToString(@"mm\:ss");
            }
        }

        private void StartGameClick() {
            _signalBus.Fire(new SignalStartTwoPersonModeGame(TimeSpan.FromSeconds(_currentTime), _firstPlayerNameField.text, _secondPlayerNameField.text));
            UIService.HidePanelBypassStack<TwoModePanel>();
            UIService.OpenPanel<GamePanel>();
        }

        private void Remove10SecClick() {
            if ((TimeSpan.FromSeconds(_currentTime) > TimeSpan.FromSeconds(0))) {
                _currentTime -= 10;
                _currentTimeTextUI.text = TimeSpan.FromSeconds(_currentTime).ToString(@"mm\:ss");
            }
        }
    }

    public class TwoPersonModeController {
        public string SecondPersonName { get; private set; }
        public TimeSpan TargetGameTime { get; private set; }
        public string FirstPersonName { get; private set; }

        private SignalBus _signalBus;

        public TwoPersonModeController(SignalBus signalBus) {
            _signalBus = signalBus;
            _signalBus.Subscribe<SignalStartTwoPersonModeGame>(UpdateValue);
        }

        private void UpdateValue(SignalStartTwoPersonModeGame signalStartTwoPersonModeGame) {
            SecondPersonName = signalStartTwoPersonModeGame.SecondPersonName;
            FirstPersonName = signalStartTwoPersonModeGame.FirstPersonName;
            TargetGameTime = signalStartTwoPersonModeGame.GameTime;
        }
    }
}

