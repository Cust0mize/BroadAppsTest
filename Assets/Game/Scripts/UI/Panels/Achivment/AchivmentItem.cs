using System.Collections.Generic;
using Game.Scripts.Game;
using UnityEngine.UI;
using UnityEngine;
using Zenject;
using TMPro;
using Enums;

namespace Game.Scripts.UI.Panels.Achivment {
    public class AchivmentItem : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _revardValueFaceButtonUI;
        [SerializeField] private TextMeshProUGUI _achivmentNameUI;
        [SerializeField] private TextMeshProUGUI _revardValueUI;
        [SerializeField] private TextMeshProUGUI _recivedTextUI;
        [SerializeField] private Image _faceButtonImage;
        [SerializeField] private Sprite _complitedBack;
        [SerializeField] private Button _revardButton;
        [SerializeField] private Image _backImage;
        [SerializeField] private FloatBar _bar;

        public AchivmentType AchivmentType { get; private set; }
        private AchivmentConfig _achivmentConfig;
        private float _targetValue;
        private float _rewardValue;

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

        public void Init(AchivmentConfig achivmentConfig, bool isComplite) {
            _revardButton.RemoveAllAndSubscribeButton(GetRewardClick);
            _achivmentConfig = achivmentConfig;
            AchivmentType = achivmentConfig.AchivmentType;
            _targetValue = _achivmentConfig.TargetValue;
            _rewardValue = _achivmentConfig.RewardValue;

            _achivmentNameUI.text = achivmentConfig.AchivmentName;
            _revardValueUI.text = $"+${achivmentConfig.RewardValue}";
            _revardValueFaceButtonUI.text = $"+${achivmentConfig.RewardValue}";

            if (_gameData.AchivmentValue.ContainsKey(AchivmentType)) {
                _bar.UpdateBar(0, _targetValue, _gameData.AchivmentValue[AchivmentType]);
                _bar.UpdateStartValue(_gameData.AchivmentValue[AchivmentType], _targetValue);
            }
            else {
                _gameData.AchivmentValue.Add(AchivmentType, 0);
                _bar.UpdateBar(0, _targetValue, _gameData.AchivmentValue[AchivmentType]);
                _bar.UpdateStartValue(_gameData.AchivmentValue[AchivmentType], _targetValue);
            }
            if (isComplite) {
                SetComplitedState();
            }
            else {
                SetNotComplitedState();
            }
        }

        public void UpdateValue(float newValue) {
            _bar.SetCurrentValue(newValue);
            _bar.UpdateStartValue(newValue, _targetValue);
        }

        private void GetRewardClick() {
            if (_gameData.AchivmentValue[AchivmentType] > _targetValue) {
                _currenciesService.AddMoney(_rewardValue);
                SetComplitedState();
                if (_gameData.TakedReward.ContainsKey(AchivmentType)) {
                    _gameData.TakedReward[AchivmentType].Add(_targetValue);
                }
                else {
                    _gameData.TakedReward.Add(AchivmentType, new List<float> { _targetValue });
                }
                _gameData.Save();
            }
        }

        public void SetComplitedState() {
            _faceButtonImage.gameObject.SetActive(true);
            _recivedTextUI.gameObject.SetActive(true);
            _backImage.sprite = _complitedBack;
            _bar.gameObject.SetActive(false);
        }

        private void SetNotComplitedState() {
            _faceButtonImage.gameObject.SetActive(false);
            _recivedTextUI.gameObject.SetActive(false);
            _bar.gameObject.SetActive(true);
        }
    }
}