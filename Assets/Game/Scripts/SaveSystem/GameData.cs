using System.Collections.Generic;
using Game.Scripts.UI.Panels;
using Unity.VisualScripting;
using System;
using UnityEngine;

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

    private Dictionary<UpgradeType, int> _upgradeLevels = new();
    public Dictionary<UpgradeType, int> UpgradeLevels {
        get { return _upgradeLevels; }
        set {
            UpgradeLevels.AddRange(value);
            Save();
        }
    }

    private Dictionary<ShopListType, int> _selectCustomElement = new();
    public Dictionary<ShopListType, int> SelectCustomElement {
        get { return _selectCustomElement; }
        set {
            _selectCustomElement.AddRange(value);
            Save();
        }
    }    
    
    private Dictionary<AchivmentType, float> _achivmentValue = new();
    public Dictionary<AchivmentType, float> AchivmentValue {
        get { return _achivmentValue; }
        set {
            _achivmentValue.AddRange(value);
            Save();
        }
    }    
    
    private Dictionary<AchivmentType, List<float>> _takedReward = new();
    public Dictionary<AchivmentType, List<float>> TakedReward {
        get { return _takedReward; }
        set {
            _takedReward.AddRange(value);
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
        TakedReward = gameData.TakedReward;
        CurrentLevel = gameData.CurrentLevel;
        CurrentMoney = gameData.CurrentMoney;
        BuyShopItems = gameData.BuyShopItems;
        UpgradeLevels = gameData.UpgradeLevels;
        AchivmentValue = gameData.AchivmentValue;
        CurrentGamemode = gameData.CurrentGamemode;
        CurrentComplexity = gameData.CurrentComplexity;
        SelectCustomElement = gameData.SelectCustomElement;
        CurrentLevelProgressValue = gameData.CurrentLevelProgressValue;
    }

    public void ResetGame() {
        CurrentLevel = 0;
        CurrentMoney = 5000;
        CurrentGamemode = Gamemode.Classic;
        CurrentComplexity = Complexity.Easy;
        CurrentLevelProgressValue = 0;
        BuyShopItems = new();
        UpgradeLevels = new();
        AchivmentValue = new();
        SelectCustomElement = new();
    }
}
