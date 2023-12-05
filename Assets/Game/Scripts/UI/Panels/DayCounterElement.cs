using UnityEngine;
using Zenject;
using System;

namespace Game.Scripts.UI.Panels {
    public class DayCounterElement : MonoBehaviour {
        private DayElement[] _dayElements;
        private GameData _gameData;

        [Inject]
        private void Construct(GameData gameData) {
            _gameData = gameData;
        }

        public void Init() {
            _dayElements = transform.GetComponentsInChildren<DayElement>();
            int currentDayIndex = (int)DateTime.Now.DayOfWeek;
            int startDayIndex = currentDayIndex - _gameData.DayIndex;
            int startWeekIndex = startDayIndex < 1 ? 0 : startDayIndex;

            for (int i = 0; i < _dayElements.Length; i++) {
                if (currentDayIndex > startWeekIndex && i >= startWeekIndex) {
                    _dayElements[i].Init(((Days)i).ToString(), true);
                    startWeekIndex++;
                }
                else {
                    _dayElements[i].Init(((Days)i).ToString(), false);
                }
            }
        }
    }
}
