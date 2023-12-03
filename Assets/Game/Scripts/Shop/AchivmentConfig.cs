using UnityEngine;

[CreateAssetMenu(fileName = "AchivmentConfig", menuName = "ScriptableObjects/AchivmentConfig", order = 1)]
public class AchivmentConfig : ScriptableObject {
    public string AchivmentName => GetAchivmentName();
    public AchivmentType AchivmentType;
    public float RewardValue;
    public float TargetValue;

    private string GetAchivmentName() {
        string result = string.Empty;
        if (AchivmentType == AchivmentType.Earn) {
            result = $"{AchivmentType} ${TargetValue}";
        }
        else {
            result = $"{AchivmentType} {TargetValue} Minutes";
        }
        return result;
    }
}
