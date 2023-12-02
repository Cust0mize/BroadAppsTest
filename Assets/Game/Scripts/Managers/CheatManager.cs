using Game.Scripts.Game;
using UnityEngine;
using Zenject;

public class CheatManager : MonoBehaviour {
    private CurrenciesService _currenciesService;
    private LevelService _levelService;

    [Inject]
    private void Construct(
        CurrenciesService currenciesService,
        LevelService levelService
    ) {
        _currenciesService = currenciesService;
        _levelService = levelService;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            _levelService.AddLevelProgressValue(900);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            _currenciesService.RemoveMoney(100);
        }        
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            _currenciesService.AddMoney(100);
        }
    }
}
