using Game.Scripts.Signal;
using Zenject;
using System;

public class EveryDayCounterService : ITickable {
    private SignalBus _signalBus;
    private GameData _gameData;
    private bool _gameIsLoad;

    [Inject]
    private void Construct(
        SignalBus signalBus,
        GameData gameData
    ) {
        _signalBus = signalBus;
        _gameData = gameData;
        _signalBus.Subscribe<SignalGameIsLoad>(Init);
    }

    public void Tick() {
        if (_gameIsLoad) {
            CheckReloadTime();
        }
    }

    public string GetTimeToReload() {
        TimeSpan updateIn = GetRefreshTimeSpan();
        string formattedText = $"{(int)updateIn.TotalHours:00}:{updateIn.Minutes:00}:{updateIn.Seconds:00}";
        return formattedText;
    }

    public TimeSpan GetRefreshTimeSpan() {
        DateTime currentTimeNow = DateTime.Now;
        DateTime lastRefreshTime = _gameData.LastEntryTime;
        DateTime refreshTime = lastRefreshTime.AddDays(1);
        TimeSpan refrestIn = refreshTime - currentTimeNow;
        return refrestIn;
    }

    private void CheckReloadTime() {
        if (GetRefreshTimeSpan() < -TimeSpan.FromDays(1)) {
            _gameData.DayIndex = 0;
        }
        else if (GetRefreshTimeSpan() <= TimeSpan.Zero) {
            _gameData.LastEntryTime = DateTime.Now;
            _gameData.DayIndex++;
            _signalBus.Fire(new SignalNewDay());
        }
    }

    private void Init() {
        _gameIsLoad = true;
    }
}