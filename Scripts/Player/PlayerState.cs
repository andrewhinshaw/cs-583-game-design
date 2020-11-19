using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance;
    private string currentScene;

    private GameObject levelCompletePortal;
    private Animation levelCompletePortalAnim;

    public Text timerText;
    private bool timerStarted;
    private bool timerFinished;
    private float startTime;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;

        if (Loader.GetCurrentScene() != "Tutorial" && !timerStarted)
        {
            StartTimer();
        }
    }

    void Update()
    {
        if (timerStarted && !timerFinished)
        {
            timerText = GameObject.Find("GameTimerText").GetComponent<Text>();

            float t = Time.time - startTime;

            string minutes = ((int) t / 60).ToString();
            string seconds = (t % 60).ToString("00");

            timerText.text = minutes + ":" + seconds;
        }
    }

    public void TriggerDead()
    {
        GridData.GetInstance().ResetGrid();

        PlayerMovement.instance.ResetPositionToSpawn();
        DroneController.instance.ResetPositionToSpawn();
    }

    public void LevelComplete()
    {
        levelCompletePortal = GameObject.Find("LevelCompletePortal");

        AudioManager.instance.Play("LevelComplete");

        DroneController.instance.DroneDisable(true);

        // levelCompletePortal.SetActive(true);
        //
        // levelCompletePortalAnim["PortalOpen"].wrapMode = WrapMode.Once;
        // levelCompletePortalAnim.Play("PortalOpen");
    }

    public void GameComplete()
    {
        StopTimer();
    }

    public void StartTimer()
    {
        timerStarted = true;
        startTime = Time.time;
    }

    public void StopTimer()
    {
        timerFinished = true;
    }
}
