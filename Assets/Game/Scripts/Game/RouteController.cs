using Game.Scripts.Signal;
using Zenject;

namespace Game.Scripts.Game {
    public class RouteController {
        public RouteConfig CurrentSelectedRouteConfig { get; private set; }
        private GameData _gameData;

        public RouteController(
            SignalBus signalBus,
            GameData gameData
        ) {
            signalBus.Subscribe<SignalBuyRoute>(BuyRoute);
            signalBus.Subscribe<SignalSelectRoute>(SelectRoute);
            _gameData = gameData;
        }

        private void BuyRoute(SignalBuyRoute signalBuyRoute) {
            _gameData.BuyRoutes.Add(signalBuyRoute.RouteConfig.RouteName);
            _gameData.Save();
        }

        private void SelectRoute(SignalSelectRoute signalSelectRoute) {
            CurrentSelectedRouteConfig = signalSelectRoute.RouteConfig;
        }
    }
}
