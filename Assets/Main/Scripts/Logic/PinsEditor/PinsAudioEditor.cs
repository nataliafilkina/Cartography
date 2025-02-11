using System.Threading.Tasks;
using UnityEngine;

namespace Logic
{
    public class PinsAudioEditor
    {
        private readonly PinsStorage _storage;

        public PinsAudioEditor(PinsStorage storageService)
        {
            _storage = storageService;
        }

        public void EditAudio(PinData pin, AudioClip clip)
        {
            _storage.RegisterChanges(pin, clip);
            pin.Clip = clip ? clip : null;
        }

        public async Task<AudioClip> GetClip(PinData pin)
        {
            if (pin.IsClipLoaded)
                return pin.Clip;

            if (!string.IsNullOrEmpty(pin.AudioName))
            {
                var clip = await _storage.LoadAudio(pin.ImageName);
                pin.Clip = clip;
            }

            pin.IsSpriteLoaded = true;

            return pin.Clip;
        }
    }
}
