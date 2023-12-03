using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Zenject;

namespace Game.Scripts.UI.Panels {
    [RequireComponent(typeof(Button))]
    public class BackButton : MonoBehaviour {
        private UIService _uIService;

        [Inject]
        private void Construct(UIService uIService) {
            _uIService = uIService;
        }

        private void Start() {
            GetComponent<Button>().RemoveAllAndSubscribeButton(ReturnToMainMenu);
        }

        private void ReturnToMainMenu() {
            _uIService.HideAllPanels(new List<UIPanel> { _uIService.GetPanel<MainScreenPanel>() });
            _uIService.OpenPanel<MainScreenPanel>();
        }
    }
}
