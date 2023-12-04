﻿using Game.Scripts.UI.Panels.Record;
using Game.Scripts.Signal;
using System.Linq;
using Zenject;
using System;

namespace Game.Scripts.Game {
    public class RecordController {
        private CurrenciesService _currenciesService;
        private GameData _gameData;

        public RecordController(
        CurrenciesService currenciesService,
        SignalBus signalBus,
        GameData gameData
        ) {
            _gameData = gameData;
            _currenciesService = currenciesService;
            signalBus.Subscribe<SignalSaveRecord>(SaveRecord);
        }

        public void SaveRecord(SignalSaveRecord signalSaveRecord) {
            var allRecordElement = new AllGameRecordInfo(DateTime.Now, signalSaveRecord.GameTime, _currenciesService.CurrentMoney, signalSaveRecord.Reward);
            var bestRecordElement = new BestRecordInfo(signalSaveRecord.GameTime, signalSaveRecord.Reward);
            SaveAllRecordElement(allRecordElement);
            SaveBestRecordElement(bestRecordElement);
            _gameData.Save();
        }

        private void SaveBestRecordElement(BestRecordInfo bestRecordElement) {
            int bestRecordNullElementIdex = -1;
            for (int i = 0; i < _gameData.BestGameRecords.Length; i++) {
                if (_gameData.BestGameRecords[i] == null) {
                    bestRecordNullElementIdex = i;
                    break;
                }
            }

            if (bestRecordNullElementIdex == -1) {
                if (bestRecordElement.Reward < _gameData.BestGameRecords[_gameData.BestGameRecords.Length - 1].Reward) {
                    return;
                }
                var list = _gameData.BestGameRecords.ToList();
                list.Sort((x, y) => x.Reward.CompareTo(y.Reward));
                list.Reverse();
                _gameData.BestGameRecords = list.ToArray();
                var currentElement = bestRecordElement;
                for (int i = 0; i < _gameData.BestGameRecords.Length; i++) {
                    if (currentElement.Reward > _gameData.BestGameRecords[i].Reward) {
                        if (i + 1 >= _gameData.BestGameRecords.Length) {
                            _gameData.BestGameRecords[i] = bestRecordElement;
                        }
                        else {
                            currentElement = _gameData.BestGameRecords[i];
                            _gameData.BestGameRecords[i] = bestRecordElement;
                            bestRecordElement = currentElement;
                        }
                    }
                }
            }
            else {
                _gameData.BestGameRecords[bestRecordNullElementIdex] = bestRecordElement;
            }
        }

        private void SaveAllRecordElement(AllGameRecordInfo allRecordElement) {
            int nullElementIndex = -1;
            for (int i = 0; i < _gameData.AllGameRecords.Length; i++) {
                if (_gameData.AllGameRecords[i] == null) {
                    nullElementIndex = i;
                    break;
                }
            }

            if (nullElementIndex == -1) {
                for (int i = 0; i < _gameData.AllGameRecords.Length; i++) {
                    if (i + 1 >= _gameData.AllGameRecords.Length) {
                        _gameData.AllGameRecords[i] = allRecordElement;
                    }
                    else {
                        _gameData.AllGameRecords[i] = _gameData.AllGameRecords[i + 1];
                    }
                }
            }
            else {
                _gameData.AllGameRecords[nullElementIndex] = allRecordElement;
            }
        }
    }
}
