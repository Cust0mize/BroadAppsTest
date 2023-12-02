using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class GameStarter : MonoBehaviour {
    private List<ILoadableElement> _loadableElements = new();
    private UIService _uIService;

    [Inject]
    private void Construct(
    UIService uIService,
    List<ILoadableElement> loadableElements
    ) {
        _loadableElements = loadableElements;
        _uIService = uIService;
    }

    private void Start() {
        List<ILoadableElement> newElement = _loadableElements;
        _loadableElements.Sort((x, y) => x.Order.CompareTo(y.Order));
        for (int i = 0; i < newElement.Count; i++) {
            newElement[i].Load();
        }
    }
}