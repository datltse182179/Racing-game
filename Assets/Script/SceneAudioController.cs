using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAudioController : MonoBehaviour
{
    [Header("Scene Audio Settings")]
    public string[] musicScenes = { "scene_night" }; // Các scene sẽ phát nhạc
    public string[] silentScenes = { "Garage" }; // Các scene sẽ dừng nhạc
    
    [Header("Auto Setup")]
    public bool autoSetupOnStart = true;
    
    private void Start()
    {
        if (autoSetupOnStart)
        {
            CheckCurrentScene();
        }
    }
    
    private void OnEnable()
    {
        // Đăng ký event khi scene thay đổi
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDisable()
    {
        // Hủy đăng ký event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckSceneAudio(scene.name);
    }
    
    private void CheckCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        CheckSceneAudio(currentScene);
    }
    
    private void CheckSceneAudio(string sceneName)
    {
        if (AudioManager.Instance == null) return;
        
        // Kiểm tra xem scene có trong danh sách phát nhạc không
        bool shouldPlayMusic = IsSceneInList(sceneName, musicScenes);
        bool shouldStopMusic = IsSceneInList(sceneName, silentScenes);
        
        if (shouldPlayMusic)
        {
            Debug.Log($"Scene '{sceneName}' - Bắt đầu phát nhạc");
            AudioManager.Instance.PlayMusic();
        }
        else if (shouldStopMusic)
        {
            Debug.Log($"Scene '{sceneName}' - Dừng nhạc");
            AudioManager.Instance.StopMusic();
        }
        else
        {
            Debug.Log($"Scene '{sceneName}' - Không thay đổi trạng thái nhạc");
        }
    }
    
    private bool IsSceneInList(string sceneName, string[] sceneList)
    {
        if (sceneList == null) return false;
        
        foreach (string scene in sceneList)
        {
            if (scene == sceneName)
                return true;
        }
        return false;
    }
    
    // Phương thức để thêm scene vào danh sách phát nhạc
    public void AddMusicScene(string sceneName)
    {
        if (!IsSceneInList(sceneName, musicScenes))
        {
            string[] newList = new string[musicScenes.Length + 1];
            for (int i = 0; i < musicScenes.Length; i++)
            {
                newList[i] = musicScenes[i];
            }
            newList[musicScenes.Length] = sceneName;
            musicScenes = newList;
            
            Debug.Log($"Đã thêm scene '{sceneName}' vào danh sách phát nhạc");
        }
    }
    
    // Phương thức để thêm scene vào danh sách dừng nhạc
    public void AddSilentScene(string sceneName)
    {
        if (!IsSceneInList(sceneName, silentScenes))
        {
            string[] newList = new string[silentScenes.Length + 1];
            for (int i = 0; i < silentScenes.Length; i++)
            {
                newList[i] = silentScenes[i];
            }
            newList[silentScenes.Length] = sceneName;
            silentScenes = newList;
            
            Debug.Log($"Đã thêm scene '{sceneName}' vào danh sách dừng nhạc");
        }
    }
}
