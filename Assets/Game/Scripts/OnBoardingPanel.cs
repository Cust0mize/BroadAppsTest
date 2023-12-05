using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Firebase.Installations;
using Firebase.RemoteConfig;
using Firebase.Extensions;
using Game.Scripts.Signal;
using UnityEngine;
using Firebase;
using Zenject;
using System;

public class OnBoardingPanel : UIPanel {
    private OnBoardingScreen[] _onBoardingScreen;
    private int _currentScreenIndex;
    private long _onboardingIndex = 1;

    private GameData _gameData;

    [Inject]
    private void Construct(
        SignalBus signalBus,
        GameData gameData
    ) {
        _gameData = gameData;
        signalBus.Subscribe<SignalNextOnBoardingClick>(OnBoardingScreenUpdate);
    }

    private void OnBoardingScreenUpdate(SignalNextOnBoardingClick signalNextOnBoardingClick) {
        _onBoardingScreen[_onboardingIndex].SetActive(_currentScreenIndex, false);
        _currentScreenIndex++;
        if (_currentScreenIndex < _onBoardingScreen[_onboardingIndex].ListLength) {
            _onBoardingScreen[_onboardingIndex].SetActive(_currentScreenIndex, true);
        }
        else {
            if (_onboardingIndex == 0) {
                _gameData.IsShowOnboarding = false;
                print("end");
            }
        }
    }

    private void Start() {
        if (_gameData.IsShowOnboarding) {
            _onboardingIndex = FirebaseRemoteConfig.DefaultInstance.GetValue("_isReview").LongValue;
            _onBoardingScreen = transform.GetComponentsInChildren<OnBoardingScreen>(true);

            for (int i = 0; i < _onBoardingScreen.Length; i++) {
                _onBoardingScreen[i].Init();
                _onBoardingScreen[i].gameObject.SetActive(false);
            }
            _onBoardingScreen[_onboardingIndex].gameObject.SetActive(true);
            _onBoardingScreen[_onboardingIndex].SetActive(_currentScreenIndex, true);
        }
    }
}

public static class RemoteConfig {
    static DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    public static bool IsInited { get; private set; } = false;

    public static async UniTask Init() {
        await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(async task =>
        {
            dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available) {
                await InitializeFirebase();

                await FirebaseInstallations.DefaultInstance.GetTokenAsync(forceRefresh: true).ContinueWith(task =>
                {
                    if (!(task.IsCanceled || task.IsFaulted) && task.IsCompleted) {
                        Debug.Log(string.Format("THIS Installations token {0}", task.Result));
                    }
                });
            }
            else {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private async static UniTask InitializeFirebase() {
        if (IsInited) return;
        Dictionary<string, object> defaults =
              new Dictionary<string, object>();

        await FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults);
        await FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        await FirebaseRemoteConfig.DefaultInstance.ActivateAsync();
        IsInited = true;
    }
}
