using Game.Scripts.Signal;
using UnityEngine.UI;
using UnityEngine;
using Zenject;
using TMPro;
using Game.Scripts.Game;

namespace Game.Scripts.UI.Panels {
    public class RouteElement : MonoBehaviour {
        public RouteConfig RouteConfig { get; private set; }

        [SerializeField] private TextMeshProUGUI _routeDistanceTextUI;
        [SerializeField] private TextMeshProUGUI _routeNameTextUI;
        [SerializeField] private Image _selectionImage;
        [SerializeField] private Button _selectButton;
        [SerializeField] private Button _buyButton;
        private CurrenciesService _currenciesService;
        private SignalBus _signalBus;
        private bool _isBuy;
        private float _price;

        [Inject]
        private void Construct(
            CurrenciesService currenciesService,
            SignalBus signalBus
        ) {
            _currenciesService = currenciesService;
            _signalBus = signalBus;
        }

        public void Init(RouteConfig routeConfig, bool isBuy) {
            _isBuy = isBuy;
            _selectButton.RemoveAllAndSubscribeButton(SelectClick);
            _buyButton.RemoveAllAndSubscribeButton(BuyClick);
            IsBuySetState();
            RouteConfig = routeConfig;
            _price = RouteConfig.RoutePrice;
            UpdateUIText();
            SetSelectState(false);
        }

        public void SetSelectState(bool state) {
            _selectionImage.gameObject.SetActive(state);
        }

        public void UpdateUIText() {
            if (_isBuy) {
                _routeDistanceTextUI.text = $"Distance: {RouteConfig.RouteDistance} km";
            }
            else {
                _routeDistanceTextUI.text = $"Price: ${RouteConfig.RoutePrice}";
            }
            _routeNameTextUI.text = $"{RouteConfig.RouteName}";
        }

        private void IsBuySetState() {
            _selectButton.gameObject.SetActive(_isBuy);
            _buyButton.gameObject.SetActive(!_isBuy);
        }

        private void BuyClick() {
            if (_currenciesService.RemoveMoney(_price)) {
                _isBuy = true;
                IsBuySetState();
                _signalBus.Fire(new SignalBuyRoute(RouteConfig));
            }
        }

        private void SelectClick() {
            _signalBus.Fire(new SignalSelectRoute(RouteConfig));
        }
    }
}