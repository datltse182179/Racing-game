using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CarSelection : MonoBehaviour
{
    [Header("Button and Canvas")]
    public Button nextButton;
    public Button previousButton;
    
    [Header("Audio")]
    public AudioManager audioManager;

    [Header("Cameras")]
    public GameObject cam1;
    public GameObject cam2;

    [Header("Buttons and Canvas")]
    public GameObject selectionCanvas;
    public GameObject SkipButton;
    public GameObject PlayButton;

    private int currentCar;
    private GameObject[] carList;

    private void Awake()
    {
        selectionCanvas.SetActive(false);
        PlayButton.SetActive(false);
        cam2.SetActive(false);
        chooseCar(0);
    }

    private void Start()
    {
        currentCar = PlayerPrefs.GetInt("CarSelected");

        //feeding car models to carList array
        carList = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
            carList[i] = transform.GetChild(i).gameObject;

        //keeping track of currentcar
        foreach (GameObject go in carList) 
            go.SetActive(false);

        if (carList[currentCar])
            carList[currentCar].SetActive(true);
    }

    private void chooseCar(int index)
    {
        previousButton.interactable = (currentCar != 0);
        nextButton.interactable =(currentCar != transform.childCount - 1);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == index);
        }
    }

    public void switchCar(int switchCars)
    {
        currentCar += switchCars;
        chooseCar(currentCar);
    }

    public void playGame()
    {
        PlayerPrefs.SetInt("CarSelected", currentCar);
        SceneManager.LoadScene("scene_night");
        
        // Phát nhạc khi chuyển từ Garage sang scene_night
        if (audioManager != null)
        {
            audioManager.PlayMusic();
        }
    }

    public void skipButton()
    {
        selectionCanvas.SetActive(true);
        PlayButton.SetActive(true);
        SkipButton.SetActive(false);
        cam1.SetActive(false);
        cam2.SetActive(true);
    }
}
