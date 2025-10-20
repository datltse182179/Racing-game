using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAudioInitializer : MonoBehaviour
{
    [Header("Auto Setup")]
    public bool autoCreateAudioManager = true;
    public bool autoPlayMusic = true;
    
    [Header("Music Settings")]
    public AudioClip[] musicClips;
    public float musicVolume = 0.7f;
    
    private void Start()
    {
        if (autoCreateAudioManager)
        {
            SetupAudioManager();
        }
        
        if (autoPlayMusic)
        {
            PlayMusicIfNeeded();
        }
    }
    
    private void SetupAudioManager()
    {
        // Kiểm tra xem đã có AudioManager chưa
        if (AudioManager.Instance == null)
        {
            // Tạo AudioManager mới
            GameObject audioManagerObject = new GameObject("AudioManager");
            AudioManager audioManager = audioManagerObject.AddComponent<AudioManager>();
            
            // Thiết lập nhạc
            if (musicClips.Length > 0)
            {
                audioManager.musicClips = musicClips;
            }
            
            audioManager.musicVolume = musicVolume;
            
            Debug.Log("✅ Đã tạo AudioManager trong scene city");
        }
        else
        {
            Debug.Log("✅ AudioManager đã tồn tại");
        }
    }
    
    private void PlayMusicIfNeeded()
    {
        if (AudioManager.Instance != null)
        {
            // Kiểm tra scene hiện tại
            string currentScene = SceneManager.GetActiveScene().name;
            
            if (currentScene == "scene_night")
            {
                AudioManager.Instance.PlayMusic();
                Debug.Log("🎵 Đã phát nhạc trong scene city");
            }
        }
    }
    
    // Phương thức để phát nhạc thủ công
    public void PlayMusicNow()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic();
        }
    }
    
    // Phương thức để dừng nhạc
    public void StopMusicNow()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
        }
    }
}
