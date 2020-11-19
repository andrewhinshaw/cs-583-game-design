using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreOverlay : MonoBehaviour
{
    private Text scoreText;
    private Text currentDayText;
    private Text highscoreText;
    private Text powerUpText;

    private float currentTime = 0f;
    private float startingTime = 3.0f;
    private bool startCountdown = false;

    public GameObject[] strikesLeftArray;

    private static ScoreOverlay instance;

    public static ScoreOverlay GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
        scoreText = transform.Find("scoreText").GetComponent<Text>();
        currentDayText = transform.Find("currentDayText").GetComponent<Text>();
        highscoreText = transform.Find("highscoreText").GetComponent<Text>();
        powerUpText = transform.Find("powerUpText").GetComponent<Text>();
    }

    private void Start()
    {
        highscoreText.text = "HIGHSCORE: " + Score.GetHighScore().ToString();
        currentDayText.text = "DAY " + Rocket.GetInstance().GetCurrentDay().ToString();
        currentTime = startingTime;
        HidePowerUpMessage();
    }

    private void Update()
    {
        scoreText.text = Rocket.GetInstance().GetCurrentScore().ToString();

        if (startCountdown == true)
        {
            currentTime -= 1 * Time.deltaTime;
        }
        if (currentTime < 0)
        {
            HidePowerUpMessage();
        }
    }

    public void UpdateStrikeOverlay()
    {
        GameObject strike;
        for (int i = 0; i < strikesLeftArray.Length; i++)
        {
            strike = strikesLeftArray[i];
            strike.gameObject.SetActive(false);
        }

        for (int i = 0; i < Rocket.GetInstance().GetStrikesLeft(); i++)
        {
            strike = strikesLeftArray[i];
            strike.gameObject.SetActive(true);
        }
    }

    public void ShowPowerUpMessage(string powerUpName)
    {
        switch (powerUpName)
        {
            case "FutureSightPowerUpPrefab(Clone)":
                powerUpText.text = "FUTURE SIGHT ENABLED";
                break;
            case "GravHelperPowerUpPrefab(Clone)":
                powerUpText.text = "GRAV HELPER ENABLED";
                break;
            case "PlusOnePowerUpPrefab(Clone)":
                powerUpText.text = "+1 STRIKE ADDED";
                break;
        }
        powerUpText.gameObject.SetActive(true);
        startCountdown = true;
    }

    public void HidePowerUpMessage()
    {
        powerUpText.gameObject.SetActive(false);
    }
}
