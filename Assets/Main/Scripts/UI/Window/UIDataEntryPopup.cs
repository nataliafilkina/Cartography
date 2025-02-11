using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Logic;
using Zenject;

namespace UI
{
    public class UIDataEntryPopup : MonoBehaviour
    {
        #region SerializeFields
        [Header("Label")]
        [SerializeField]
        private GameObject _labelCreatePrefab;
        [Header("InputFields")]
        [SerializeField]
        private TMP_InputField _nameInput;
        [SerializeField]
        private TMP_InputField _descriptionInput;
        [SerializeField]
        private Image _image;
        [Header("Buttons")]
        [SerializeField]
        private Button _addImage;
        [SerializeField]
        private Button _removeImage;
        [SerializeField]
        private Button _addAudio;
        [SerializeField]
        private Button _saveButton;
        [SerializeField]
        private Button _deleteButton;
        [SerializeField]
        private Button _cancelButton;
        #endregion

        #region Fields
        private OpenFileDialog _openDialog = new();
        private PinsEditor _pinsEditor;
        private GameObject _uiLabel;
        private UIShowHideHandler _showHideHandler;
        private UIModalWindow _modalWindow;
        private CanvasGroup _canvasGroup;

        private PinData _pinData;
        private Vector3 _clickPosition;
        private AudioClip _pinAudioClip;

        private bool _isEditMode;
        private bool _isImageEdit = false;
        private bool _isClipEdit = false;
        #endregion

        [Inject]
        public void Construct(PinsEditor pinsEditor, UIShowHideHandler showHideHandler, UIModalWindow modalWindow)
        {
            _pinsEditor = pinsEditor;
            _showHideHandler = showHideHandler;
            _modalWindow = modalWindow;
        }

        private void Start()
        {
            _image.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _addImage.onClick.AddListener(OnAddImage);
            _removeImage.onClick.AddListener(OnRemoveImage);
            _addAudio.onClick.AddListener(OnAddAudio);
            _deleteButton.onClick.AddListener(OnDeleteClick);
            _saveButton.onClick.AddListener(OnSaveClick);
            _cancelButton.onClick.AddListener(OnCancleClick);

            if(_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnDisable()
        {
            _addImage.onClick.RemoveListener(OnAddImage);
            _removeImage.onClick.RemoveListener(OnRemoveImage);
            _addAudio.onClick.RemoveListener(OnAddAudio);
            _deleteButton.onClick.RemoveListener(OnDeleteClick);
            _saveButton.onClick.RemoveListener(OnSaveClick);
            _cancelButton.onClick.RemoveListener(OnCancleClick);
        }

        public void ShowAddMode(Vector3 clickPosition)
        {
            _isEditMode = false;
            _clickPosition = clickPosition;
            _uiLabel = Instantiate(_labelCreatePrefab, clickPosition, Quaternion.identity, transform);
            _deleteButton.gameObject.SetActive(false);
            gameObject.SetActive(true);
        }

        public void ShowEditMode(PinData pinData)
        {
            _isEditMode = true;
            _pinData = pinData;

            _nameInput.text = _pinData.Name;
            _descriptionInput.text = _pinData.Description;

            if (pinData.Sprite != null)
            {
                _image.sprite = pinData.Sprite;
                _image.gameObject.SetActive(true);
                _removeImage.gameObject.SetActive(true);
            }

            _deleteButton.gameObject.SetActive(true);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            if (_isEditMode)
                _showHideHandler.OnCloseEdit(_pinData);
            Clear();
            _image.gameObject.SetActive(false);
            _removeImage.gameObject.SetActive(false);
        }

        private void OnSaveClick()
        {
            if (string.IsNullOrEmpty(_nameInput.text))
            {
                _canvasGroup.blocksRaycasts = false;
                _modalWindow.ShowMessage("Невозможно создать пин",
                    "Необходимо заполнить поле \"Наименование\"", 
                    () => _canvasGroup.blocksRaycasts = true); ;
                return;
            }

            if (_isEditMode)
                EditPin();
            else
                AddPin();

            Hide();
        }

        private async void OnAddImage()
        {
            Texture2D texture = await _openDialog.OpenImage();
            if (texture != null)
            {
                _image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                _isImageEdit = true;
                _image.gameObject.SetActive(true);
            }
        }

        private void OnRemoveImage()
        {
            _image.sprite = null;
            _isImageEdit = true;
            _image.gameObject.SetActive(false);
        }

        private async void OnAddAudio()
        {
            AudioClip clip = await _openDialog.OpenAudio();
            if (clip != null)
            {
                _pinAudioClip = clip;
                _isClipEdit = true;
            }
        }

        private void OnCancleClick()
        {
            Hide();
        }

        private void OnDeleteClick()
        {
            _canvasGroup.blocksRaycasts = false;
            _modalWindow.ShowConfirmMessage("Удаление", "Удалить пин?",
                () =>
                {
                    _pinsEditor.Remove(_pinData);
                    _isEditMode = false;
                    _canvasGroup.blocksRaycasts = true;
                    Hide();
                },
                () => { _canvasGroup.blocksRaycasts = true; });
        }

        private void Clear()
        {
            if (_uiLabel != null)
                Destroy(_uiLabel);

            _image.sprite = null;
            _nameInput.text = "";
            _descriptionInput.text = "";
            _isImageEdit = false;
            _isImageEdit = false;
            _pinAudioClip = null;
        }

        private void EditPin()
        {
            PinData newPinData = new(_pinData)
            {
                Name = _nameInput.text,
                Description = _descriptionInput.text
            };

            _pinsEditor.Edit(
                _pinData,
                newPinData,
                texture: _isImageEdit ? _image.sprite?.texture : null,
                editTexture: _isImageEdit,
                clip: _isClipEdit ? _pinAudioClip : null,
                editAudio: _isClipEdit
            );
        }

        private void AddPin()
        {
            var texture = _image.sprite ? _image.sprite.texture : null;
            _pinsEditor.Create(_nameInput.text, _descriptionInput.text, _clickPosition, texture, _pinAudioClip);
        }
    }
}
