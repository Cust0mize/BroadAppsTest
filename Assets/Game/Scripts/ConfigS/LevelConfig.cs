using System;

[Serializable]
public class LevelConfig {
    public int StartValue;
    public int EndValue;
    public int Level;

    public object Clone() {
        return new LevelConfig {
            StartValue = StartValue,
            EndValue = EndValue,
            Level = Level,
        };
    }
}
