using AnimationsDoTween;
using Logic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class UIReadPanel : MonoBehaviour
    {
        [SerializeField]
        private bool _isAnimated = false;
        [SerializeField]
        private AudioSource _audioSource;

        [Header("TextFields")]
        [SerializeField]
        protected TextMeshProUGUI _nameText;
        [SerializeField]
        protected TextMeshProUGUI _descriptionText;
        [SerializeField]
        protected Image _image;

        [Header("Buttons")]
        [SerializeField]
        protected Button _closeButton;
        [SerializeField]
        protected Button _audioButton;

        protected PinData _pinData;
        protected PinsEditor _pinsEditor;
        protected bool _isOpen;
        private FadingAnimation _fadeAnimation;

        [Inject]
        public void Construct(PinsEditor pinsEditor)
        {
            _pinsEditor = pinsEditor;
        }

        protected virtual void OnEnable()
        {
            if (_isAnimated && _fadeAnimation == null)
                transform.TryGetComponent(out _fadeAnimation);

            _audioButton?.onClick.AddListener(OnAudioClick);
            _closeButton.onClick.AddListener(Hide);
        }

        protected virtual void OnDisable()
        {
            _audioButton?.onClick.RemoveListener(OnAudioClick);
            _closeButton.onClick.RemoveListener(Hide);
        }

        public void Show(PinData pinData)
        {
            _pinData = pinData;
            _isOpen = true;

            SetSprite();
            SetAudio();

            _nameText.text = _pinData.Name;
            _descriptionText.text = _pinData.Description;

            gameObject.SetActive(true);
            if (_fadeAnimation)
                _fadeAnimation.FadeIn();
        }

        public void Hide()
        {
            void Hidden()
            {
                _image.gameObject.SetActive(false);
                _audioButton?.gameObject.SetActive(false);
                gameObject.SetActive(false);
                Clear();
            }

            if (_fadeAnimation)
                _fadeAnimation.FadeOut(Hidden);
            else
                Hidden();
        }

        protected virtual void OnAudioClick()
        {
            if (_audioSource.isPlaying) 
                _audioSource.Pause();
            else 
                _audioSource.Play();
        }

        protected virtual void Clear()
        {
            _pinData = null;
            _image.sprite = null;
            _isOpen = false;

            if(_audioSource)
                _audioSource.clip = null;
        }

        private async Task SetSprite()
        {
            var sprite = await _pinsEditor.GetSprite(_pinData);
            if (_isOpen && sprite != null)
            {
                _image.sprite = sprite;
                _image.gameObject.SetActive(true);
            }
        }

            private async Task SetAudio()
        {
            if (_audioButton != null)
            {
                var clip = await _pinsEditor.GetClip(_pinData);
                if (_isOpen)
                {
                    _audioSource.clip = clip;
                    _audioButton.gameObject.SetActive(true);
                }
            }
        }
    }
}
