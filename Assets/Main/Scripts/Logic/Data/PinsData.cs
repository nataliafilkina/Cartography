using System.Collections.Generic;

namespace Logic
{
    public class PinsData
    {
        public int CurrentMaxObjectId { get; set; }
        public int CurrentMaxImageId { get; set; }
        public int CurrentMaxAudioId { get; set; }

        public List<PinData> Pins = new();
    }
}

