using System.Collections.Generic;
using Game.Scripts.Signal;
using Game.Scripts.UI.Panels;
using UnityEngine;
using Zenject;

public class GameStarter : MonoBehaviour {
    private List<ILoadableElement> _loadableElements = new();
    private SignalBus _signalBus;

    [Inject]
    private void Construct(
        List<ILoadableElement> loadableElements,
        UIService uIService,
        SignalBus signalBus
    ) {
        _loadableElements = loadableElements;
        _signalBus = signalBus;
        signalBus.Subscribe<SignalNewDay>(() => uIService.OpenPanel<EveryDayPanel>());
    }

    private void Start() {
        List<ILoadableElement> newElement = _loadableElements;
        _loadableElements.Sort((x, y) => x.Order.CompareTo(y.Order));
        for (int i = 0; i < newElement.Count; i++) {
            newElement[i].Load();
        }

        _signalBus.Fire(new SignalGameIsLoad());
    }
}