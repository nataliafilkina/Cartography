using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class UIGraphicRaycaster : MonoBehaviour
    {
        private EventSystem _eventSystem;
        private GraphicRaycaster _raycaster;

        private void Awake()
        {
            _raycaster = GetComponent<GraphicRaycaster>();
            _eventSystem = GetComponent<EventSystem>();
        }

        public GameObject GetTopObject(Vector3 point)
        {
            PointerEventData pointerData = new(_eventSystem)
            {
                position = point
            };

            List<RaycastResult> results = new();
            _raycaster.Raycast(pointerData, results);

            if (results.Count < 1)
                return null;

            foreach (var result in results)
            {
                if (IsPixelOpaque(result.gameObject, point))
                    return result.gameObject;
            }

            return null;
        }

        public bool IsPixelOpaque(GameObject uiObject, Vector2 point)
        {
            Image image = uiObject.GetComponent<Image>();
            if (!image.sprite.texture.isReadable)
                return false;

            RectTransform rectTransform = uiObject.GetComponent<RectTransform>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                point,
                null,
                out Vector2 localPoint
            );

            Rect spriteRect = image.sprite.rect;
            int texX = Mathf.FloorToInt((localPoint.x + rectTransform.rect.width / 2) / rectTransform.rect.width * spriteRect.width);
            int texY = Mathf.FloorToInt((localPoint.y + rectTransform.rect.height / 2) / rectTransform.rect.height * spriteRect.height);

            if (texX < 0 || texX >= spriteRect.width || texY < 0 || texY >= spriteRect.height)
                return false;

            Color pixelColor = image.sprite.texture.GetPixel(texX, texY);
            return pixelColor.a > 0.1f;
        }
    }
}
