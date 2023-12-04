using System.Collections.Generic;
using Game.Scripts.Signal;
using UnityEngine;
using Zenject;
using System;
using Enums;
using TMPro;
using System.Runtime.CompilerServices;
using UnityEditor.Tilemaps;

namespace Game.Scripts.UI.Panels.Achivment {
    public class AchivmentPanel : UIPanel, ILoadableElement {
        [SerializeField] private AchivmentConfig[] _achivmentConfigs;
        [SerializeField] private AchivmentItem _achivmentItemPrefab;
        [SerializeField] private RectTransform _achivmentRectRoot;
        private List<AchivmentItem> _achivmentItems = new();
        private DiContainer _diContainer;
        private GameData _gameData;

        public int Order => 5;

        [Inject]
        private void Construct(
        DiContainer diContainer,
        SignalBus signalBus,
        GameData gameData
        ) {
            _diContainer = diContainer;
            _gameData = gameData;
            signalBus.Subscribe<SignalUpdateAchivment>(UpdateAchivmentValue);
        }

        public override void Show() {
            base.Show();

        }

        public void Load() {
            List<AchivmentConfig> addedConfigs = new();
            foreach (var item in Enum.GetValues(typeof(AchivmentType))) {
                if (_gameData.TakedReward.ContainsKey((AchivmentType)item)) {
                    for (int i = 0; i < _gameData.TakedReward[(AchivmentType)item].Count; i++) {
                        for (int j = 0; j < _achivmentConfigs.Length; j++) {
                            if (_achivmentConfigs[j].TargetValue == _gameData.TakedReward[(AchivmentType)item][i] && _achivmentConfigs[j].AchivmentType == (AchivmentType)item) {
                                var achivmentElement = _diContainer.InstantiatePrefabForComponent<AchivmentItem>(_achivmentItemPrefab, _achivmentRectRoot);
                                achivmentElement.Init(_achivmentConfigs[j], true);
                                _achivmentItems.Add(achivmentElement);
                                addedConfigs.Add(_achivmentConfigs[j]);
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < _achivmentConfigs.Length; i++) {
                if (addedConfigs.Contains(_achivmentConfigs[i])) {
                    continue;
                }
                var achivmentElement = _diContainer.InstantiatePrefabForComponent<AchivmentItem>(_achivmentItemPrefab, _achivmentRectRoot);
                achivmentElement.Init(_achivmentConfigs[i], false);
                _achivmentItems.Add(achivmentElement);
            }
            gameObject.SetActive(false);
        }

        private void UpdateAchivmentValue(SignalUpdateAchivment signalUpdateAchivment) {
            _gameData.AchivmentValue[signalUpdateAchivment.AchivmentType] += signalUpdateAchivment.Value;
            for (int i = 0; i < _achivmentItems.Count; i++) {
                if (signalUpdateAchivment.AchivmentType == _achivmentItems[i].AchivmentType) {
                    _achivmentItems[i].UpdateValue(_gameData.AchivmentValue[signalUpdateAchivment.AchivmentType]);
                    _gameData.Save();
                }
            }
        }
    }
}
