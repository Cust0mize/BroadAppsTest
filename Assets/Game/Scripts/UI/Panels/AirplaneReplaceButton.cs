using System.Collections.Generic;

namespace Game.Scripts.UI.Panels {
    public class AirplaneReplaceButton : BaseReplaceButton {
        public override ReplaceButtonType ReplaceButtonType => ReplaceButtonType.Airplane;

        protected override void ClickButton() {
            var airplaneShop = UIService.GetPanel<AirplaneShopList>();
            CustomizationPanel.ReplaceButtonSetSelect(ReplaceButtonType);
            UIService.HideAllPanels(new List<UIPanel>() { UIService.GetPanel<CustomizationPanel>() });
            UIService.OpenPanel<AirplaneShopList>();
        }
    }
}