using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoAudioSetup : MonoBehaviour
{
    [Header("Auto Setup")]
    public bool setupOnStart = true;
    public bool playMusicOnStart = true;
    
    [Header("Music Settings")]
    public AudioClip[] musicClips;
    public float musicVolume = 0.7f;
    
    private void Start()
    {
        if (setupOnStart)
        {
            SetupAudioManager();
        }
        
        if (playMusicOnStart)
        {
            PlayMusicAfterDelay();
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
            audioManager.useSeparateAudioSource = true;
            
            Debug.Log("✅ Đã tạo AudioManager trong scene_night");
        }
        else
        {
            Debug.Log("✅ AudioManager đã tồn tại");
        }
    }
    
    private void PlayMusicAfterDelay()
    {
        // Delay một chút để đảm bảo AudioManager đã sẵn sàng
        Invoke(nameof(PlayMusicNow), 0.5f);
    }
    
    private void PlayMusicNow()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic();
            Debug.Log("🎵 Đã phát nhạc trong scene_night");
        }
        else
        {
            Debug.LogWarning("⚠️ AudioManager.Instance vẫn null!");
        }
    }
    
    // Phương thức để phát nhạc thủ công
    public void PlayMusic()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic();
        }
    }
    
    // Phương thức để dừng nhạc
    public void StopMusic()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
        }
    }
}
