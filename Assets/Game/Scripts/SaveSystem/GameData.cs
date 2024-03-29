﻿using Game.Scripts.UI.Panels.Record;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using Enums;

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

    private AllGameRecordInfo[] _allGameRecords = new AllGameRecordInfo[5];
    public AllGameRecordInfo[] AllGameRecords {
        get { return _allGameRecords; }
        set {
            _allGameRecords = value;
            Save();
        }
    }

    private BestRecordInfo[] _bestGameRecords = new BestRecordInfo[3];
    public BestRecordInfo[] BestGameRecords {
        get { return _bestGameRecords; }
        set {
            _bestGameRecords = value;
            Save();
        }
    }

    private List<string> _buyRoutes = new();
    public List<string> BuyRoutes {
        get { return _buyRoutes; }
        set {
            _buyRoutes = value;
            Save();
        }
    }

    private int _currentLevelPlane;
    public int CurrentLevelPlane {
        get { return _currentLevelPlane; }
        set {
            _currentLevelPlane = value;
            Save();
        }
    }

    private DateTime _lastEntryTime = DateTime.Now - TimeSpan.FromDays(1);
    public DateTime LastEntryTime {
        get { return _lastEntryTime; }
        set {
            _lastEntryTime = value;
            Save();
        }
    }

    private int _dayIndex;
    public int DayIndex {
        get { return _dayIndex; }
        set {
            _dayIndex = value;
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

        DayIndex = gameData.DayIndex;
        BuyRoutes = gameData.BuyRoutes;
        TakedReward = gameData.TakedReward;
        CurrentLevel = gameData.CurrentLevel;
        CurrentMoney = gameData.CurrentMoney;
        BuyShopItems = gameData.BuyShopItems;
        LastEntryTime = gameData.LastEntryTime;
        UpgradeLevels = gameData.UpgradeLevels;
        AllGameRecords = gameData.AllGameRecords;
        AchivmentValue = gameData.AchivmentValue;
        BestGameRecords = gameData.BestGameRecords;
        CurrentGamemode = gameData.CurrentGamemode;
        CurrentLevelPlane = gameData.CurrentLevelPlane;
        CurrentComplexity = gameData.CurrentComplexity;
        SelectCustomElement = gameData.SelectCustomElement;
        CurrentLevelProgressValue = gameData.CurrentLevelProgressValue;
    }

    public void ResetGame() {
        DayIndex = 0;
        BuyRoutes.Clear();
        TakedReward.Clear();
        LastEntryTime = DateTime.Now;
        CurrentLevel = 0;
        CurrentMoney = 5000;
        CurrentGamemode = Gamemode.Classic;
        CurrentComplexity = Complexity.Easy;
        CurrentLevelProgressValue = 0;
        BuyShopItems.Clear();
        UpgradeLevels.Clear();
        AchivmentValue.Clear();

        SelectCustomElement.Clear();
        AllGameRecords = new AllGameRecordInfo[5];
        BestGameRecords = new BestRecordInfo[3];
        CurrentLevelPlane = 0;
        Save();
    }
}
