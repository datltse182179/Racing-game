using UnityEngine;

/*
 * HƯỚNG DẪN SỬ DỤNG HỆ THỐNG ÂM THANH
 * ===================================
 * 
 * 1. THIẾT LẬP CƠ BẢN:
 *    - Thêm AudioManager vào scene chính (sẽ tự động tạo nếu dùng AudioManagerSetup)
 *    - Kéo các file nhạc (.mp3, .wav) vào mảng Music Clips của AudioManager
 *    - Thiết lập volume mong muốn (0.0 - 1.0)
 * 
 * 2. ĐIỀU KHIỂN BẰNG PHÍM:
 *    - K: Chuyển sang bài trước
 *    - L: Chuyển sang bài tiếp theo  
 *    - P: Dừng/Phát nhạc (toggle)
 * 
 * 3. TÍCH HỢP SCENE:
 *    - Nhạc sẽ tự động phát khi chuyển từ Garage sang scene_night
 *    - Nhạc sẽ dừng khi quay về Garage
 *    - Có thể tùy chỉnh danh sách scene trong SceneAudioController
 * 
 * 4. HIỂN THỊ THÔNG TIN:
 *    - Sử dụng MusicDisplay để hiển thị thông tin nhạc trên UI
 *    - Tạo Text UI và gán vào musicInfoText và controlsText
 * 
 * 5. TỰ ĐỘNG THIẾT LẬP:
 *    - Sử dụng AudioManagerSetup để tự động tạo AudioManager
 *    - Sử dụng SceneAudioController để tự động quản lý nhạc theo scene
 * 
 * 6. SCRIPT API:
 *    - AudioManager.Instance.PlayMusic() - Phát nhạc
 *    - AudioManager.Instance.StopMusic() - Dừng nhạc
 *    - AudioManager.Instance.NextTrack() - Bài tiếp theo
 *    - AudioManager.Instance.PreviousTrack() - Bài trước
 *    - AudioManager.Instance.ToggleMusic() - Bật/tắt nhạc
 * 
 * 7. LƯU Ý:
 *    - AudioManager sử dụng Singleton pattern, chỉ có 1 instance
 *    - AudioManager sẽ không bị destroy khi chuyển scene
 *    - Đảm bảo file nhạc có format được Unity hỗ trợ
 *    - Kiểm tra Audio Source settings trong AudioManager
 */

public class AudioSystemGuide : MonoBehaviour
{
    [Header("Hướng dẫn sử dụng")]
    [TextArea(10, 20)]
    public string guideText = @"
HỆ THỐNG ÂM THANH ĐÃ SẴN SÀNG!

CÁCH SỬ DỤNG:
1. Thêm AudioManager vào scene
2. Kéo nhạc vào Music Clips array
3. Nhấn Play để test

ĐIỀU KHIỂN:
- K: Bài trước
- L: Bài tiếp theo  
- P: Dừng/Phát nhạc

TÍCH HỢP:
- Nhạc tự động phát khi chuyển từ Garage sang scene_night
- Nhạc tự động dừng khi quay về Garage

THIẾT LẬP:
- Sử dụng AudioManagerSetup để tự động setup
- Sử dụng SceneAudioController để quản lý theo scene
- Sử dụng MusicDisplay để hiển thị thông tin
    ";
    
    [Header("Test Buttons")]
    public bool showTestButtons = true;
    
    private void OnGUI()
    {
        if (!showTestButtons) return;
        
        GUILayout.BeginArea(new Rect(10, 10, 300, 200));
        GUILayout.Label("🎵 Audio System Test Controls", GUI.skin.box);
        
        if (GUILayout.Button("Play Music"))
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayMusic();
        }
        
        if (GUILayout.Button("Stop Music"))
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.StopMusic();
        }
        
        if (GUILayout.Button("Next Track"))
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.NextTrack();
        }
        
        if (GUILayout.Button("Previous Track"))
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PreviousTrack();
        }
        
        if (GUILayout.Button("Toggle Music"))
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.ToggleMusic();
        }
        
        GUILayout.EndArea();
    }
}
