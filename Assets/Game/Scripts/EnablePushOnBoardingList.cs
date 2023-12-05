using Game.Scripts.Signal;

public class EnablePushOnBoardingList : BaseOnBoardingList {
    public override void ClickButton() {
        //Button.RemoveAllAndSubscribeButton();
        SignalBus.Fire(new SignalNextOnBoardingClick());
    }
}
