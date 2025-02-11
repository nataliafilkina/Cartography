using DG.Tweening;
using UnityEngine;

namespace AnimationsDoTween
{
    public class PulseAnimation : MonoBehaviour
    {
        [SerializeField]
        private float _scaleFactor = 1.2f;
        [SerializeField]
        private float _duration = 0.2f;

        private Vector3 _originalScale;
        private Tween _tween;

        private void Start()
        {
            _originalScale = transform.localScale;
            StartPulse();
        }

        public void StartPulse()
        {
            if (_tween != null && _tween.IsActive())
                return;

            _tween = transform.DOScaleX(_originalScale.x * _scaleFactor, _duration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        private void OnDestroy()
        {
            _tween?.Kill();
        }
    }
}
