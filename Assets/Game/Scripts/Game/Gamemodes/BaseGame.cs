﻿using Game.Scripts.UI.Panels;
using Game.Scripts.Signal;
using UnityEngine;
using Zenject;
using Enums;

namespace Game.Scripts.Game.Gamemodes {
    public abstract class BaseGame {
        public abstract Gamemode GameMode { get; }

        private GameSettingsController _gameSettingsController;
        private GameService _gameService;

        protected CurrenciesService CurrenciesService { get; private set; }
        protected UIService UIService { get; private set; }
        protected SignalBus SignalBus { get; }
        public float LooseValue => 0.95f;
        protected float EndTime => StartTime - Time.time;

        public AnimationCurve MultiplyAnimationCurve { get; private set; }
        public float CurrentMultiply { get; private set; }
        public float StartTimeGame { get; private set; }
        public bool IsStartGame { get; private set; }
        public float StartTime { get; private set; }
        public float Multiply { get; private set; }
        public float Amount { get; private set; }

        public BaseGame(
            GameSettingsController gameSettingsController,
            CurrenciesService currenciesService,
            GameService gameService,
            UIService uIService,
            SignalBus signalBus
        ) {
            _gameSettingsController = gameSettingsController;
            CurrenciesService = currenciesService;
            _gameService = gameService;
            UIService = uIService;
            SignalBus = signalBus;
        }

        public abstract void SetStartValue();

        public virtual void GameUpdate() {
            if (IsStartGame) {
                StartTime += Time.deltaTime;
                CurrentMultiply += Time.deltaTime / 2;
            }
        }

        public abstract void SetResult();

        public void StartGame() {
            CurrentMultiply = 0;
            StartTime = 0;
            IsStartGame = true;
            _gameSettingsController.ParceValue(out float multiply, out float amount);
            Multiply = multiply;
            Amount = amount;
            StartTimeGame = Time.time;
            MultiplyAnimationCurve = _gameService.GetNewCurve(0.1f, 0.95f);//???
        }

        public virtual void StopGame(SignalStopGame signalStopGame) {
            IsStartGame = false;
            UIService.GetPanel<GamePanel>().UpdateButtonState(GameButtonType.Bid);
        }
    }
}