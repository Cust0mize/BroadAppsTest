using Game.Scripts.UI.Panels;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

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

    private float _currentMoney = 5000;
    public float CurrentMoney {
        get { return _currentMoney; }
        set {
            _currentMoney = value;
            Save();
        }
    }

    private Dictionary<ShopListType, List<int>> _buyShopItems = new();
    public Dictionary<ShopListType, List<int>> BuyShopItems {
        get { return _buyShopItems; }
        set {
            BuyShopItems.AddRange(value);
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
        CurrentMoney = gameData.CurrentMoney;
        BuyShopItems = gameData.BuyShopItems;
        CurrentGamemode = gameData.CurrentGamemode;
        CurrentComplexity = gameData.CurrentComplexity;
        CurrentLevelProgressValue = gameData.CurrentLevelProgressValue;
    }

    public void ResetGame() {
        CurrentLevel = 0;
        CurrentMoney = 5000;
        CurrentGamemode = Gamemode.Classic;
        CurrentComplexity = Complexity.Easy;
        CurrentLevelProgressValue = 0;
        BuyShopItems = new();
    }
}
