using Logic;
using UI;
using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [Header("UI")]
    [SerializeField]
    private UIShowHideHandler _showHidePanel;
    [SerializeField]
    private UIGraphicRaycaster _mapGraphicRaycaster;
    [SerializeField]
    private UIPinSpawner _spawner;
    [SerializeField]
    private UIModalWindow _modalWindow;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject _labelPrefab;

    public override void InstallBindings()
    {
        Container.BindFactory<PinData, Transform, UILabel, UILabel.Factory>().FromComponentInNewPrefab(_labelPrefab);

        Container.Bind<UIModalWindow>().FromInstance(_modalWindow).AsSingle();
        Container.Bind<UIShowHideHandler>().FromInstance(_showHidePanel).AsSingle();
        Container.Bind<UIGraphicRaycaster>().FromInstance(_mapGraphicRaycaster).AsSingle();
        Container.Bind<UIPinSpawner>().FromInstance(_spawner).AsSingle();

        Container.Bind<PinsEditor>().FromNew().AsSingle().NonLazy();
        Container.Resolve<PinsEditor>().Begin();
    }
}
