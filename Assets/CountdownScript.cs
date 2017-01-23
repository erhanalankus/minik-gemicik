using UnityEngine;
using UnityEngine.UI;

public class CountdownScript : MonoBehaviour
{
    public Text CountdownUIText;
    public int roundTime = 30;
    public bool countDownIsActive = false;

    float referenceTime = 0;

    void Update()
    {
        if (countDownIsActive)
        {
            referenceTime += Time.deltaTime;
            if (referenceTime >= 1)
            {
                roundTime--;
                CountdownUIText.text = roundTime.ToString();
                referenceTime = 0;
                if (roundTime <= 0)
                {
                    CountdownUIText.text = "0";
                    StopCountdown();
                    GetComponent<GMScript>().EndGame(GMScript.EndGameCondition.ShipWon);
                }
            }
        }
    }

    public void StartCountdown()
    {
        CountdownUIText.text = roundTime.ToString();
        countDownIsActive = true;
    }

    public void StopCountdown()
    {
        countDownIsActive = false;
    }
}
