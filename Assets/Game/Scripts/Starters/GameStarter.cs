using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameStarter : MonoBehaviour {
    private List<ILoadableElement> _loadableElements = new();

    [Inject]
    private void Construct(
    List<ILoadableElement> loadableElements
    ) {
        _loadableElements = loadableElements;
    }

    private void Start() {
        List<ILoadableElement> newElement = _loadableElements;
        _loadableElements.Sort((x, y) => x.Order.CompareTo(y.Order));
        for (int i = 0; i < newElement.Count; i++) {
            newElement[i].Load();
        }
    }
}