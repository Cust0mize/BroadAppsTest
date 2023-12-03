using UnityEngine.UI;
using UnityEngine;
using Zenject;

namespace Game.Scripts.UI.Panels {
    [RequireComponent(typeof(Button))]
    public abstract class BaseReplaceButton : MonoBehaviour {
        protected CustomizationPanel CustomizationPanel { get; private set; }
        public abstract ReplaceButtonType ReplaceButtonType { get; }
        protected UIService UIService { get; private set; }

        private Image _buttonImage;
        private Color _startColor;
        private Button _button;

        [Inject]
        private void Construct(UIService uIService) {
            UIService = uIService;
        }

        public void Init(CustomizationPanel customizationPanel) {
            CustomizationPanel = customizationPanel;
            _button = GetComponent<Button>();
            _buttonImage = _button.GetComponent<Image>();
            _button.RemoveAllAndSubscribeButton(ClickButton);
            _startColor = _buttonImage.color;
        }

        public void SelectButton() {
            _buttonImage.color = _startColor;
        }

        public void UnselectButton() {
            _buttonImage.color = Color.clear;
        }

        protected abstract void ClickButton();
    }

    public enum ReplaceButtonType {
        Background,
        Airplane,
        AirplaneUpgrade
    }
}