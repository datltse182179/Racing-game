using UnityEngine;
using System.Collections;

public class AutoSkipAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    public float animationDuration = 3.22f; // Thời gian animation (từ CameraAnim.anim)
    public float autoSkipDelay = 0.5f; // Delay trước khi auto skip (giây)
    
    [Header("References")]
    public GameObject carSelectionObject; // GameObject có script CarSelection
    
    private bool hasSkipped = false; // Đảm bảo chỉ skip 1 lần
    
    private void Start()
    {
        // Tự động skip sau khi animation kết thúc
        StartCoroutine(AutoSkipAfterAnimation());
    }
    
    private IEnumerator AutoSkipAfterAnimation()
    {
        // Chờ animation kết thúc + delay
        yield return new WaitForSeconds(animationDuration + autoSkipDelay);
        
        // Kiểm tra nếu chưa skip và có reference đến CarSelection
        if (!hasSkipped && carSelectionObject != null)
        {
            CarSelection carSelection = carSelectionObject.GetComponent<CarSelection>();
            if (carSelection != null)
            {
                Debug.Log("Auto skip animation - calling skipButton()");
                carSelection.skipButton();
                hasSkipped = true;
            }
        }
    }
    
    // Hàm này được gọi từ Animation Event khi animation kết thúc
    public void OnAnimationComplete()
    {
        if (!hasSkipped && carSelectionObject != null)
        {
            CarSelection carSelection = carSelectionObject.GetComponent<CarSelection>();
            if (carSelection != null)
            {
                Debug.Log("Animation complete - auto calling skipButton()");
                carSelection.skipButton();
                hasSkipped = true;
            }
        }
    }
    
    // Hàm này có thể được gọi từ nút Skip thủ công
    public void ManualSkip()
    {
        if (!hasSkipped && carSelectionObject != null)
        {
            CarSelection carSelection = carSelectionObject.GetComponent<CarSelection>();
            if (carSelection != null)
            {
                Debug.Log("Manual skip - calling skipButton()");
                carSelection.skipButton();
                hasSkipped = true;
            }
        }
    }
}
