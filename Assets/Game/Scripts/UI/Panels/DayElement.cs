using UnityEngine;
using TMPro;
using Zenject;

namespace Game.Scripts.UI.Panels {

    public class DayElement : MonoBehaviour {
        [SerializeField] private GameObject _disableElement;
        [SerializeField] private GameObject _enableElement;
        private TextMeshProUGUI[] _dayTextsUI;

        public void Init(string dayName, bool isActive) {
            _dayTextsUI = transform.GetComponentsInChildren<TextMeshProUGUI>();

            _disableElement.gameObject.SetActive(!isActive);
            _enableElement.gameObject.SetActive(isActive);

            for (int i = 0; i < _dayTextsUI.Length; i++) {
                _dayTextsUI[i].text = dayName;
            }
        }
    }

    public enum Days {
        Mon,
        Tue,
        Web,
        Thu,
        Fri,
        Sat,
        Sun
    }
}
