using UnityEngine;

public class CarAudioChecker : MonoBehaviour
{
    [Header("Car Audio Debug")]
    public bool showDebugInfo = true;
    public bool fixAudioIssues = true;
    
    private AudioSource[] allAudioSources;
    private AudioSource carEngineAudio;
    
    private void Start()
    {
        // Tìm tất cả AudioSource trong scene
        allAudioSources = FindObjectsOfType<AudioSource>();
        
        // Tìm AudioSource của xe (thường là PlayerCarController)
        PlayerCarController playerCar = FindObjectOfType<PlayerCarController>();
        if (playerCar != null && playerCar.audioSource != null)
        {
            carEngineAudio = playerCar.audioSource;
        }
        
        // Kiểm tra và fix audio issues
        if (fixAudioIssues)
        {
            FixAudioIssues();
        }
        
        if (showDebugInfo)
        {
            DebugAudioInfo();
        }
    }
    
    private void FixAudioIssues()
    {
        // Đảm bảo AudioSource của xe hoạt động
        if (carEngineAudio != null)
        {
            carEngineAudio.enabled = true;
            carEngineAudio.playOnAwake = true;
            carEngineAudio.volume = Mathf.Max(carEngineAudio.volume, 0.3f); // Giảm volume xe
            carEngineAudio.priority = 128; // Ưu tiên thấp hơn nhạc nền
            carEngineAudio.spatialBlend = 1f; // 3D sound (theo vị trí xe)
            
            Debug.Log("✅ Đã fix AudioSource của xe");
        }
        
        // Đảm bảo AudioManager không ảnh hưởng xe
        if (AudioManager.Instance != null && AudioManager.Instance.musicSource != null)
        {
            AudioManager.Instance.musicSource.priority = 128; // Ưu tiên thấp hơn xe
            AudioManager.Instance.musicSource.volume = Mathf.Min(AudioManager.Instance.musicSource.volume, 0.7f);
            
            Debug.Log("✅ Đã điều chỉnh AudioManager không ảnh hưởng xe");
        }
    }
    
    private void DebugAudioInfo()
    {
        Debug.Log("🔍 AUDIO DEBUG INFO:");
        Debug.Log($"Tổng số AudioSource trong scene: {allAudioSources.Length}");
        
        if (carEngineAudio != null)
        {
            Debug.Log($"Xe AudioSource: {carEngineAudio.name} - Enabled: {carEngineAudio.enabled} - Volume: {carEngineAudio.volume}");
        }
        
        if (AudioManager.Instance != null)
        {
            if (AudioManager.Instance.musicSource != null)
            {
                Debug.Log($"✅ Nhạc nền AudioSource: {AudioManager.Instance.musicSource.name} - Volume: {AudioManager.Instance.musicSource.volume}");
            }
            else
            {
                Debug.LogWarning("⚠️ AudioManager không có MusicSource!");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ Không tìm thấy AudioManager Instance!");
        }
        
        // Kiểm tra xem có AudioSource nào bị tắt không
        foreach (AudioSource audio in allAudioSources)
        {
            if (!audio.enabled)
            {
                Debug.LogWarning($"⚠️ AudioSource bị tắt: {audio.name}");
            }
        }
        
        // Liệt kê tất cả AudioSource
        for (int i = 0; i < allAudioSources.Length; i++)
        {
            Debug.Log($"AudioSource {i}: {allAudioSources[i].name} - Enabled: {allAudioSources[i].enabled}");
        }
    }
    
    // Phương thức để bật lại tiếng động cơ
    public void EnableCarEngine()
    {
        if (carEngineAudio != null)
        {
            carEngineAudio.enabled = true;
            carEngineAudio.volume = 0.5f; // Volume mặc định
            Debug.Log("🔊 Đã bật lại tiếng động cơ");
        }
    }
    
    // Phương thức để tắt nhạc nền tạm thời
    public void MuteBackgroundMusic()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetVolume(0f);
            Debug.Log("🔇 Đã tắt nhạc nền");
        }
    }
    
    // Phương thức để bật lại nhạc nền
    public void UnmuteBackgroundMusic()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetVolume(0.7f);
            Debug.Log("🔊 Đã bật lại nhạc nền");
        }
    }
}
