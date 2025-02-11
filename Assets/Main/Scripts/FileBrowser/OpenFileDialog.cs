using UnityEngine;
using SFB;
using StorageService;
using System.Threading.Tasks;
using System.Linq;

public class OpenFileDialog : MonoBehaviour
{
    private ImageStorageService _imageStorage = new();
    private AudioStorageService _audioStorageService = new();

    public async Task<Texture2D> OpenImage()
    {
        var extensions = new[] {
            new ExtensionFilter("Image Files", "png"),
        };
        var paths = StandaloneFileBrowser.OpenFilePanel("Выберите изображение", "", extensions, false);
        if (paths.Length > 0)
        {
            var texture = await _imageStorage.LoadByPath(paths[0]);
            return texture;
        }
        return null;
    }

    public async Task<AudioClip> OpenAudio()
    {
        var extensions = new[] {
            new ExtensionFilter("Sound Files", "wav"),
        };
        var paths = StandaloneFileBrowser.OpenFilePanel("Выберите аудиофайл", "", extensions, false);
        if (paths.Length > 0)
        {
            var clip = await _audioStorageService.LoadWavByPath(paths[0]);
            return clip;
        }
        return null;
    }
}
