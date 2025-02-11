using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UI;
using UnityEngine;

namespace Logic
{
    public class PinsEditor
    {
        public event Action OnFirstChanged;
        #region Fields
        private PinsStorage _storage = new();
        private UIPinSpawner _spawner;
        private PinsData _data;
        private PinsImageEditor _imageEditor;
        private PinsAudioEditor _audioEditor;

        public IReadOnlyList<PinData> Pins => _data.Pins.AsReadOnly();
        public bool HasChanged { get; private set; } = false;
        #endregion

        public PinsEditor(UIPinSpawner spawner)
        {
            _data = _storage.LoadData();
            _spawner = spawner;
            _imageEditor = new(_storage);
            _audioEditor = new(_storage);
        }

        public void Begin()
        {
            _spawner.Begin(this);
        }

        public void Save()
        {
            _storage.SaveData(_data);
            HasChanged = false;
        }

        public PinData Create(string name, string description, Vector2 position, Texture2D texture = null, AudioClip clip = null)
        {
            Sprite sprite = texture ? Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)) : null;

            string id = (++_data.CurrentMaxObjectId).ToString();
            var pin = new PinData(id, name, description, position, "", "")
            {
                Sprite = sprite,
                Clip = clip
            };
            _storage.RegisterChanges(pin, texture);
            _storage.RegisterChanges(pin, clip);
            pin.IsSpriteLoaded = true;

            _data.Pins.Add(pin);
            _spawner.AddLabel(pin);

            HandleFirstChange();

            return pin;
        }

        public void Edit(PinData pin, PinData newPinData, Texture2D texture = null, bool editTexture = false,
                         AudioClip clip = null, bool editAudio = false)
        {
            var _pin = _data.Pins.Find(p => p.Id == pin.Id);
            if (_pin != null)
            {
                if(editTexture) _imageEditor.EditImage(_pin, texture);
                if(editAudio) _audioEditor.EditAudio(_pin, clip);
                _pin.Edit(newPinData);
                HandleFirstChange();
            }
        }

        public void Remove(PinData pin)
        {
            _storage.RemoveImage(pin.Id, pin.ImageName);
            _storage.RemoveAudio(pin.Id, pin.AudioName);
            _spawner.RemoveLabel(pin.Id);
            _data.Pins.Remove(pin);

            HandleFirstChange();
        }

        public async Task<Sprite> GetSprite(PinData pin) => await _imageEditor.GetSprite(pin);
        public async Task<AudioClip> GetClip(PinData pin) => await _audioEditor.GetClip(pin);

        private void HandleFirstChange()
        {
            if (!HasChanged)
            {
                OnFirstChanged?.Invoke();
                HasChanged = true;
            }
        }
    }
}
