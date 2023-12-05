namespace Game.Scripts.Game.GameValue {
    public class StartTripValue : BaseStartGameValue {
        public float StartDownMultiplyValue { get; }
        public float DownMultiplyValue { get; }

        public StartTripValue(float startDownMultiplyValue) {
            StartDownMultiplyValue = startDownMultiplyValue;
            DownMultiplyValue = startDownMultiplyValue;
        }
    }
}