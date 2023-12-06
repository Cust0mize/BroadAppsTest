using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Button))]
public class CloseButton : MonoBehaviour, IBeginDragHandler, IDragHandler {
    private RectTransform _rectTransform;
    private Vector2 _startPosition;
    private bool _isSavePosition;
    private UIService _uIService;
    private bool _isDrag;

    [Inject]
    private void Construct(UIService uIService) {
        _uIService = uIService;
    }

    private void OnEnable() {
        if (_rectTransform != null) {
            _rectTransform.anchoredPosition = Vector2.zero;
        }
    }

    private void Start() {
        _rectTransform = transform.parent as RectTransform;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (!_isSavePosition) {
            _isSavePosition = true;
            _startPosition = _rectTransform.position;
        }
        _isDrag = true;
    }

    public void OnDrag(PointerEventData eventData) {
        if (!_isDrag) {
            return;
        }

        float ypos;
        if (_rectTransform.anchoredPosition.y + eventData.delta.y <= _startPosition.y) {
            ypos = _rectTransform.anchoredPosition.y + eventData.delta.y;
        }
        else {
            ypos = _startPosition.y;
        }

        _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, ypos);

        if (_rectTransform.anchoredPosition.y < _startPosition.y - 100) {
            _isDrag = false;
            _uIService.HidePanelBypassStack(_uIService.GetOpenPanel());
        }
    }
}