using Game.Scripts.Signal;
using Zenject;

public class NextOnBoardingList : BaseOnBoardingList {
    public override void ClickButton() {
        SignalBus.Fire(new SignalNextOnBoardingClick());
    }
}
