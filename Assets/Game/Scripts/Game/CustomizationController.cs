using Game.Scripts.Signal;
using UnityEngine.UI;
using UnityEngine;
using Zenject;
using Enums;

namespace Game.Scripts.Game {
    public class CustomizationController : MonoBehaviour {
        [SerializeField] private Image _bacgroundImage;
        [SerializeField] private Image _playerImage;

        [Inject]
        private void Construct(
        SignalBus signalBus
        ) {
            signalBus.Subscribe<SignalSelectAvailableElement>(UpdateImage);
        }

        private void UpdateImage(SignalSelectAvailableElement signalSelectAvailableElement) {
            if (signalSelectAvailableElement.AvailableElement.ShopListType == ShopListType.Airplane) {
                _playerImage.sprite = signalSelectAvailableElement.AvailableElement.BackgroundItem.Icon;
            }
            else {
                _bacgroundImage.sprite = signalSelectAvailableElement.AvailableElement.BackgroundItem.Icon;
            }
        }
    }
}