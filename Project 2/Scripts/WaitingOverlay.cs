using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingOverlay : MonoBehaviour
{
    private float currentTime = 0f;
    private float startingTime = 3.0f;
    private bool startCountdown;

    public Text waitingText;
    public Text countdownText;

    private static WaitingOverlay instance;

    public static WaitingOverlay GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        startCountdown = false;
        currentTime = startingTime;
    }

    private void Update()
    {
        if (startCountdown == true)
        {
            currentTime -= 1 * Time.deltaTime;
        }

        if (Level.GetInstance().state == Level.State.Waiting)
        {
            if (currentTime <= 3.0f && currentTime > 2.0f)
            {
                countdownText.text = "3";
            }
            else if (currentTime <= 2.0f && currentTime > 1.0f)
            {
                countdownText.text = "2";
            }
            else if (currentTime <= 1.0f && currentTime > 0.0f)
            {
                countdownText.text = "1";
            }
            else if (currentTime <= 0.0f && currentTime > -1.0f)
            {
                countdownText.text = "BEGIN";
                Level.GetInstance().state = Level.State.Playing;
            }
        }
        if (Level.GetInstance().state == Level.State.Playing)
        {
            if (currentTime < -1)
            {
                HideCountdownText();
            }
        }
    }

    public void ShowWaitingText()
    {
        waitingText.gameObject.SetActive(true);
    }

    public void HideWaitingText()
    {
        waitingText.gameObject.SetActive(false);
    }

    public void ShowCountdownText()
    {
        countdownText.gameObject.SetActive(true);
        startCountdown = true;
    }

    public void HideCountdownText()
    {
        countdownText.gameObject.SetActive(false);
    }
}
