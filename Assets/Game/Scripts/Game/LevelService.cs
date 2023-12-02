using System.Linq;
using Zenject;

public class LevelService : ILoadableElement {
    public int CurrentLevelProgressValue { get; private set; }
    public int CurrentLevel { get; private set; }
    private LevelConfig _levelConfig;

    public int Order => 1;

    private ConfigService _configService;
    private SignalBus _signalBus;
    private UIService _uIService;
    private GameData _gameData;

    [Inject]
    private void Construct(
    ConfigService configService,
    UIService uIService,
    SignalBus signalBus,
    GameData gameData
    ) {
        _configService = configService;
        _signalBus = signalBus;
        _uIService = uIService;
        _gameData = gameData;
    }

    public void AddLevelProgressValue(int value) {
        CurrentLevelProgressValue += value;
        _gameData.CurrentLevelProgressValue = CurrentLevelProgressValue;

        if (CurrentLevelProgressValue >= _levelConfig.EndValue) {
            AddLevel();
        }

        UpdateAllLevelInfo();
    }

    public void Load() {
        CurrentLevelProgressValue = _gameData.CurrentLevelProgressValue;
        CurrentLevel = _gameData.CurrentLevel;
        _levelConfig = _configService.LevelConfigs.FirstOrDefault(x => x.Level == CurrentLevel);
        UpdateAllLevelInfo();
    }

    private void AddLevel() {
        CurrentLevel++;
        _gameData.CurrentLevel = CurrentLevel;
        _levelConfig = _configService.LevelConfigs.FirstOrDefault(x => x.Level == CurrentLevel);
    }

    private void UpdateAllLevelInfo() {
        _signalBus.Fire(new SignalUpdateLevel(CurrentLevel, _levelConfig.StartValue, _levelConfig.EndValue, _gameData.CurrentLevelProgressValue));
    }
}

public class CurrenciesService {
    public float CurrentMoney;

    public void AddMoney() {

    }

    public void RemoveMoney() {

    }
}