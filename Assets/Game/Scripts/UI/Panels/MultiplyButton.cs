using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

namespace Game.Scripts.UI.Panels {
    public class MultiplyButton : MonoBehaviour {
        [field: SerializeField] public float Multiply { get; private set; }
        [SerializeField] private Color _unselectionColor;
        [SerializeField] private Color _selectionColor;
        private Image _image;
        private TextMeshProUGUI _textMesh;
        private Action<MultiplyButton> _multiplyButtonClick;

        public void Init(Action<MultiplyButton> multiplyButtonClick) {
            _image = GetComponent<Image>();
            _multiplyButtonClick = multiplyButtonClick;
            transform.GetComponent<Button>().RemoveAllAndSubscribeButton(SelectButton);
            _textMesh = transform.GetComponentInChildren<TextMeshProUGUI>();
            _textMesh.text = $"{Multiply:f1}X";
        }

        private void SelectButton() {
            _multiplyButtonClick?.Invoke(this);
        }

        public void Select() {
            _image.color = _selectionColor;
        }

        public void Unselect() {
            _image.color = _unselectionColor;
        }
    }
}