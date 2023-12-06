using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Firebase.Installations;
using Firebase.RemoteConfig;
using Firebase.Extensions;
using UnityEngine;
using Firebase;
using System;

namespace Game.Scripts.Services {
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
}