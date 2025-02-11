using System;
using System.Text.Json.Serialization;
using UnityEngine;

namespace Logic
{
    public class PinData
    {
        public event Action OnEdited;

        [JsonInclude]
        public string Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public string AudioName { get; set; }
        public SerializableVector3 Position { get; set; }

        [JsonIgnore]
        public Sprite Sprite { get; set; }
        [JsonIgnore]
        public AudioClip Clip { get; set; }
        [JsonIgnore]
        public bool IsSpriteLoaded = false;
        [JsonIgnore]
        public bool IsClipLoaded = false;

        public PinData() { }

        public PinData(string id, string name, string description, Vector3 position, string imageName = "", string audioName = "")
        {
            Id = id;
            Edit(name, description, position, imageName, audioName);
        }

        public PinData(PinData pinData)
        {
            Id = pinData.Id;
            Name = pinData.Name;
            Description = pinData.Description;
            Position = pinData.Position;
            ImageName = pinData.ImageName;
            AudioName = pinData.AudioName;
        }

        public void Edit(string name, string description, Vector3 position, string imageName, string audioName)
        {
            Name = name;
            Description = description;
            Position = position;
            ImageName = imageName;
            AudioName = audioName;
        }

        public void Edit(PinData newData)
        {
            Name = newData.Name;
            Description = newData.Description;

            Position = newData.Position;

            OnEdited?.Invoke();
        }
    }
}
