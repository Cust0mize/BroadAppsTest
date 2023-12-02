using UnityEngine;
using Zenject;

public class CheatManager : MonoBehaviour {
    private LevelService _levelService;

    [Inject]
    private void Construct(LevelService levelService) {
        _levelService = levelService;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            _levelService.AddLevelProgressValue(900);
        }
    }
}
