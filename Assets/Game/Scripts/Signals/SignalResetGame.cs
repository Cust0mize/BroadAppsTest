using Game.Scripts.UI.Panels.Customization;
using System;
using Enums;

namespace Game.Scripts.Signal {
    public struct SignalResetGame {

    }

    public struct SignalNoMoney {

    }

    public struct SignalStartGame {

    }

    public struct SignalWinGame {
        public float RewardValue;
        public float GameTime;

        public SignalWinGame(float rewardValue, float gameTime) {
            RewardValue = rewardValue;
            GameTime = gameTime;
        }
    }

    public struct SignalStopGame {
        public bool IsWin { get; private set; }

        public SignalStopGame(bool isWin) {
            IsWin = isWin;
        }
    }

    public struct SignalLooseGame {

    }

    public struct SignalStartTwoPersonModeGame {
        public TimeSpan GameTime { get; }
        public string FirstPersonName { get; }
        public string SecondPersonName { get; }

        public SignalStartTwoPersonModeGame(TimeSpan timeSpan, string text1, string text2) {
            SecondPersonName = text2;
            FirstPersonName = text1;
            GameTime = timeSpan;
        }
    }

    public struct SignalStartTaskModeGame {
        public TimeSpan GameTime { get; }
        public float TaskValue { get; }

        public SignalStartTaskModeGame(TimeSpan timeSpan, float taskValue) {
            TaskValue = taskValue;
            GameTime = timeSpan;
        }
    }

    public struct SignalUpdateResultTwoGame {
        public int WinPlayerIndex { get; }
        public float[] Rewards { get; }
        public string[] Names { get; }

        public SignalUpdateResultTwoGame(int winPlayerIndex, float[] rewards, string[] names) {
            WinPlayerIndex = winPlayerIndex;
            Rewards = rewards;
            Names = names;
        }
    }

    public struct SignalBuyRoute {
        public RouteConfig RouteConfig;

        public SignalBuyRoute(RouteConfig routeConfig) {
            RouteConfig = routeConfig;
        }
    }

    public struct SignalSelectRoute {
        public RouteConfig RouteConfig;

        public SignalSelectRoute(RouteConfig routeConfig) {
            RouteConfig = routeConfig;
        }
    }

    public struct SignalUpdateAchivment {
        public AchivmentType AchivmentType { get; }
        public float Value { get; }

        public SignalUpdateAchivment(AchivmentType achivmentType, float value) {
            AchivmentType = achivmentType;
            Value = value;
        }
    }

    public struct SignalSelectAvailableElement {
        public AvailableElement AvailableElement;

        public SignalSelectAvailableElement(AvailableElement availableElement) {
            AvailableElement = availableElement;
        }
    }

    public struct OpenGamePanel {
    }

    public struct SignalUpdateAirplane {
    }

    public struct SignalBuyNewElemetn {
        public BackgroundItem BackgroundItem;
        public ShopListType ShopListType;

        public SignalBuyNewElemetn(BackgroundItem backgroundItem, ShopListType shopListType) {
            ShopListType = shopListType;
            BackgroundItem = backgroundItem;
        }
    }

    public struct SignalSaveRecord {
        public float GameTime { get; }
        public float Reward { get; }

        public SignalSaveRecord(float gameTime, float reward) {
            GameTime = gameTime;
            Reward = reward;
        }
    }
}