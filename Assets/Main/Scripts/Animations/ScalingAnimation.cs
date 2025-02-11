using UnityEngine;
using DG.Tweening;

namespace AnimationsDoTween
{
    public class ScalingAnimation
    {
        private Transform _transform;
        private float _scaleFactor;
        private float _duration = 0.2f;
        private Vector3 _originalScale;

        public ScalingAnimation(Transform transform, float scaleOffset)
        {
            _transform = transform;
            _scaleFactor = scaleOffset;
            _originalScale = transform.localScale;
        }

        public void DoScale()
        {
            if (_originalScale == _transform.localScale)
                _transform.DOScale(_transform.localScale * _scaleFactor, _duration).SetEase(Ease.OutBack);
        }

        public void ReturnScale()
        {
            if (_originalScale != _transform.localScale)
                _transform.DOScale(_originalScale, _duration).SetEase(Ease.InOutBack);
        }
    }
}
