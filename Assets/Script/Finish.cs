using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Finish : MonoBehaviour
{
    [Header("Finish UI Var")]
    public GameObject finishUI;
    public GameObject playerUI;
    public GameObject playerCar;

    [Header("Win/Lose Status")]
    public TextMeshProUGUI status;

    private void Start()
    {
        StartCoroutine(waitforthefinishUI());
    }

    private void OnTriggerEnter(Collider other)
    {
        // Chỉ kích hoạt khi collider đã được bật (sau 25 giây)
        if (!gameObject.GetComponent<BoxCollider>().enabled)
            return;
            
        Debug.Log($"Finish line triggered by: {other.gameObject.tag}");
            
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player reached finish line - You Win!");
            StartCoroutine(finishZoneTimer());

            status.text = "You Win";
            status.color = Color.black;
        }
        else if (other.gameObject.tag == "OpponentCar")
        {
            Debug.Log("Opponent reached finish line - You Lose!");
            StartCoroutine(finishZoneTimer());

            status.text = "You Lose";
            status.color = Color.red;
        }
    }

    IEnumerator waitforthefinishUI()
    {
        // Đảm bảo collider là trigger và được bật
        BoxCollider finishCollider = gameObject.GetComponent<BoxCollider>();
        finishCollider.isTrigger = true; // Đảm bảo là trigger
        finishCollider.enabled = false; // Tắt trong 25 giây đầu
        
        Debug.Log("Finish line collider disabled for 25 seconds");
        
        yield return new WaitForSeconds(25f);
        
        finishCollider.enabled = true; // Bật lại sau 25 giây để có thể trigger
        Debug.Log("Finish line collider enabled - ready to trigger!");
    }

    IEnumerator finishZoneTimer()
    {
        finishUI.SetActive(true);
        playerUI.SetActive(false);
        playerCar.SetActive(false);

        yield return new WaitForSeconds(5f);
        Time.timeScale = 0f;
    }
}
