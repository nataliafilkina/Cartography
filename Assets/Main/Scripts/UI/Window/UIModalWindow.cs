using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIModalWindow : MonoBehaviour
    {
        [Header("Header")]
        [SerializeField]
        protected TextMeshProUGUI _headerText;

        [Header("Content")]
        [SerializeField]
        protected TextMeshProUGUI _contentText;

        [Header("Buttons")]
        [SerializeField]
        protected Button _confirmButton;
        [SerializeField]
        protected Button _declineButton;

        protected Action _onConfirmAction;
        protected Action _onDeclineAction;

        protected virtual void Update()
        {
            if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter))
                Confirm();
            if (Input.GetKeyUp(KeyCode.Escape))
                Decline();
        }

        private void OnEnable()
        {
            _confirmButton.onClick.AddListener(Confirm);
            _declineButton.onClick.AddListener(Decline);
        }

        private void OnDisable()
        {
            _confirmButton.onClick.RemoveListener(Confirm);
            _declineButton.onClick.RemoveListener(Decline);
        }

        public void ShowConfirmMessage(string header, string content,
                        Action confirmAction, Action declineAction = null)
        {
            SetData(header, content);
            _confirmButton.gameObject.SetActive(true);
            _declineButton.gameObject.SetActive(true);

            _onConfirmAction = confirmAction;
            _onDeclineAction = declineAction;
            gameObject.SetActive(true);
        }

        public void ShowMessage(string header, string content, Action confirmAction = null)
        {
            SetData(header, content);

            _confirmButton.gameObject.SetActive(true);
            _declineButton.gameObject.SetActive(false);
            _onConfirmAction = confirmAction;

            gameObject.SetActive(true);
        }

        private void SetData(string header, string content)
        {
            _headerText.text = header;
            _contentText.text = content;
        }

        public virtual void Close()
        {
            _headerText.text = "";
            _contentText.text = "";

            gameObject.SetActive(false);
        }

        public virtual void Confirm()
        {
            _onConfirmAction?.Invoke();
            Close();
        }

        public virtual void Decline()
        {
            _onDeclineAction?.Invoke();
            Close();
        }
    }
}
