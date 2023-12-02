using System.Collections.Generic;
using Game.Scripts.Signal;
using Game.Scripts.Game;
using UnityEngine.UI;
using UnityEngine;
using Zenject;

namespace Game.Scripts.UI.Panels {

    public class BackgroundPanel : UIPanel {
        [field: SerializeField] public List<BackgroundItem> BackgroundItemsSO { get; private set; }
        [SerializeField] private RectTransform _gridRectTransform;
        [SerializeField] private GridLayoutGroup _currentGrid;
        private BackgroundElement[] _backgroundElements;
        private CurrenciesService _currenciesService;
        private SignalBus _signalBus;
        private GameData _gameData;

        [Inject]
        private void Construct(
        CurrenciesService currenciesService,
        SignalBus signalBus,
        GameData gameData
        ) {
            _currenciesService = currenciesService;
            _signalBus = signalBus;
            _gameData = gameData;
        }

        public bool TryBuyUpgrade(BackgroundItem backgroundItem) {
            if (_currenciesService.RemoveMoney(backgroundItem.Price)) {
                _gameData.BuyBackgroundsIndex.Add(backgroundItem.Order);
                _gameData.Save();
                _signalBus.Fire(new SignalBuyNewElemetn(backgroundItem));
                return true;
            }
            else {
                return false;
            }
        }

        public void Init() {
            _backgroundElements = transform.GetComponentsInChildren<BackgroundElement>(true);
            BackgroundItemsSO.Sort((x, y) => x.Order.CompareTo(y.Order));
            for (int i = 0; i < _backgroundElements.Length; i++) {
                _backgroundElements[i].Init(BackgroundItemsSO[i], this, _gameData.BuyBackgroundsIndex.Contains(BackgroundItemsSO[i].Order));
            }
            RectUtils.SetRectSizeInGrid(_currentGrid, _gridRectTransform, BackgroundItemsSO.Count);
        }
    }
}