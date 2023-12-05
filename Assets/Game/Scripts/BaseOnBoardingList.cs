using UnityEngine.UI;
using UnityEngine;
using Zenject;

public abstract class BaseOnBoardingList : MonoBehaviour {
    protected Button Button { get; private set; }
    protected SignalBus SignalBus;

    [Inject]
    private void Construct(SignalBus signalBus) {
        SignalBus = signalBus;
    }

    public void Init() {
        Button = transform.GetComponentInChildren<Button>(true);
        Button.RemoveAllAndSubscribeButton(ClickButton);
    }

    public abstract void ClickButton();

    public void SetActive(bool state) {
        gameObject.SetActive(state);
    }
}
