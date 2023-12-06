using Cysharp.Threading.Tasks;
using Game.Scripts.Services;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class EntryPointStarter : MonoBehaviour {
    [SerializeField] private Image _loadImage;
    private BackendService _backendService;
    private SceneLoaderService _sceneLoaderService;
    private ConfigService _configService;
    private SaveSystem _saveSystem;
    private GameData _gameData;

    private bool _loaded = false;

    [Inject]
    private void Construct(
    SceneLoaderService sceneLoaderService,
    BackendService backendService,
    ConfigService configService,
    SaveSystem saveSystem,
    GameData gameData
    ) {
        _sceneLoaderService = sceneLoaderService;
        _backendService = backendService;
        _configService = configService;
        _saveSystem = saveSystem;
        _gameData = gameData;
    }

    private async void Start() {
        _loadImage.fillAmount = 0;
        _loadImage.DOFillAmount(1, 0.5f).SetEase(Ease.Linear).OnComplete(async () =>
        {
            await UniTask.WaitUntil(() => _loaded);
            _sceneLoaderService.LoadScene(SceneName.Game);
        });

        await RemoteConfig.Init();
        await _backendService.LoadValueFromServer();

        _gameData.Init(_saveSystem);
        _gameData.SetValue(_saveSystem.LoadFromDevice());
        _configService.LoadConfigs();
        _loaded = true;
    }
}
