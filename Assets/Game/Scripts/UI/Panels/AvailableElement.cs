using UnityEngine.UI;
using UnityEngine;

namespace Game.Scripts.UI.Panels {
    public class AvailableElement : MonoBehaviour {
        [SerializeField] private Image _selectionRamkImage;
        [SerializeField] private Image _backIconImage;

        public void Init(Sprite sprite) {
            _backIconImage.sprite = sprite;
        }
    }
}