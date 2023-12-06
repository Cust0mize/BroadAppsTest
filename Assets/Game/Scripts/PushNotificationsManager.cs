using Firebase.Messaging;
using UnityEngine;

public class PushNotificationsManager : MonoBehaviour {
    private void Start() {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseMessaging.TokenReceived += TokenRecevied;
            FirebaseMessaging.MessageReceived += MessageReceiver;
        });
    }

    private void MessageReceiver(object sender, MessageReceivedEventArgs e) {
        Debug.Log(e.Message);
    }

    private void TokenRecevied(object sender, TokenReceivedEventArgs e) {
        Debug.Log(e.Token);
    }
}