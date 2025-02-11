using DG.Tweening;
using System;
using UnityEngine;

namespace AnimationsDoTween
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadingAnimation : MonoBehaviour
    {
        [SerializeField]
        private float _durationFade;
        private CanvasGroup _canvasGroup;
        private Tween _tween;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeIn(Action onComplete = null)
        {
            Fade(1f, _durationFade, () =>
            {
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;

                onComplete?.Invoke();
            });
        }

        public void FadeOut(Action onComplete = null)
        {
            Fade(0f, _durationFade, () =>
            {
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;

                onComplete?.Invoke();
            });
        }

        private void Fade(float end, float duration, TweenCallback onEnd)
        {
            _tween?.Kill(false);

            _canvasGroup.alpha = end == 1f ? 0f : 1f;
            _tween = _canvasGroup.DOFade(end, duration);
            _tween.onComplete += onEnd;
        }
    }
}
