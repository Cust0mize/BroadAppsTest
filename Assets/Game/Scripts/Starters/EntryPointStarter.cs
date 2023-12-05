using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class EntryPointStarter : MonoBehaviour {
    [SerializeField] private Image _loadImage;
    private SceneLoaderService _sceneLoaderService;
    private ConfigService _configService;
    private SaveSystem _saveSystem;
    private GameData _gameData;

    [Inject]
    private void Construct(
    SceneLoaderService sceneLoaderService,
    ConfigService configService,
    SaveSystem saveSystem,
    GameData gameData
    ) {
        _sceneLoaderService = sceneLoaderService;
        _configService = configService;
        _saveSystem = saveSystem;
        _gameData = gameData;
    }

    private async void Start() {
        await RemoteConfig.Init();

        _loadImage.fillAmount = 0;
        _gameData.Init(_saveSystem);
        _gameData.SetValue(_saveSystem.LoadFromDevice());
        _configService.LoadConfigs();
        _loadImage.DOFillAmount(1, 0.25f).SetEase(Ease.Linear).OnComplete(() => _sceneLoaderService.LoadScene(SceneName.Game));
    }
}
