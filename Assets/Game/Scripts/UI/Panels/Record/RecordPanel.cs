using UnityEngine;
using Zenject;
using System;

namespace Game.Scripts.UI.Panels.Record {
    public class RecordPanel : UIPanel {
        private AllGameRecordElement[] _allGameRecordElements;
        private BestRecordElement[] _bestRecordElements;
        private GameData _gameData;

        [Inject]
        private void Construct(GameData gameData) {
            _gameData = gameData;
        }

        private void Start() {
            SearchChildrenElement();
        }

        private void SearchChildrenElement() {
            _allGameRecordElements = transform.GetComponentsInChildren<AllGameRecordElement>(true);
            _bestRecordElements = transform.GetComponentsInChildren<BestRecordElement>(true);
        }

        public override void Show() {
            base.Show();
            if (_allGameRecordElements == null || _bestRecordElements == null) {
                SearchChildrenElement();
            }

            for (int i = _gameData.AllGameRecords.Length - 1; i >= 0; i--) {
                if (_gameData.AllGameRecords[i] != null) {
                    _allGameRecordElements[i].Init(_gameData.AllGameRecords[i]);
                }
                else {
                    _allGameRecordElements[i].gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < _gameData.BestGameRecords.Length; i++) {
                if (_gameData.BestGameRecords[i] != null) {
                    _bestRecordElements[i].Init(_gameData.BestGameRecords[i]);
                }
                else {
                    _bestRecordElements[i].gameObject.SetActive(false);
                }
            }
        }
    }

    [Serializable]
    public class AllGameRecordInfo {
        public DateTime DateTime { get; private set; }
        public float GameTime { get; private set; }
        public float Balance { get; private set; }
        public float Reward { get; private set; }

        public AllGameRecordInfo(DateTime dateTime, float gameTime, float balance, float reward) {
            GameTime = Mathf.Abs(gameTime);
            DateTime = dateTime;
            Balance = balance;
            Reward = reward;
        }
    }

    [Serializable]
    public class BestRecordInfo {
        public float GameTime { get; private set; }
        public float Reward { get; private set; }

        public BestRecordInfo(float gameTime, float reward) {
            GameTime = Mathf.Abs(gameTime);
            Reward = reward;
        }
    }
}