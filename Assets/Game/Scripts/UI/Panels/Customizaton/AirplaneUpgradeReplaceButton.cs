using System.Collections.Generic;

namespace Game.Scripts.UI.Panels.Customization {
    public class AirplaneUpgradeReplaceButton : BaseReplaceButton {
        public override ReplaceButtonType ReplaceButtonType => ReplaceButtonType.AirplaneUpgrade;

        protected override void ClickButton() {
            CustomizationPanel.ReplaceButtonSetSelect(ReplaceButtonType);
            UIService.HideAllPanels(new List<UIPanel>() { UIService.GetPanel<CustomizationPanel>() });
            UIService.OpenPanel<AirplaneUpgradePanel>();
        }
    }
}
