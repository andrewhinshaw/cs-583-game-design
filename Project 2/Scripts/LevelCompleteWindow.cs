using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteWindow : MonoBehaviour
{
    private Transform levelCompleteWindowTransform;
    private Text dayScoreText;
    private Text totalScoreText;

    private static LevelCompleteWindow instance;

    public static LevelCompleteWindow GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
        dayScoreText = transform.Find("DayScoreText").GetComponent<Text>();
        totalScoreText = transform.Find("TotalScoreText").GetComponent<Text>();
        levelCompleteWindowTransform = GetComponent<Transform>();
    }

    private void Start()
    {
        HideWindow();
    }

    public void NextLevel()
    {
        Rocket.GetInstance().IncrementCurrentDay();
        SoundsManager.PlaySound(SoundsManager.Sound.ButtonClick);
        Rocket.GetInstance().ResetPosition();
        Loader.Load(Loader.Scene.GameScene);
    }

    public void BackToMainMenu()
    {
        SoundsManager.PlaySound(SoundsManager.Sound.ButtonClick);
        GameResetManager.DestroyPlayer();
        Loader.Load(Loader.Scene.MainMenu);
    }

    public void LevelComplete()
    {
        totalScoreText.text = "TOTAL SCORE: " + Rocket.GetInstance().GetTotalScore();

        dayScoreText.text = "TODAY: " + Rocket.GetInstance().GetCurrentScore().ToString();
        dayScoreText.text += "/" + Level.GetInstance().GetTotalPoints().ToString();
        
        ShowWindow();
    }

    private void ShowWindow()
    {
        gameObject.SetActive(true);
        levelCompleteWindowTransform.localPosition = new Vector3(0, 0, 0);
    }

    private void HideWindow()
    {
        gameObject.SetActive(false);
    }
}
