using UnityEngine;

public class AudioMixer : MonoBehaviour
{
    [Header("Audio Mixing")]
    [Range(0f, 1f)]
    public float musicVolume = 0.7f;
    
    [Range(0f, 1f)]
    public float carVolume = 0.3f;
    
    [Header("Auto Adjust")]
    public bool autoAdjustOnStart = true;
    
    private AudioSource musicAudioSource;
    private AudioSource carAudioSource;
    
    private void Start()
    {
        if (autoAdjustOnStart)
        {
            FindAudioSources();
            AdjustAudioLevels();
        }
    }
    
    private void FindAudioSources()
    {
        // Tìm AudioSource của nhạc nền
        if (AudioManager.Instance != null && AudioManager.Instance.musicSource != null)
        {
            musicAudioSource = AudioManager.Instance.musicSource;
        }
        
        // Tìm AudioSource của xe
        PlayerCarController playerCar = FindObjectOfType<PlayerCarController>();
        if (playerCar != null && playerCar.audioSource != null)
        {
            carAudioSource = playerCar.audioSource;
        }
    }
    
    private void AdjustAudioLevels()
    {
        // Điều chỉnh nhạc nền
        if (musicAudioSource != null)
        {
            musicAudioSource.volume = musicVolume;
            musicAudioSource.priority = 64; // Ưu tiên cao
            musicAudioSource.spatialBlend = 0f; // 2D sound
            Debug.Log($"🎵 Nhạc nền: Volume = {musicVolume}, Priority = 64");
        }
        
        // Điều chỉnh tiếng động cơ
        if (carAudioSource != null)
        {
            carAudioSource.volume = carVolume;
            carAudioSource.priority = 128; // Ưu tiên thấp
            carAudioSource.spatialBlend = 1f; // 3D sound
            Debug.Log($"🚗 Tiếng động cơ: Volume = {carVolume}, Priority = 128");
        }
    }
    
    // Phương thức để điều chỉnh volume thủ công
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicAudioSource != null)
        {
            musicAudioSource.volume = musicVolume;
        }
    }
    
    public void SetCarVolume(float volume)
    {
        carVolume = Mathf.Clamp01(volume);
        if (carAudioSource != null)
        {
            carAudioSource.volume = carVolume;
        }
    }
    
    // Phương thức để tắt tiếng động cơ tạm thời
    public void MuteCarEngine()
    {
        if (carAudioSource != null)
        {
            carAudioSource.volume = 0f;
            Debug.Log("🔇 Đã tắt tiếng động cơ");
        }
    }
    
    // Phương thức để bật lại tiếng động cơ
    public void UnmuteCarEngine()
    {
        if (carAudioSource != null)
        {
            carAudioSource.volume = carVolume;
            Debug.Log("🔊 Đã bật lại tiếng động cơ");
        }
    }
}
