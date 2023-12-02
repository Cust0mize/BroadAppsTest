using Game.Scripts.UI.Panels;
using System;

[Serializable]
public class GameData {
    private SaveSystem _saveSystem;

    #region Properties
    private int _currentLevel;
    public int CurrentLevel {
        get { return _currentLevel; }
        set {
            _currentLevel = value;
            Save();
        }
    }    
    
    private int _currentLevelProgressValue;
    public int CurrentLevelProgressValue {
        get { return _currentLevelProgressValue; }
        set {
            _currentLevelProgressValue = value;
            Save();
        }
    }    
    
    private Complexity _currentComplexity;
    public Complexity CurrentComplexity {
        get { return _currentComplexity; }
        set {
            _currentComplexity = value;
            Save();
        }
    }    
    
    private Gamemode _currentGamemode;
    public Gamemode CurrentGamemode {
        get { return _currentGamemode; }
        set {
            _currentGamemode = value;
            Save();
        }
    }

    #endregion

    public void Init(SaveSystem saveSystem) {
        _saveSystem = saveSystem;
    }

    public void Save() {
        if (_saveSystem != null) {
            _saveSystem.Save(this);
        }
    }

    public void SetValue(GameData gameData) {
        if (gameData == null) {
            return;
        }
        CurrentLevel = gameData.CurrentLevel;
        CurrentGamemode = gameData.CurrentGamemode;
        CurrentComplexity = gameData.CurrentComplexity;
        CurrentLevelProgressValue = gameData.CurrentLevelProgressValue;
    }

    public void ResetGame() {

    }
}
