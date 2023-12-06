using UnityEngine.UI;
using UnityEngine;

namespace Game.Scripts.UI.Panels {
    public class MorePanel : UIPanel {
        [SerializeField] private Button _changeTheBalanceButton;
        [SerializeField] private Button _shareButton;
        [SerializeField] private Button _usageButton;
        [SerializeField] private Button _rateButton;

        private string _shareURL = "";
        private string _usageURL = "";
        private string _rateURL = "";

        private void Start() {
            _changeTheBalanceButton.RemoveAllAndSubscribeButton(() => UIService.OpenPanel<ConfirmRemoveBalancePanel>());
            //_shareButton.RemoveAllAndSubscribeButton(() => Application.OpenURL(_shareURL));
            //_usageButton.RemoveAllAndSubscribeButton(() => Application.OpenURL(_usageURL));
            //_rateButton.RemoveAllAndSubscribeButton(() => Application.OpenURL(_rateURL));
        }
    }
}