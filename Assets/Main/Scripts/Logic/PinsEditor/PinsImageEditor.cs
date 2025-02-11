using System.Threading.Tasks;
using UnityEngine;

namespace Logic
{
    public class PinsImageEditor
    {
        private readonly PinsStorage _storage;

        public PinsImageEditor(PinsStorage storageService)
        {
            _storage = storageService;
        }

        public void EditImage(PinData pin, Texture2D texture)
        {
            _storage.RegisterChanges(pin, texture);
            pin.Sprite = texture ? Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)) : null;
        }

        public async Task<Sprite> GetSprite(PinData pin)
        {
            if (pin.IsSpriteLoaded)
                return pin.Sprite;

            if (!string.IsNullOrEmpty(pin.ImageName))
            {
                var texture = await _storage.LoadImage(pin.ImageName);
                var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                pin.Sprite = sprite;
            }

            pin.IsSpriteLoaded = true;

            return pin.Sprite;
        }
    }
}

