using UnityEngine;
using TMPro;

namespace Game.Scripts.UI.Panels.Record {
    public class BestRecordElement : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _gameTimeTextUI;
        [SerializeField] private TextMeshProUGUI _rewardTextUI;

        public void Init(BestRecordInfo bestRecordInfo) {
            _gameTimeTextUI.text = $"{bestRecordInfo.GameTime:f0}Sec";
            _rewardTextUI.text = $"${bestRecordInfo.Reward:f2}";
            gameObject.SetActive(true);
        }
    }
}