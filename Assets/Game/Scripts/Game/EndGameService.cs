using Game.Scripts.Signal;
using Zenject;

namespace Game.Scripts.Game {
    public class EndGameService {
        private SceneLoaderService _sceneLoaderService;
        private GameData _gameData;

        public EndGameService(
        SceneLoaderService sceneLoaderService,
        SignalBus signalBus,
        GameData gameData
        ) {
            _sceneLoaderService = sceneLoaderService;
            _gameData = gameData;
            signalBus.Subscribe<SignalResetGame>(ResetGame);
        }

        private void ResetGame() {
            _gameData.ResetGame();
            _sceneLoaderService.LoadScene(SceneName.EntryPointStarter);
        }
    }
}
