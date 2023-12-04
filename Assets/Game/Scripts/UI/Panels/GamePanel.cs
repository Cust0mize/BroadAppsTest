using Cysharp.Threading.Tasks;
using Game.Scripts.Signal;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using Zenject;
using System;
using TMPro;

namespace Game.Scripts.UI.Panels {
    public class GamePanel : UIPanel {
        [SerializeField] private TextMeshProUGUI _waitingTextUI;
        [SerializeField] private TextMeshProUGUI _levelTextUI;
        [SerializeField] private Image _waitingImageFill;
        [SerializeField] private Image _barRoot;
        private GameButton[] _gameButtons;
        private bool _startAwait;

        private Action _startGameClick;
        private Action _cashOutClick;
        private Action _cancelClick;
        private SignalBus _signalBus;
        private GameData _gameData;

        [Inject]
        private void Construct(
            SignalBus signalBus,
            GameData gameData
        ) {
            _signalBus = signalBus;
            _gameData = gameData;
            signalBus.Subscribe<SignalLooseGame>(LooseGame);
            signalBus.Subscribe<SignalUpdateLevel>(UpdateLevelNumber);
        }

        public void UpdateLevelNumber(SignalUpdateLevel signalUpdateLevel) {
            _levelTextUI.text = $"{signalUpdateLevel.NewLevelIndex + 1} Level";
        }

        public override void Show() {
            base.Show();
            _signalBus.Fire(new OpenGamePanel());
            _levelTextUI.gameObject.SetActive(_gameData.CurrentGamemode != Enums.Gamemode.Two);
        }

        public void UpdateButtonState(GameButtonType targetType) {
            for (int i = 0; i < _gameButtons.Length; i++) {
                if (_gameButtons[i].ButtonType == targetType) {
                    _gameButtons[i].gameObject.SetActive(true);
                }
                else {
                    _gameButtons[i].gameObject.SetActive(false);
                }
            }
        }

        private void Start() {
            SetStateWaitingInfo(false);
            SubscribeButtons();
            UpdateButtonState(GameButtonType.Bid);
        }

        private void SubscribeButtons() {
            _gameButtons = FindObjectsOfType<GameButton>(true);
            for (int i = 0; i < _gameButtons.Length; i++) {
                var eve = SubscribeGameButton(_gameButtons[i]);
                _gameButtons[i].Init(eve);
            }
        }

        private Action SubscribeGameButton(GameButton gameButton) {
            Action currentEvent = null;
            switch (gameButton.ButtonType) {
                case GameButtonType.Bid: _startGameClick += StartGameClick; currentEvent = _startGameClick; break;
                case GameButtonType.CashOut: _cashOutClick += CashOutClick; currentEvent = _cashOutClick; break;
                case GameButtonType.Cancel: _cancelClick += CancelClick; currentEvent = _cancelClick; break;
                default: Debug.LogError("No button type"); break;
            }
            return currentEvent;
        }

        private async void StartGameClick() {
            SetStateWaitingInfo(true);
            UpdateButtonState(GameButtonType.Cancel);
            float awaitTime = 2;
            _startAwait = true;
            _waitingImageFill.fillAmount = 1;
            var fillTween = _waitingImageFill.DOFillAmount(0, awaitTime).SetEase(Ease.Linear);

            while (awaitTime > 0) {
                if (!_startAwait) {
                    fillTween.Kill();
                    return;
                }
                awaitTime -= Time.deltaTime;
                await UniTask.Delay(TimeSpan.FromTicks(1), cancellationToken: destroyCancellationToken);
            }
            SetStateWaitingInfo(false);
            UpdateButtonState(GameButtonType.CashOut);
            _signalBus.Fire(new SignalStartGame());
        }

        private void CashOutClick() {
            UpdateButtonState(GameButtonType.Bid);
            _signalBus.Fire(new SignalStopGame(true));
        }

        private void CancelClick() {
            _startAwait = false;
            SetStateWaitingInfo(false);
            UpdateButtonState(GameButtonType.Bid);
        }

        private void LooseGame() {
            UpdateButtonState(GameButtonType.Bid);
        }

        private void SetStateWaitingInfo(bool state) {
            _barRoot.gameObject.SetActive(state);
            _waitingTextUI.gameObject.SetActive(state);
        }
    }
}
