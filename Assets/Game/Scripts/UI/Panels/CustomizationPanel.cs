using System.Collections.Generic;
using Game.Scripts.Signal;
using UnityEngine.UI;
using UnityEngine;
using Zenject;

namespace Game.Scripts.UI.Panels {
    public class CustomizationPanel : UIPanel, ILoadableElement {
        [SerializeField] private ShopListPanel[] _backgroundPanels;
        [SerializeField] private AvailableElement _availableElementPrefab;
        [SerializeField] private RectTransform _availableElementRoot;
        [SerializeField] private HorizontalLayoutGroup _availableContentRect;
        private BaseReplaceButton[] _replaceButtons;
        private List<AvailableElement> _availableElements = new();
        private RectTransform _layoutRect;
        private DiContainer _diContainer;
        private SignalBus _signalBus;
        private float _prefabWidth;

        private GameData _gameData;
        public int Order => 5;

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
            _signalBus.Subscribe<SignalBuyNewElemetn>(BuyElement);
        }

        private void BuyElement(SignalBuyNewElemetn signalBuyNewElemetn) {
            AddNewAvalibleElement(signalBuyNewElemetn.BackgroundItem, signalBuyNewElemetn.ShopListType);
        }

        public override void Show() {
            base.Show();
            UIService.OpenPanel<BackgroundShopList>();
            ReplaceButtonSetSelect(ReplaceButtonType.Background, UIService.GetPanel<BackgroundShopList>());
            LoadBuyElement(UIService.GetPanel<BackgroundShopList>());
        }

        public void Load() {
            _prefabWidth = _availableElementPrefab.GetComponent<RectTransform>().rect.width;
            _layoutRect = _availableContentRect.GetComponent<RectTransform>();
            _backgroundPanels = transform.GetComponentsInChildren<ShopListPanel>();
            _replaceButtons = transform.GetComponentsInChildren<BaseReplaceButton>();
            for (int i = 0; i < _replaceButtons.Length; i++) {
                _replaceButtons[i].Init(this);
            }

            for (int i = 0; i < _backgroundPanels.Length; i++) {
                if (!_gameData.BuyShopItems.ContainsKey(_backgroundPanels[i].ShopListType)) {
                    _gameData.BuyShopItems.Add(_backgroundPanels[i].ShopListType, new List<int>());
                }

                _backgroundPanels[i].Init();
            }

            gameObject.SetActive(false);
            UIService.HidePanelBypassStack<AirplaneShopList>();
            UIService.OpenPanel<BackgroundShopList>();
        }

        public void ReplaceButtonSetSelect(ReplaceButtonType replaceButtonType, ShopListPanel panel) {
            for (int i = 0; i < _replaceButtons.Length; i++) {
                if (replaceButtonType == _replaceButtons[i].ReplaceButtonType) {
                    _replaceButtons[i].SelectButton();
                    LoadBuyElement(panel);
                }
                else {
                    _replaceButtons[i].UnselectButton();
                }
            }
        }

        private void LoadBuyElement(ShopListPanel backgroundShopPanel) {
            var count = _availableElementRoot.childCount;
            for (int i = 0; i < count; i++) {
                _availableElementRoot.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < backgroundShopPanel.BackgroundItemsSO.Count; i++) {
                if (_gameData.BuyShopItems[backgroundShopPanel.ShopListType].Contains(backgroundShopPanel.BackgroundItemsSO[i].Order)) {
                    AddNewAvalibleElement(backgroundShopPanel.BackgroundItemsSO[i], backgroundShopPanel.ShopListType);
                }
            }
            backgroundShopPanel.UpdateRectSize();
            RectUtils.SetHorizontalRectInLayoutGroup(_layoutRect, _availableContentRect, _prefabWidth, _gameData.BuyShopItems[backgroundShopPanel.ShopListType].Count);
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
        }

        private void SelectNewElement(SignalSelectAvailableElement signalBuyNewElemetn) {
            foreach (var item in _availableElements) {
                if (item == signalBuyNewElemetn.AvailableElement) {
                    item.SelectElement();
                }
                else {
                    item.UnselectElement();
                }
            }
        }
    }

    public enum ShopListType {
        Airplane,
        Background
    }
}