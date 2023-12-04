using UnityEngine;
using TMPro;

namespace Game.Scripts.UI.Panels {
    public class NameAndRewardItem : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _rewardUI;
        [SerializeField] private TextMeshProUGUI _nameUI;

        public void Init(string name, float rewardUI) {
            _rewardUI.text = $"${rewardUI:f0}";
            _nameUI.text = name;
        }
    }
}