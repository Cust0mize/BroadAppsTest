using Game.Scripts.Signal;
using UnityEngine.UI;
using UnityEngine;
using Zenject;
using Enums;

namespace Game.Scripts.UI.Panels.Customization {
    [RequireComponent(typeof(Button))]
    public class AvailableElement : MonoBehaviour {
        [SerializeField] private Image _selectionRamkImage;
        [SerializeField] private Image _backIconImage;
        [SerializeField] private Button _selectButton;
        private SignalBus _signalBus;

        public ShopListType ShopListType { get; private set; }
        public BackgroundItem BackgroundItem { get; private set; }

        [Inject]
        private void Construct(SignalBus signalBus) {
            _signalBus = signalBus;
        }

        public void Init(BackgroundItem backgroundItem, ShopListType shopListType) {
            BackgroundItem = backgroundItem;
            GetComponent<Button>().RemoveAllAndSubscribeButton(Select);

            ShopListType = shopListType;
            _backIconImage.sprite = backgroundItem.Icon;
            UnselectElement();
        }

        private void Select() {
            _signalBus.Fire(new SignalSelectAvailableElement(this));
        }

        public void SelectElement() {
            _selectionRamkImage.gameObject.SetActive(true);
        }

        public void UnselectElement() {
            _selectionRamkImage.gameObject.SetActive(false);
        }

        public bool IsContains(BackgroundItem backgroundItem, out AvailableElement availableElement) {
            bool isContains = _backIconImage.sprite == backgroundItem.Icon;
            availableElement = null;
            if (isContains) {
                availableElement = this;
            }
            return isContains;
        }
    }
}