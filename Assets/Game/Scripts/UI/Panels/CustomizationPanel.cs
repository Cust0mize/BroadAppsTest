using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Scripts.UI.Panels {
    public class CustomizationPanel : UIPanel, ILoadableElement {
        [SerializeField] private AvailablePanel _availablePanel;
        private BaseReplaceButton[] _replaceButtons;
        private ShopListPanel[] _backgroundPanels;

        private GameData _gameData;
        public int Order => 5;

        [Inject]
        private void Construct(
        GameData gameData
        ) {
            _gameData = gameData;
        }

        public override void Show() {
            base.Show();
            UIService.OpenPanel<BackgroundShopList>();
            ReplaceButtonSetSelect(ReplaceButtonType.Background);
        }

        public void Load() {
            _backgroundPanels = transform.GetComponentsInChildren<ShopListPanel>(true);
            _replaceButtons = transform.GetComponentsInChildren<BaseReplaceButton>(true);
            for (int i = 0; i < _replaceButtons.Length; i++) {
                _replaceButtons[i].Init(this);
            }

            for (int i = 0; i < _backgroundPanels.Length; i++) {
                if (!_gameData.BuyShopItems.ContainsKey(_backgroundPanels[i].ShopListType)) {
                    _gameData.BuyShopItems.Add(_backgroundPanels[i].ShopListType, new List<int>());
                }

                _backgroundPanels[i].Init();
            }

            _availablePanel.Init();
            gameObject.SetActive(false);
            UIService.HidePanelBypassStack<AirplaneShopList>();
            UIService.OpenPanel<BackgroundShopList>();
        }

        public void ReplaceButtonSetSelect(ReplaceButtonType replaceButtonType) {
            for (int i = 0; i < _replaceButtons.Length; i++) {
                if (replaceButtonType == _replaceButtons[i].ReplaceButtonType) {
                    _replaceButtons[i].SelectButton();
                }
                else {
                    _replaceButtons[i].UnselectButton();
                }
            }
        }
    }

    public enum ShopListType {
        Airplane,
        Background
    }
}