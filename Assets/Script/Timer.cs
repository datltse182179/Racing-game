using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Timer")]
    public float countDownTimer = 5f;

    [Header("Things to stop")]
    public PlayerCarController playerCarController;
    public PlayerCarController playerCarController1;
    public OpponentCar opponentCar;
    public OpponentCar opponentCar1;

    public TextMeshProUGUI countDownText;

    void Start()
    {
        StartCoroutine(TimeCount());
    }

    // Update is called once per frame
    void Update()
    {
        // Kiểm tra nếu game đang pause thì ẩn countdown
        if (Menu.GameIsStopped)
        {
            countDownText.gameObject.SetActive(false);
            return;
        }
        else
        {
            // Hiện lại countdown khi không pause
            if (countDownTimer > 0)
            {
                countDownText.gameObject.SetActive(true);
            }
        }

        if(countDownTimer > 1)
        {
            playerCarController.acclerationForce = 0f;
            playerCarController1.acclerationForce = 0f;
            opponentCar.movingSpeed = 0f;
            opponentCar1.movingSpeed = 0f;
        }
        else if(countDownTimer == 0)
        {
            playerCarController.acclerationForce = 300f;
            playerCarController1.acclerationForce = 300f;
            opponentCar.movingSpeed = 12f;
            opponentCar1.movingSpeed = 13f;
        }
    }


    IEnumerator TimeCount()
    {
        while(countDownTimer > 0)
        {
            // Kiểm tra nếu game đang pause thì không đếm
            if (Menu.GameIsStopped)
            {
                yield return new WaitForSeconds(0.1f); // Chờ ngắn rồi kiểm tra lại
                continue;
            }

            countDownText.text = countDownTimer.ToString();
            yield return new WaitForSeconds(1f);
            countDownTimer--;
        }

        countDownText.text = "GO";
        yield return new WaitForSeconds(1f);
        countDownText.gameObject.SetActive(false);
    }
}
