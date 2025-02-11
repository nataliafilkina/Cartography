using Logic;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace UI
{
    public class UIPinSpawner : MonoBehaviour
    {
        [SerializeField]
        private Transform _labelContainer;

        private PinsEditor _pinsEditor;
        private UILabel.Factory _labelFactory;

        private Dictionary<string, UILabel> _labelById = new();

        [Inject]
        public void Construct(LazyInject<UILabel.Factory> labelFactory)
        {
            _labelFactory = labelFactory.Value;
        }

        public void Begin(PinsEditor pinsEditor)
        {
            _pinsEditor = pinsEditor;
            InstantiateLabels(_pinsEditor.Pins);
        }

        private void InstantiateLabels(IReadOnlyList<PinData> pins)
        {
            foreach (var pin in pins)
            {
                InstantiateLabel(pin);
            }
        }

        private void InstantiateLabel(PinData pin)
        {
            var labelObject = _labelFactory.Create(pin, _labelContainer);
            var label = labelObject.GetComponent<UILabel>();
            _labelById.Add(pin.Id, label);
        }

        public void AddLabel(PinData pin)
        {
            InstantiateLabel(pin);
        }

        public void RemoveLabel(string id)
        {
            var label = _labelById[id];
            _labelById.Remove(id);
            Destroy(label.gameObject);
        }
    }
}
