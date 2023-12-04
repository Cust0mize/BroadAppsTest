using System.Collections.Generic;
using Game.Scripts.Signal;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using Zenject;
using System;
using Enums;

namespace Game.Scripts.UI.Panels.Customization {
    public class AvailablePanel : UIPanel {
        [SerializeField] private HorizontalLayoutGroup _availableContentRect;
        [SerializeField] private AvailableElement _availableElementPrefab;
        [SerializeField] private RectTransform _availableElementRoot;
        private List<AvailableElement> _availableElements = new();
        private RectTransform _layoutRect;
        private DiContainer _diContainer;
        private SignalBus _signalBus;
        private GameData _gameData;
        private float _prefabWidth;

        [Inject]
        private void Construct(
            DiContainer diContainer,
            SignalBus signalBus,
            GameData gameData
        ) {
            _diContainer = diContainer;
            _signalBus = signalBus;
            _gameData = gameData;

            _signalBus.Subscribe<SignalSelectAvailableElement>(SelectNewElement);
            _signalBus.Subscribe<SignalSelectAvailableElement>(SaveSelectElement);
            _signalBus.Subscribe<SignalBuyNewElemetn>(BuyElement);
        }

        public void Init() {
            _prefabWidth = _availableElementPrefab.GetComponent<RectTransform>().rect.width;
            _layoutRect = _availableContentRect.GetComponent<RectTransform>();
            gameObject.SetActive(false);
            LoadBuyElement(UIService.GetPanel<BackgroundShopList>());
            LoadBuyElement(UIService.GetPanel<AirplaneShopList>());

            foreach (var item in Enum.GetValues(typeof(ShopListType))) {
                if (_gameData.SelectCustomElement.ContainsKey((ShopListType)item)) {
                    _signalBus.Fire(new SignalSelectAvailableElement(_availableElements.FirstOrDefault(x => x.BackgroundItem.Order == _gameData.SelectCustomElement[(ShopListType)item])));
                }
            }
        }

        private void AddNewAvalibleElement(BackgroundItem backgroundItem, ShopListType shopListType) {
            bool isContains = false;
            for (int i = 0; i < _availableElements.Count; i++) {
                if (_availableElements[i].IsContains(backgroundItem, out AvailableElement element)) {
                    isContains = true;
                    element.gameObject.SetActive(true);
                    break;
                }
            }
            if (!isContains) {
                var element = _diContainer.InstantiatePrefabForComponent<AvailableElement>(_availableElementPrefab, _availableElementRoot);
                element.Init(backgroundItem, shopListType);
                _availableElements.Add(element);
            }
            RectUtils.SetHorizontalRectInLayoutGroup(_layoutRect, _availableContentRect, _prefabWidth, _gameData.BuyShopItems[shopListType].Count);
        }

        public void LoadBuyElement(ShopListPanel backgroundShopPanel) {
            var count = _availableElementRoot.childCount;
            for (int i = 0; i < count; i++) {
                _availableElementRoot.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < backgroundShopPanel.BackgroundItemsSO.Count; i++) {
                if (_gameData.BuyShopItems[backgroundShopPanel.ShopListType].Contains(backgroundShopPanel.BackgroundItemsSO[i].Order)) {
                    AddNewAvalibleElement(backgroundShopPanel.BackgroundItemsSO[i], backgroundShopPanel.ShopListType);
                }
            }
            if (_gameData.SelectCustomElement.ContainsKey(backgroundShopPanel.ShopListType)) {
                SelectElementByOrder(_gameData.SelectCustomElement[backgroundShopPanel.ShopListType], backgroundShopPanel.ShopListType);
            }
        }

        private void BuyElement(SignalBuyNewElemetn signalBuyNewElemetn) {
            AddNewAvalibleElement(signalBuyNewElemetn.BackgroundItem, signalBuyNewElemetn.ShopListType);
        }

        private void SelectNewElement(SignalSelectAvailableElement signalBuyNewElemetn) {
            foreach (var item in _availableElements) {
                if (item == signalBuyNewElemetn.AvailableElement && item.ShopListType == signalBuyNewElemetn.AvailableElement.ShopListType) {
                    item.SelectElement();
                }
                else {
                    item.UnselectElement();
                }
            }
        }

        public void SelectElementByOrder(int order, ShopListType shopListType) {
            var targetElemetn = _availableElements.FirstOrDefault(x => x.BackgroundItem.Order == order && x.ShopListType == shopListType);

            foreach (var item in _availableElements) {
                if (targetElemetn == item) {
                    item.SelectElement();
                }
                else {
                    item.UnselectElement();
                }
            }
        }

        private void SaveSelectElement(SignalSelectAvailableElement signalBuyNewElemetn) {
            if (_gameData.SelectCustomElement.ContainsKey(signalBuyNewElemetn.AvailableElement.ShopListType)) {
                _gameData.SelectCustomElement[signalBuyNewElemetn.AvailableElement.ShopListType] = signalBuyNewElemetn.AvailableElement.BackgroundItem.Order;
            }
            else {
                _gameData.SelectCustomElement.Add(signalBuyNewElemetn.AvailableElement.ShopListType, signalBuyNewElemetn.AvailableElement.BackgroundItem.Order);
            }
            _gameData.Save();
        }
    }
}