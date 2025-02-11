using StorageService;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Logic
{
    public class PinsStorage
    {
        private JsonFileStorageService _jsonStorageService = new();
        private ImageStorageService _imageStorage = new();
        private AudioStorageService _audioStorage = new();
        private string _fileName = "PinsData";

        private Dictionary<string, string> _removedImageByIdPin = new();
        private Dictionary<PinData, Texture2D> _addedImageByIdPin = new();

        private Dictionary<string, string> _removedAudioByIdPin = new();
        private Dictionary<PinData, AudioClip> _addedAudioByIdPin = new();

        public PinsData LoadData()
        {
            PinsData pinsData = null;
            _jsonStorageService.Load<PinsData>(_fileName, data =>
            {
                if (data == null)
                {
                    data = new PinsData();
                    _jsonStorageService.Save(_fileName, data);
                }
                pinsData = data;
            });
            return pinsData;
        }

        public Task<Texture2D> LoadImage(string fileName) => _imageStorage.Load(fileName);
        public Task<AudioClip> LoadAudio(string fileName) => _audioStorage.LoadWav(fileName);

        public void SaveData(PinsData data)
        {
            data.CurrentMaxImageId = SaveImages(data.CurrentMaxImageId);
            data.CurrentMaxAudioId = SaveAudio(data.CurrentMaxAudioId);
            _jsonStorageService.Save(_fileName, data);
        }

        public void RemoveImage(string pinId, string fileName)
        {
            _removedImageByIdPin.TryAdd(pinId, fileName);
        }

        public void RemoveAudio(string pinId, string fileName)
        {
            _removedAudioByIdPin.TryAdd(pinId, fileName);
        }

        public void RegisterChanges(PinData pin, Texture2D texture)
        {
            if(!string.IsNullOrEmpty(pin.ImageName))
                RemoveImage(pin.Id, pin.ImageName);
            _addedImageByIdPin[pin] = texture;
        }

        public void RegisterChanges(PinData pin, AudioClip clip)
        {
            if (!string.IsNullOrEmpty(pin.AudioName))
                RemoveAudio(pin.Id, pin.AudioName);
            _addedAudioByIdPin[pin] = clip;
        }

        private int SaveImages(int maxImageId) 
        {
            foreach (var remove in _removedImageByIdPin)
                _imageStorage.Remove(remove.Value);

            foreach(var add in _addedImageByIdPin)
            {
                if (add.Value != null)
                {
                    string imageName = (++maxImageId).ToString();
                    add.Key.ImageName = imageName;
                    _imageStorage.Save(add.Value, imageName);
                }
                else
                    add.Key.ImageName = "";
            }
            _removedImageByIdPin.Clear();
            _addedImageByIdPin.Clear();
            return maxImageId;
        }

        private int SaveAudio(int maxClipId)
        {
            foreach (var remove in _removedAudioByIdPin)
                _audioStorage.Remove(remove.Value);

            foreach (var add in _addedAudioByIdPin)
            {
                if (add.Value != null)
                {
                    string imageName = (++maxClipId).ToString();
                    add.Key.ImageName = imageName;
                    _audioStorage.SaveWav(add.Value, imageName);
                }
                else
                    add.Key.ImageName = "";
            }
            _removedImageByIdPin.Clear();
            _addedImageByIdPin.Clear();
            return maxClipId;
        }
    }
}
