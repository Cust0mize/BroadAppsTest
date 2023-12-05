using System.Collections.Generic;
using Game.Scripts.Signal;
using Game.Scripts.Game;
using UnityEngine.UI;
using UnityEngine;
using Zenject;

namespace Game.Scripts.UI.Panels {
    public class TripModePanel : UIPanel {
        [SerializeField] private VerticalLayoutGroup _verticalLayourGroup;
        [SerializeField] private RectTransform _routeElementsRoot;
        [SerializeField] private RouteElement _routeElementPrefab;
        [SerializeField] private RouteConfig[] _routeConfigs;
        [SerializeField] private Button _startGameButton;
        private List<RouteElement> _routeElements = new();
        private RouteController _routeController;
        private DiContainer _diContainer;
        private GameData _gameData;

        [Inject]
        private void Construct(
            RouteController routeController,
            DiContainer diContainer,
            SignalBus signalBus,
            GameData gameData
        ) {
            signalBus.Subscribe<SignalSelectRoute>(UnselectElements);
            signalBus.Subscribe<SignalBuyRoute>(UpdateRouteElements);
            _routeController = routeController;
            _diContainer = diContainer;
            _gameData = gameData;
        }

        private void UpdateRouteElements(SignalBuyRoute signalBuyRoute) {
            for (int i = 0; i < _routeElements.Count; i++) {
                _routeElements[i].UpdateUIText();
            }
        }

        private void UnselectElements(SignalSelectRoute signalSelectRoute) {
            for (int i = 0; i < _routeElements.Count; i++) {
                if (_routeElements[i].RouteConfig == signalSelectRoute.RouteConfig) {
                    _routeElements[i].SetSelectState(true);
                }
                else {
                    _routeElements[i].SetSelectState(false);
                }
            }
        }

        private void Start() {
            _startGameButton.RemoveAllAndSubscribeButton(StartButtonClick);

            for (int i = 0; i < _routeConfigs.Length; i++) {
                var element = _diContainer.InstantiatePrefabForComponent<RouteElement>(_routeElementPrefab, _routeElementsRoot);
                element.Init(_routeConfigs[i], _gameData.BuyRoutes.Contains(_routeConfigs[i].RouteName));
                _routeElements.Add(element);
            }
            var prefabWidth = _routeElementPrefab.GetComponent<RectTransform>().rect.height;

            RectUtils.SetVerticalRectInLayoutGroup(_routeElementsRoot, _verticalLayourGroup, prefabWidth, _routeConfigs.Length);
        }

        private void StartButtonClick() {
            if (_routeController.CurrentSelectedRouteConfig != null) {
                UIService.HidePanelBypassStack<TripModePanel>();
                UIService.OpenPanel<ComplexityPanel>();
            }
        }
    }
}

