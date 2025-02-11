using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class UIMapClickHandler : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private UIShowHideHandler _panelsHandler;

        private UIGraphicRaycaster _raycaster;

        private void Start()
        {
            _raycaster = GetComponentInParent<UIGraphicRaycaster>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_raycaster.IsPixelOpaque(gameObject, eventData.position))
            {
                _panelsHandler.ShowAddPin(eventData.position);
            }
        }
    }
}