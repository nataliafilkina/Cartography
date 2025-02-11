using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace StorageService
{
    public class AudioStorageService
    {
        private string _folderName = "Audio";
        private string _folderPath;

        public async void SaveWav(AudioClip clip, string audioName)
        {
            if (clip == null)
                return;

            if (string.IsNullOrEmpty(_folderPath))
                _folderPath = Path.Combine(Application.persistentDataPath, _folderName);

            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }

            string path = BuildPath(audioName);
            byte[] wavData = ConvertToWav(clip);

            await File.WriteAllBytesAsync(path, wavData);
        }

        public async Task<AudioClip> LoadWav(string audioName)
        {
            string path = BuildPath(audioName);
            return await LoadWavByPath(path);
        }

        public async Task<AudioClip> LoadWavByPath(string audioPath)
        {
            if (!File.Exists(audioPath))
                return null;

            var clip = await LoadAudioFromBytes(audioPath);
            return clip;
        }

        public void Remove(string audioName)
        {
            string audioPath = BuildPath(audioName);
            if (File.Exists(audioPath))
            {
                File.Delete(audioPath);
            }
        }

        private string BuildPath(string fileName)
        {
            if (string.IsNullOrEmpty(_folderPath))
                _folderPath = Path.Combine(Application.persistentDataPath, _folderName);

            return Path.Combine(_folderPath, fileName + ".wav");
        }

        private byte[] ConvertToWav(AudioClip clip)
        {
            MemoryStream stream = new();
            using (BinaryWriter writer = new(stream))
            {
                int sampleRate = clip.frequency;
                int channels = clip.channels;
                float[] samples = new float[clip.samples * channels];
                clip.GetData(samples, 0);

                writer.Write(Encoding.UTF8.GetBytes("RIFF"));
                writer.Write(36 + samples.Length * 2);
                writer.Write(Encoding.UTF8.GetBytes("WAVE"));
                writer.Write(Encoding.UTF8.GetBytes("fmt "));
                writer.Write(16);
                writer.Write((short)1);
                writer.Write((short)channels);
                writer.Write(sampleRate);
                writer.Write(sampleRate * channels * 2);
                writer.Write((short)(channels * 2));
                writer.Write((short)16);
                writer.Write(Encoding.UTF8.GetBytes("data"));
                writer.Write(samples.Length * 2);

                foreach (float sample in samples)
                {
                    short intSample = (short)(sample * short.MaxValue);
                    writer.Write(intSample);
                }
            }
            return stream.ToArray();
        }

        private async Task<AudioClip> LoadAudioFromBytes(string audioPath)
        {
            string tempPath = "file://" + audioPath;
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(tempPath, AudioType.WAV))
            {
                var request = www.SendWebRequest();
                while (!request.isDone) 
                {
                    await Task.Yield();
                }

                if (www.result != UnityWebRequest.Result.Success)
                    return null;
                else
                   return DownloadHandlerAudioClip.GetContent(www);
            }
        }

    }
}
