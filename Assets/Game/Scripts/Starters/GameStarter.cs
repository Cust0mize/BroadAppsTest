using System.Collections.Generic;
using Game.Scripts.UI.Panels;
using Game.Scripts.Signal;
using UnityEngine;
using Zenject;

public class GameStarter : MonoBehaviour {
    private List<ILoadableElement> _loadableElements = new();
    private SignalBus _signalBus;
    private GameData _gameData;

    [Inject]
    private void Construct(
        List<ILoadableElement> loadableElements,
        UIService uIService,
        SignalBus signalBus,
        GameData gameData
    ) {
        _loadableElements = loadableElements;
        _signalBus = signalBus;
        _gameData = gameData;
        signalBus.Subscribe<SignalNewDay>(() => uIService.OpenPanel<EveryDayPanel>());
    }

    private void Start() {
        List<ILoadableElement> newElement = _loadableElements;
        _loadableElements.Sort((x, y) => x.Order.CompareTo(y.Order));
        for (int i = 0; i < newElement.Count; i++) {
            newElement[i].Load();
        }

        if (!_gameData.IsShowOnboarding) {
            _signalBus.Fire(new SignalGameIsLoad());
        }
    }
}