using UnityEngine;

[CreateAssetMenu(fileName = "BackgroundItem", menuName = "ScriptableObjects/BackgroundItem", order = 1)]
public class BackgroundItem : ScriptableObject {
    public int Order;
    public Sprite Icon;
    public float Price;
}
