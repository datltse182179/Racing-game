using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("All Menu's")]
    public GameObject pauseMenuUI;
    public static bool GameIsStopped = false;
    
    [Header("Audio")]
    public AudioManager audioManager;

    private void Update()
    {
        // Kiểm tra nhấn ESC để pause/resume
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsStopped)
            {
                Resume(); // Nếu đang pause thì resume
            }
            else
            {
                Pause(); // Nếu đang chạy thì pause
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsStopped = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsStopped = true;
    }

    public void Restart()
    {
        SceneManager.LoadScene("scene_night");
        Time.timeScale = 1.0f;
        
        // Phát nhạc khi chuyển sang scene_night
        if (audioManager != null)
        {
            audioManager.PlayMusic();
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Garage");
        Time.timeScale = 1f;
        
        // Dừng nhạc khi quay về Garage
        if (audioManager != null)
        {
            audioManager.StopMusic();
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
