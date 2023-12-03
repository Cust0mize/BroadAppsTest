using Game.Scripts.Game;
using UnityEngine.UI;
using UnityEngine;
using Zenject;
using TMPro;

namespace Game.Scripts.UI.Panels {
    public class UpgradeElement : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _upgradeNameUI;
        [SerializeField] private TextMeshProUGUI _maxText;
        [SerializeField] private TextMeshProUGUI _buttonText;
        [SerializeField] private UpgradeConfig _upgradeConfig;
        [SerializeField] private StepImage[] _imageSteps;
        [SerializeField] private Button _buyButton;
        private int _currentLevel;

        private CurrenciesService _currenciesService;
        private GameData _gameData;

        [Inject]
        private void Construct(
        CurrenciesService currenciesService,
        GameData gameData
        ) {
            _currenciesService = currenciesService;
            _gameData = gameData;
        }

        public void Init() {
            if (!_gameData.UpgradeLevels.ContainsKey(_upgradeConfig.UpgradeType)) {
                _gameData.UpgradeLevels.Add(_upgradeConfig.UpgradeType, _currentLevel);
            }
            _currentLevel = _gameData.UpgradeLevels[_upgradeConfig.UpgradeType];
            _upgradeNameUI.text = _upgradeConfig.Name;

            bool isFull = true;
            for (int i = 0; i < _imageSteps.Length; i++) {
                _imageSteps[i].Init();
                if (i < _currentLevel) {
                    _imageSteps[i].Activate();
                }
                else {
                    _imageSteps[i].Disable();
                    isFull = false;
                }
            }

            if (isFull) {
                DisableButton();
            }
            else {
                _buttonText.text = $"Buy: {_upgradeConfig.Prices[_currentLevel]}";
                _buyButton.RemoveAllAndSubscribeButton(BuyClick);
                _maxText.gameObject.SetActive(false);
            }
        }

        private void BuyClick() {
            if (_currentLevel < _upgradeConfig.Prices.Count) {
                if (_currenciesService.RemoveMoney(_upgradeConfig.Prices[_currentLevel])) {
                    _imageSteps[_currentLevel].Activate();
                    _currentLevel++;

                    _gameData.UpgradeLevels[_upgradeConfig.UpgradeType] = _currentLevel;
                    _gameData.Save();

                    if (_currentLevel >= _upgradeConfig.Prices.Count) {
                        DisableButton();
                    }
                    else {
                        _buttonText.text = $"Buy: {_upgradeConfig.Prices[_currentLevel]}";
                    }
                }
            }
        }

        private void DisableButton() {
            _buyButton.gameObject.SetActive(false);
            _maxText.gameObject.SetActive(true);
        }
    }
}