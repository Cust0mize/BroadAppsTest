using UnityEngine;

[CreateAssetMenu(fileName = "RouteConfig", menuName = "ScriptableObjects/RouteConfig", order = 1)]
public class RouteConfig : ScriptableObject {
    public string RouteName => $"{StartCity}-{EndSityCity}";
    public float RouteDistance;
    public float RoutePrice;
    public string EndSityCity;
    public string StartCity;
}
