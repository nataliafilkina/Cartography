using AnimationsDoTween;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI
{
    public class UILabelHandler : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private UIShowHideHandler _showHighPanel;
        private UIGraphicRaycaster _raycasterMap;
        private UILabel _label;
        private ScalingAnimation _animation;

        [SerializeField]
        private float _timePressing = 0.5f;
        [SerializeField]
        private float _scaleAnimation = 1.2f;

        private bool _isPressed = false;
        private bool _isDragged = false;

        private Vector3 _oldPosition;

        [Inject]
        public void Construct(UIShowHideHandler showHighPanel, UIGraphicRaycaster raycasterMap)
        {
            _showHighPanel = showHighPanel;
            _raycasterMap = raycasterMap;
        }

        private void Start()
        {
            _label = GetComponent<UILabel>();
            _animation = new ScalingAnimation(transform, _scaleAnimation);
        }

        public void Initialized(UIShowHideHandler showHighPanel, UIGraphicRaycaster raycasterMap)
        {
            _showHighPanel = showHighPanel;
            _raycasterMap = raycasterMap;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isPressed = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            StartCoroutine(IsLongPress(isLongPress =>
            {
                if (isLongPress)
                {
                    _oldPosition = transform.position;
                    _isDragged = true;
                }
                else
                    _showHighPanel.ShowPreviw(_label.PinData);
            }));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _animation.DoScale();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isPressed = false;
            _animation.ReturnScale();
        }

        public void OnDrop(PointerEventData eventData)
        {
            _isDragged = false;
            if (IsPointOnMap(transform.position))
                _label.SetPosition();
            else
                transform.position = _oldPosition;
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (_isDragged)
                transform.position += (Vector3)eventData.delta;
        }

        private bool IsPointOnMap(Vector3 point)
        {
            var topObject = _raycasterMap.GetTopObject(point);

            if (topObject != null && topObject.CompareTag("Map"))
                return true;

            return false;
        }

        private IEnumerator IsLongPress(Action<bool> isLongPress)
        {
            var timer = 0f;
            _isPressed = true;
            while (_isPressed && timer < _timePressing)
            {
                timer += Time.deltaTime;

                yield return null;
            }

            isLongPress(timer >= _timePressing);
        }
    }
}
