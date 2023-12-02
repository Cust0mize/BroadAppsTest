public struct SignalUpdateLevel {
    public int NewLevelIndex { get; private set; }
    public int NewStartValue { get; private set; }
    public int NewEndValue { get; private set; }
    public int NewLevelProgressValue { get; private set; }

    public SignalUpdateLevel(int levelIndex, int startValue, int endValue, int levelProgressValue) {
        NewLevelProgressValue = levelProgressValue;
        NewLevelIndex = levelIndex;
        NewStartValue = startValue;
        NewEndValue = endValue;
    }
}
