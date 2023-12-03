using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Game.Scripts.UI.Panels {

    public class BackgroundElement : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _priceTextUI;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Image _iconImage;
        private ShopListPanel _backgroundPanel;
        private BackgroundItem _item;
        public bool IsBuy { get; private set; }

        public void Init(BackgroundItem backgroundItem, ShopListPanel backgroundPanel, bool isBuy) {
            _backgroundPanel = backgroundPanel;
            _item = backgroundItem;
            _iconImage.sprite = _item.Icon;
            _priceTextUI.text = _item.Price.ToString();
            _buyButton.RemoveAllAndSubscribeButton(ButtonClick);
            IsBuy = isBuy;
            gameObject.SetActive(!IsBuy);
        }

        private void ButtonClick() {
            if (_backgroundPanel.TryBuyUpgrade(_item)) {
                gameObject.SetActive(false);
                IsBuy = true;
            }
        }
    }
}