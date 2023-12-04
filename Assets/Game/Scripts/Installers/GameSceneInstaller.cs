using Game.Scripts.UI.Panels;
using Game.Scripts.Game;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(UIService))]
public class GameSceneInstaller : MonoInstaller {

    public override void InstallBindings() {
        Container.BindInterfacesAndSelfTo<UIService>().FromComponentInHierarchy().AsCached().NonLazy();//В себе ищет панели на сцене, поэтому нужно заново инстансить в каждой сцене, либо вынести логику поиска в метод инита и реинитить в стартере.
        Container.Bind<CheatManager>().FromComponentInHierarchy().AsCached().NonLazy();
        Container.BindInterfacesAndSelfTo<CurrenciesService>().AsSingle().NonLazy();
        Container.Bind<ILoadableElement>().FromComponentsInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelService>().AsSingle().NonLazy();
        Container.Bind<TwoPersonModeController>().AsCached().NonLazy();
        Container.Bind<RecordController>().AsCached().NonLazy();
        Container.Bind<RouteController>().AsCached().NonLazy();
        Container.Bind<EndGameService>().AsCached().NonLazy();
        Container.Bind<GameService>().AsCached().NonLazy();
    }
}
