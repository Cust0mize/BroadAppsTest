using Game.Scripts.Signal;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Scripts.UI.Panels {
    public class CustomizationPanel : UIPanel, ILoadableElement {
        [SerializeField] private BackgroundPanel _backgroundPanel;
        [SerializeField] private AvailableElement _availableElementPrefab;
        [SerializeField] private RectTransform _availableElementRoot;
        private List<AvailableElement> _availableElements = new();
        private SignalBus _signalBus;
        private GameData _gameData;
        public int Order => 5;

        [Inject]
        private void Construct(
        SignalBus signalBus,
        GameData gameData
        ) {
            _signalBus = signalBus;
            _gameData = gameData;
            _signalBus.Subscribe<SignalBuyNewElemetn>(BuyElement);
        }

        private void BuyElement(SignalBuyNewElemetn signalBuyNewElemetn) {
            AddNewAvalibleElement(signalBuyNewElemetn.BackgroundItem);
        }

        public void Load() {
            _backgroundPanel.Init();
            for (int i = 0; i < _backgroundPanel.BackgroundItemsSO.Count; i++) {
                if (_gameData.BuyBackgroundsIndex.Contains(_backgroundPanel.BackgroundItemsSO[i].Order)) {
                    AddNewAvalibleElement(_backgroundPanel.BackgroundItemsSO[i]);
                }
            }

            gameObject.SetActive(false);
        }

        private void AddNewAvalibleElement(BackgroundItem backgroundItem) {
            var element = Instantiate(_availableElementPrefab, _availableElementRoot);
            element.Init(backgroundItem.Icon);
            _availableElements.Add(element);
        }
    }
}