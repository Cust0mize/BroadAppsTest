using System;
using UnityEngine.UI;
using UnityEngine;

namespace Game.Scripts.UI.Panels {
    [RequireComponent(typeof(Button))]
    public class GameButton : MonoBehaviour {
        private Action _clickEvent;

        [field: SerializeField] public GameButtonType ButtonType { get; private set; }
        public Button Button { get; private set; }

        public void Init(Action clickEvent) {
            _clickEvent = clickEvent;
            GetComponent<Button>().RemoveAllAndSubscribeButton(Click);
        }

        public void Click() {
            _clickEvent?.Invoke();
        }
    }

    public enum GameButtonType {
        Bid,
        CashOut,
        Cancel
    }
}