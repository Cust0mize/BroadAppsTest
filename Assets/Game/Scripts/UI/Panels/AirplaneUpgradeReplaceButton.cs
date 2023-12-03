namespace Game.Scripts.UI.Panels {
    public class AirplaneUpgradeReplaceButton : BaseReplaceButton {
        public override ReplaceButtonType ReplaceButtonType => ReplaceButtonType.AirplaneUpgrade;

        protected override void ClickButton() {
            //var panelType = UIService.GetPanel<BackgroundShopList>();
            //CustomizationPanel.ReplaceButtonSetSelect(ReplaceButtonType, panelType);
            print("AirplaneUpgrade");
        }
    }
}