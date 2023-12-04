namespace Game.Scripts.UI.Panels.Customization {
    public class AirplaneUpgradePanel : UIPanel {
        private UpgradeElement[] _upgradeElements;

        private void Start() {
            gameObject.SetActive(false);
            _upgradeElements = transform.GetComponentsInChildren<UpgradeElement>();
        }

        public override void Show() {
            base.Show();
            for (int i = 0; i < _upgradeElements.Length; i++) {
                _upgradeElements[i].Init();
            }
        }
    }
}