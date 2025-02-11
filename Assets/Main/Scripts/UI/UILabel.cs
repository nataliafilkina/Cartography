using Logic;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class UILabel : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _nameText;

        private PinsEditor _pinsEditor;

        public PinData PinData { get; private set; }

        [Inject]
        public void Construct(PinData pinData, Transform parent, PinsEditor pinsEditor)
        {
            _pinsEditor = pinsEditor;
            PinData = pinData;
            transform.SetParent(parent, false);
            transform.position = PinData.Position;
            _nameText.text = PinData.Name;
            PinData.OnEdited += Refresh;
        }

        public class Factory : PlaceholderFactory<PinData, Transform, UILabel> { }

        public void SetPosition()
        {
            PinData newData = new(PinData);
            newData.Position = transform.position;
            _pinsEditor.Edit(PinData, newData);
        }

        private void Refresh()
        {
            _nameText.text = PinData.Name;
        }

        private void OnDestroy()
        {
            if (PinData != null)
            {
                PinData.OnEdited -= Refresh;
            }
        }
    }
}
