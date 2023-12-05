using Game.Scripts.Signal;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(AudioMixerManager))]
public class ProjectInstaller : MonoInstaller {
    public override void InstallBindings() {
        Container.Bind<AudioMixerManager>().FromComponentInHierarchy().AsTransient().NonLazy();
        Container.Bind<ParticleAnimationService>().AsSingle().NonLazy();
        Container.Bind<WindowShowHideAnimation>().AsSingle().NonLazy();
        Container.Bind<ResourceLoaderService>().AsSingle().NonLazy();
        Container.Bind<SceneLoaderService>().AsSingle().NonLazy();
        Container.Bind<ConfigService>().AsSingle().NonLazy();
        Container.Bind<SaveSystem>().AsSingle().NonLazy();
        Container.Bind<GameData>().AsSingle().NonLazy();

        SignalRegistry();
    }

    public void SignalRegistry() {
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<SignalSelectAvailableElement>().OptionalSubscriber();
        Container.DeclareSignal<SignalStartTwoPersonModeGame>().OptionalSubscriber();
        Container.DeclareSignal<SignalUpdateResultTwoGame>().OptionalSubscriber();
        Container.DeclareSignal<SignalStartTaskModeGame>().OptionalSubscriber();
        Container.DeclareSignal<SignalStartTaskModeGame>().OptionalSubscriber();
        Container.DeclareSignal<SignalUpdateAchivment>().OptionalSubscriber();
        Container.DeclareSignal<SignalUpdateAirplane>().OptionalSubscriber();
        Container.DeclareSignal<SignalBuyNewElemetn>().OptionalSubscriber();
        Container.DeclareSignal<SignalUpdateLevel>().OptionalSubscriber();
        Container.DeclareSignal<SignalMoneyUpdate>().OptionalSubscriber();
        Container.DeclareSignal<SignalSelectRoute>().OptionalSubscriber();
        Container.DeclareSignal<SignalSaveRecord>().OptionalSubscriber();
        Container.DeclareSignal<SignalResetGame>().OptionalSubscriber();
        Container.DeclareSignal<SignalStartGame>().OptionalSubscriber();
        Container.DeclareSignal<SignalLooseGame>().OptionalSubscriber();
        Container.DeclareSignal<SignalStopGame>().OptionalSubscriber();
        Container.DeclareSignal<SignalBuyRoute>().OptionalSubscriber();
        Container.DeclareSignal<SignalNoMoney>().OptionalSubscriber();
        Container.DeclareSignal<OpenGamePanel>().OptionalSubscriber();
        Container.DeclareSignal<SignalWinGame>().OptionalSubscriber();
    }
}
