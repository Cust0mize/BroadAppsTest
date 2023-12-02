namespace Game.Scripts.Signal {
    public struct SignalMoneyUpdate {
        public float NewMoneyValue { get; private set; }

        public SignalMoneyUpdate(float moneyValue) {
            NewMoneyValue = moneyValue;
        }
    }
}