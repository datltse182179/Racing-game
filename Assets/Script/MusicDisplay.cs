using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicDisplay : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI musicInfoText;
    public TextMeshProUGUI controlsText;
    
    [Header("Display Settings")]
    public float displayDuration = 3f; // Thời gian hiển thị thông tin (giây)
    public bool showControls = true;
    
    private float displayTimer = 0f;
    private bool isDisplaying = false;
    
    private void Start()
    {
        // Ẩn text ban đầu
        if (musicInfoText != null)
            musicInfoText.gameObject.SetActive(false);
            
        if (controlsText != null)
            controlsText.gameObject.SetActive(showControls);
    }
    
    private void Update()
    {
        // Cập nhật thông tin nhạc
        UpdateMusicInfo();
        
        // Xử lý hiển thị thông tin
        HandleDisplayTimer();
        
        // Cập nhật text điều khiển
        UpdateControlsText();
    }
    
    private void UpdateMusicInfo()
    {
        if (AudioManager.Instance != null && musicInfoText != null)
        {
            string info = AudioManager.Instance.GetCurrentTrackInfo();
            musicInfoText.text = $"🎵 {info}";
        }
    }
    
    private void HandleDisplayTimer()
    {
        if (isDisplaying)
        {
            displayTimer -= Time.deltaTime;
            
            if (displayTimer <= 0f)
            {
                HideMusicInfo();
            }
        }
    }
    
    private void UpdateControlsText()
    {
        if (controlsText != null && showControls)
        {
            controlsText.text = 
                               "K - Previous\n" +
                               "L - Next\n" +
                               "P - Play/Pause";
        }
    }
    
    public void ShowMusicInfo()
    {
        if (musicInfoText != null)
        {
            musicInfoText.gameObject.SetActive(true);
            displayTimer = displayDuration;
            isDisplaying = true;
        }
    }
    
    public void HideMusicInfo()
    {
        if (musicInfoText != null)
        {
            musicInfoText.gameObject.SetActive(false);
            isDisplaying = false;
        }
    }
    
    public void ToggleControls()
    {
        showControls = !showControls;
        if (controlsText != null)
        {
            controlsText.gameObject.SetActive(showControls);
        }
    }
}
