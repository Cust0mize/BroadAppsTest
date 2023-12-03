using Game.Scripts.UI.Panels;

namespace Game.Scripts.Signal {
    public struct SignalResetGame {

    }

    public struct SignalNoMoney {

    }

    public struct SignalStartGame {

    }

    public struct SignalStopGame {
        public bool IsWin { get; private set; }

        public SignalStopGame(bool isWin) {
            IsWin = isWin;
        }
    }

    public struct SignalLooseGame {

    }

    public struct SignalUpdateAchivment {
        public AchivmentType AchivmentType { get; private set; }
        public float Value { get; private set; }

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

    public struct SignalBuyNewElemetn {
        public BackgroundItem BackgroundItem;
        public ShopListType ShopListType;

        public SignalBuyNewElemetn(BackgroundItem backgroundItem, ShopListType shopListType) {
            ShopListType = shopListType;
            BackgroundItem = backgroundItem;
        }
    }
}