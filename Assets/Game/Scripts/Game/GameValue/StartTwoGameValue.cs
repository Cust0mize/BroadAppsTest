using System;

namespace Game.Scripts.Game.GameValue {
    public class StartTwoGameValue : BaseStartGameValue {
        public TimeSpan CurrentTimeSpan { get; private set; }
        public TimeSpan StartTimeSpan { get; }
        public int PlayerCount { get; }
        public int CurrentPlayerIndex { get; private set; }
        public float FirstResultValue { get; }
        public float SecondResultValue { get; }

        public StartTwoGameValue(
            TimeSpan startTimeSpan,
            int playerCount
        ) : base() {
            PlayerCount = playerCount;
            CurrentPlayerIndex = 1;
            FirstResultValue = 0;
            SecondResultValue = 0;
            StartTimeSpan = startTimeSpan;
        }
    }
}