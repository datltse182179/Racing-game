using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Music Settings")]
    public AudioClip[] musicClips; // Danh sách các bài nhạc
    public AudioSource musicSource;
    
    [Header("Audio Mixing")]
    public bool useSeparateAudioSource = true; // Sử dụng AudioSource riêng cho nhạc
    
    [Header("Volume Settings")]
    [Range(0f, 1f)]
    public float musicVolume = 0.7f;
    
    private int currentMusicIndex = 0;
    private bool isMusicPlaying = false;
    private bool isPaused = false;
    
    // Singleton pattern để đảm bảo chỉ có 1 AudioManager
    public static AudioManager Instance { get; private set; }
    
    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Khởi tạo AudioSource nếu chưa có
        if (musicSource == null && useSeparateAudioSource)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            Debug.Log("✅ Đã tạo AudioSource riêng cho AudioManager");
        }
        
        // Thiết lập AudioSource
        if (musicSource != null)
        {
            musicSource.loop = true;
            musicSource.volume = musicVolume;
            musicSource.playOnAwake = false;
            musicSource.priority = 64; // Ưu tiên cao hơn tiếng động cơ
            musicSource.spatialBlend = 0f; // 2D sound (không bị ảnh hưởng bởi vị trí)
        }
    }
    
    private void Start()
    {
        // Kiểm tra scene hiện tại và phát nhạc nếu cần
        CheckSceneAndPlayMusic();
    }
    
    private void Update()
    {
        // Xử lý input cho điều khiển nhạc
        HandleMusicControls();
    }
    
    private void CheckSceneAndPlayMusic()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        
        // Phát nhạc khi chuyển từ Garage sang scene_night
        if (currentScene == "scene_night" && !isMusicPlaying)
        {
            PlayMusic();
        }
    }
    
    private void HandleMusicControls()
    {
        // Phím K: Bài trước
        if (Input.GetKeyDown(KeyCode.K))
        {
            PreviousTrack();
        }
        
        // Phím L: Bài tiếp theo
        if (Input.GetKeyDown(KeyCode.L))
        {
            NextTrack();
        }
        
        // Phím P: Dừng/Phát nhạc
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleMusic();
        }
    }
    
    public void PlayMusic()
    {
        if (musicClips.Length == 0)
        {
            Debug.LogWarning("Không có bài nhạc nào được thiết lập!");
            return;
        }
        
        // Kiểm tra musicSource
        if (musicSource == null)
        {
            Debug.LogWarning("MusicSource bị null! Đang tạo mới...");
            if (useSeparateAudioSource)
            {
                musicSource = gameObject.AddComponent<AudioSource>();
                musicSource.loop = true;
                musicSource.volume = musicVolume;
                musicSource.playOnAwake = false;
                musicSource.priority = 128;
            }
            else
            {
                Debug.LogError("Không thể tạo MusicSource!");
                return;
            }
        }
        
        // Đảm bảo index hợp lệ
        currentMusicIndex = Mathf.Clamp(currentMusicIndex, 0, musicClips.Length - 1);
        
        musicSource.clip = musicClips[currentMusicIndex];
        musicSource.Play();
        isMusicPlaying = true;
        isPaused = false;
        
        Debug.Log($"Đang phát bài: {musicClips[currentMusicIndex].name} ({currentMusicIndex + 1}/{musicClips.Length})");
    }
    
    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
        isMusicPlaying = false;
        isPaused = false;
        Debug.Log("Nhạc đã dừng");
    }
    
    public void PauseMusic()
    {
        if (isMusicPlaying && !isPaused && musicSource != null)
        {
            musicSource.Pause();
            isPaused = true;
            Debug.Log("Nhạc đã tạm dừng");
        }
    }
    
    public void ResumeMusic()
    {
        if (isMusicPlaying && isPaused && musicSource != null)
        {
            musicSource.UnPause();
            isPaused = false;
            Debug.Log("Nhạc đã tiếp tục");
        }
    }
    
    public void ToggleMusic()
    {
        if (!isMusicPlaying)
        {
            PlayMusic();
        }
        else if (isPaused)
        {
            ResumeMusic();
        }
        else
        {
            PauseMusic();
        }
    }
    
    public void NextTrack()
    {
        if (musicClips.Length == 0) return;
        
        currentMusicIndex = (currentMusicIndex + 1) % musicClips.Length;
        
        if (isMusicPlaying)
        {
            PlayMusic();
        }
        
        Debug.Log($"Chuyển sang bài tiếp theo: {musicClips[currentMusicIndex].name} ({currentMusicIndex + 1}/{musicClips.Length})");
    }
    
    public void PreviousTrack()
    {
        if (musicClips.Length == 0) return;
        
        currentMusicIndex = (currentMusicIndex - 1 + musicClips.Length) % musicClips.Length;
        
        if (isMusicPlaying)
        {
            PlayMusic();
        }
        
        Debug.Log($"Chuyển sang bài trước: {musicClips[currentMusicIndex].name} ({currentMusicIndex + 1}/{musicClips.Length})");
    }
    
    public void SetVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
    }
    
    // Phương thức để thêm nhạc từ script khác
    public void AddMusicClip(AudioClip clip)
    {
        if (clip != null)
        {
            // Tạo array mới với clip mới
            AudioClip[] newClips = new AudioClip[musicClips.Length + 1];
            for (int i = 0; i < musicClips.Length; i++)
            {
                newClips[i] = musicClips[i];
            }
            newClips[musicClips.Length] = clip;
            musicClips = newClips;
            
            Debug.Log($"Đã thêm bài nhạc: {clip.name}");
        }
    }
    
    // Phương thức để lấy thông tin nhạc hiện tại
    public string GetCurrentTrackInfo()
    {
        if (musicClips.Length == 0) return "Không có nhạc";
        
        string status = isPaused ? " (Tạm dừng)" : (isMusicPlaying ? " (Đang phát)" : " (Dừng)");
        return $"{musicClips[currentMusicIndex].name}{status}";
    }
}
