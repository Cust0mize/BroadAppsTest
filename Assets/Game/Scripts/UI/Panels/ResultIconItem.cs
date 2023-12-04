using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Game.Scripts.UI.Panels {
    public class ResultIconItem : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _nameTextUI;
        [SerializeField] private Image[] _winImages;

        public void Init(string name, bool isWin) {
            _nameTextUI.text = name;

            for (int i = 0; i < _winImages.Length; i++) {
                _winImages[i].gameObject.SetActive(isWin);
            }
        }
    }
}