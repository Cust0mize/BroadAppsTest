using UnityEngine;
using Zenject;

[RequireComponent(typeof(UIService))]
public class GameSceneInstaller : MonoInstaller {

    public override void InstallBindings() {
        Container.BindInterfacesAndSelfTo<UIService>().FromComponentInHierarchy().AsCached().NonLazy();     //� ���� ���� ������ �� �����, ������� ����� ������ ���������� � ������ �����, ���� ������� ������ ������ � ����� ����� � ��������� � ��������.
        Container.Bind<CheatManager>().FromComponentInHierarchy().AsCached().NonLazy();
        Container.Bind<ILoadableElement>().FromComponentsInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelService>().AsSingle().NonLazy();
    }
}
