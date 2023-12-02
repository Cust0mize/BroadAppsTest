using UnityEngine.UI;
using UnityEngine;

namespace Game.Scripts.Game {
    [ExecuteAlways]
    public class Player : MonoBehaviour {
        [SerializeField] private Vector2 _startPosition;
        [SerializeField] private Vector2 _endPosition;
        [SerializeField] private AnimationCurve _animationCurve;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _graphImage;

        public void UpdatePosition(float timeValue) {
            Vector2 resultEndPosition = new Vector3(_endPosition.x, _animationCurve.Evaluate(timeValue) * _endPosition.y);
            _rectTransform.anchoredPosition = Vector3.Lerp(_startPosition, resultEndPosition, timeValue);
            _graphImage.fillAmount = timeValue;
        }
    }
}
