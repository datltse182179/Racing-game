using UnityEngine;

public class AudioManagerSetup : MonoBehaviour
{
    [Header("Audio Setup")]
    public AudioClip[] defaultMusicClips; // Nhạc mặc định
    public float defaultVolume = 0.7f;
    
    [Header("Auto Setup")]
    public bool autoSetupOnStart = true;
    public bool createAudioManagerIfMissing = true;
    
    private void Start()
    {
        if (autoSetupOnStart)
        {
            SetupAudioManager();
        }
    }
    
    public void SetupAudioManager()
    {
        // Kiểm tra xem đã có AudioManager chưa
        if (AudioManager.Instance == null && createAudioManagerIfMissing)
        {
            CreateAudioManager();
        }
        
        // Thiết lập nhạc mặc định nếu có
        if (AudioManager.Instance != null && defaultMusicClips.Length > 0)
        {
            AudioManager.Instance.musicClips = defaultMusicClips;
            AudioManager.Instance.SetVolume(defaultVolume);
            
            Debug.Log($"Đã thiết lập {defaultMusicClips.Length} bài nhạc mặc định");
        }
    }
    
    private void CreateAudioManager()
    {
        // Tạo GameObject mới cho AudioManager
        GameObject audioManagerObject = new GameObject("AudioManager");
        
        // Thêm AudioManager component
        AudioManager audioManager = audioManagerObject.AddComponent<AudioManager>();
        
        // Thiết lập nhạc mặc định
        if (defaultMusicClips.Length > 0)
        {
            audioManager.musicClips = defaultMusicClips;
        }
        
        // Thiết lập volume
        audioManager.musicVolume = defaultVolume;
        
        Debug.Log("Đã tạo AudioManager tự động");
    }
    
    // Phương thức để thêm nhạc từ Inspector
    public void AddMusicClip(AudioClip clip)
    {
        if (AudioManager.Instance != null && clip != null)
        {
            AudioManager.Instance.AddMusicClip(clip);
        }
    }
    
    // Phương thức để phát nhạc ngay lập tức
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
