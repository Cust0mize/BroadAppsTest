using UnityEngine.UI;
using UnityEngine;

namespace Game.Scripts.UI.Panels {
    [RequireComponent(typeof(Image))]
    public class StepImage : MonoBehaviour {
        [SerializeField] private Color _activeColor;
        private Image _image;

        public void Init() {
            _image = GetComponent<Image>();
        }

        public void Activate() {
            _image.color = _activeColor;
        }

        public void Disable() {
            _image.color = Color.white;
        }
    }
}