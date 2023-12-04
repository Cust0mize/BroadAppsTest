using UnityEngine;
using TMPro;

namespace Game.Scripts.UI.Panels.Record {
    public class AllGameRecordElement : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _dateTimeTextUI;
        [SerializeField] private TextMeshProUGUI _gameTimeTextUI;
        [SerializeField] private TextMeshProUGUI _balanceTextUI;
        [SerializeField] private TextMeshProUGUI _rewardTextUI;

        public void Init(AllGameRecordInfo allGameRecordInfo) {
            _dateTimeTextUI.text = allGameRecordInfo.DateTime.ToString("dd:MM:yy:HH:mm");
            _gameTimeTextUI.text = $"{allGameRecordInfo.GameTime:f0}Sec";
            _balanceTextUI.text = $"${allGameRecordInfo.Balance:f2}";
            _rewardTextUI.text = $"${allGameRecordInfo.Reward:f2}";
            transform.SetAsLastSibling();
            gameObject.SetActive(true);
        }
    }
}