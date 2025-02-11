using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace StorageService
{
    public class ImageStorageService
    {
        private string _folderName = "Images";
        private string _folderPath;

        public string Save(Texture2D texture, string imageName)
        {
            if(string.IsNullOrEmpty(_folderPath))
                _folderPath = Path.Combine(Application.persistentDataPath, _folderName);

            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }

            byte[] pngData = texture.EncodeToPNG();
            if (pngData != null)
            {
                string imagePath = BuildPath(imageName);
                File.WriteAllBytes(imagePath, pngData);
                return imagePath;
            }
            else
            {
                return "";
            }
        }

        public async Task<Texture2D> Load(string imageName)
        {
            string imagePath = BuildPath(imageName);
            return await LoadByPath(imagePath);
        }

        public async Task<Texture2D> LoadByPath(string imagePath)
        {
            if (File.Exists(imagePath))
            {
                byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);
                Texture2D texture = new(2, 2);
                if (texture.LoadImage(imageBytes))
                {
                    return texture;
                }
            }
            return null;
        }

        public void Remove(string imageName)
        {
            string imagePath = BuildPath(imageName);
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }

        private string BuildPath(string name)
        {
            if (string.IsNullOrEmpty(_folderPath))
                _folderPath = Path.Combine(Application.persistentDataPath, _folderName);

            return Path.Combine(_folderPath, name + ".png");
        }
    }
}

