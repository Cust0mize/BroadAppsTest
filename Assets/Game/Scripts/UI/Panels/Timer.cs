using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

namespace Game.Scripts.UI.Panels {
    public class Timer : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _currentTimeTextUI;
        [SerializeField] private Button _removeTime;
        [SerializeField] private Button _addTime;
        public float CurrentTime { get; private set; }

        private void Start() {
            _currentTimeTextUI.text = TimeSpan.FromSeconds(CurrentTime).ToString(@"mm\:ss");
            _removeTime.RemoveAllAndSubscribeButton(Remove10SecClick);
            _addTime.RemoveAllAndSubscribeButton(Add10SecClick);
        }

        private void Remove10SecClick() {
            if ((TimeSpan.FromSeconds(CurrentTime) > TimeSpan.FromSeconds(0))) {
                CurrentTime -= 10;
                _currentTimeTextUI.text = TimeSpan.FromSeconds(CurrentTime).ToString(@"mm\:ss");
            }
        }

        private void Add10SecClick() {
            if ((TimeSpan.FromSeconds(CurrentTime) < TimeSpan.FromMinutes(10))) {
                CurrentTime += 10;
                _currentTimeTextUI.text = TimeSpan.FromSeconds(CurrentTime).ToString(@"mm\:ss");
            }
        }

        public bool TimerValueNotEmpty() {
            return CurrentTime > 0;
        }
    }
}

