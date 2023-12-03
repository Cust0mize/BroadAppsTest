using Game.Scripts.UI.Panels;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeConfig", menuName = "ScriptableObjects/UpgradeConfig", order = 1)]
public class UpgradeConfig : ScriptableObject {
    public UpgradeType UpgradeType;
    public List<float> Prices;
    public string Name;
}

public enum AchivmentType {
    Earn,
    Fly
}