using System.Collections.Generic;
using UnityEngine;
using Enums;

[CreateAssetMenu(fileName = "UpgradeConfig", menuName = "ScriptableObjects/UpgradeConfig", order = 1)]
public class UpgradeConfig : ScriptableObject {
    public UpgradeType UpgradeType;
    public List<float> Prices;
    public string Name;
}