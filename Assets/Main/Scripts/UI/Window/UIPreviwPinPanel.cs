using Logic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class UIPreviwPinPanel : UIReadPanel
    {
        [Header("Buttons")]
        [SerializeField]
        private Button _readMoreButton;
        [SerializeField]
        private Button _editButton;

        private UIShowHideHandler _showHideHandler;

        [Inject]
        public void Construct(PinsEditor pinsEditor, UIShowHideHandler showHideHandler)
        {
            _pinsEditor = pinsEditor;
            _showHideHandler = showHideHandler;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _readMoreButton.onClick.AddListener(OnReadMoreClick);
            _editButton.onClick.AddListener(OnEditClick);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _readMoreButton.onClick.RemoveListener(OnReadMoreClick);
            _editButton.onClick.RemoveListener(OnEditClick);
        }

        public void OnReadMoreClick()
        {
            _showHideHandler.ShowRead(_pinData);
            Hide();
        }

        public void OnEditClick()
        {
            _showHideHandler.ShowEditPin(_pinData);
        }
    }
}
