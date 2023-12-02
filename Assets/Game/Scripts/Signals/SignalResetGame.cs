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

    public struct SignalBuyNewElemetn {
        public BackgroundItem BackgroundItem;

        public SignalBuyNewElemetn(BackgroundItem backgroundItem) {
            BackgroundItem = backgroundItem;
        }
    }
}