using DG.Tweening;
using UnityEngine;
using Zenject;

public class UIPanel : MonoBehaviour {
    [field: SerializeField] protected CanvasGroup CanvasGroup { get; private set; }
    public RectTransform CachedTransform { get; private set; }
    private WindowShowHideAnimation _windowShowHideAnimation;
    [SerializeField] private StartAnimType _startAnimType;
    [SerializeField] private EndAnimType _endAnimType;

    protected UIService UIService { get; private set; }

    [Inject]
    private void Construct(
    WindowShowHideAnimation windowShowHideAnimation,
    UIService uiService
    ) {
        _windowShowHideAnimation = windowShowHideAnimation;
        UIService = uiService;
    }

    public virtual void Show() {
        if (CachedTransform == null) {
            CachedTransform = transform as RectTransform;
        }
        _windowShowHideAnimation.GetShowSequence(_startAnimType, CanvasGroup, CachedTransform);
        gameObject.SetActive(true);
        UIService.HideUIElements();
    }

    public virtual void Hide() {
        if (_endAnimType == EndAnimType.NoAnim) {
            gameObject.SetActive(false);
        }
        else {
            _windowShowHideAnimation.GetHideSequence(_endAnimType, CanvasGroup, CachedTransform).OnComplete(() => gameObject.SetActive(false));
        }
        UIService.ShowUIElements();
    }
}
