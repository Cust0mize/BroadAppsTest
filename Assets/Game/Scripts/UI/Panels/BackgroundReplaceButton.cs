﻿namespace Game.Scripts.UI.Panels {
    public class BackgroundReplaceButton : BaseReplaceButton {
        public override ReplaceButtonType ReplaceButtonType { get => ReplaceButtonType.Background; }

        protected override void ClickButton() {
            var panelType = UIService.GetPanel<BackgroundShopList>();
            CustomizationPanel.ReplaceButtonSetSelect(ReplaceButtonType, panelType);
            UIService.HidePanelBypassStack<AirplaneShopList>();
            UIService.OpenPanel<BackgroundShopList>();
        }
    }
}