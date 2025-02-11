using Logic;
using UnityEngine;

namespace UI
{
    public class UIShowHideHandler : MonoBehaviour
    {
        [SerializeField]
        private UIPreviwPinPanel _previwPinPanel;

        [SerializeField]
        private UIDataEntryPopup _dataEntryPanel;

        [SerializeField]
        private UIReadPanel _readPanel;

        private void Start()
        {
            _previwPinPanel.gameObject.SetActive(false);
            _dataEntryPanel.gameObject.SetActive(false);
            _readPanel.gameObject.SetActive(false);
        }

        public void ShowPreviw(PinData pin)
        {
            _dataEntryPanel.Hide();
            _previwPinPanel.Hide();
            _previwPinPanel.Show(pin);
        }

        public void ShowAddPin(Vector3 clickPosition)
        {
            _previwPinPanel.Hide();
            _dataEntryPanel.ShowAddMode(clickPosition);
        }

        public void ShowRead(PinData pin)
        {
            _readPanel.Show(pin);
        }

        public void ShowEditPin(PinData pinData)
        {
            _previwPinPanel.Hide();
            _dataEntryPanel.ShowEditMode(pinData);
        }

        public void OnCloseEdit(PinData pinData)
        {
            _previwPinPanel.Show(pinData);
        }
    }
}
