namespace Game.Scripts.UI.Panels {
    public class AirplaneReplaceButton : BaseReplaceButton {
        public override ReplaceButtonType ReplaceButtonType => ReplaceButtonType.Airplane;

        protected override void ClickButton() {
            var panelType = UIService.GetPanel<AirplaneShopList>();
            CustomizationPanel.ReplaceButtonSetSelect(ReplaceButtonType, panelType);
            UIService.HidePanelBypassStack<BackgroundShopList>();
            UIService.OpenPanel<AirplaneShopList>();
        }
    }
}