using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{
    private Transform gameOverWindowTransform;
    private Text scoreText;
    private Text highscoreText;

    private static GameOverWindow instance;

    public static GameOverWindow GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
        scoreText = transform.Find("ScoreText").GetComponent<Text>();
        highscoreText = transform.Find("HighScoreText").GetComponent<Text>();
        gameOverWindowTransform = GetComponent<Transform>();
    }

    private void Start()
    {
        HideWindow();
    }

    public void RestartGame()
    {
        SoundsManager.PlaySound(SoundsManager.Sound.ButtonClick);
        GameResetManager.DestroyPlayer();
        Loader.Load(Loader.Scene.GameScene);
    }

    public void BackToMainMenu()
    {
        SoundsManager.PlaySound(SoundsManager.Sound.ButtonClick);
        GameResetManager.DestroyPlayer();
        Loader.Load(Loader.Scene.MainMenu);
    }

    public void RocketDied()
    {
        scoreText.text = Rocket.GetInstance().GetTotalScore().ToString();

        if (Rocket.GetInstance().GetTotalScore() >= Score.GetHighScore())
        {
            highscoreText.text = "NEW HIGHSCORE!";
        }
        else
        {
            highscoreText.text = "HIGHSCORE: " + Score.GetHighScore().ToString();
        }
        ShowWindow();
    }

    private void ShowWindow()
    {
        gameObject.SetActive(true);
        gameOverWindowTransform.localPosition = new Vector3(0, 0, 0);
    }

    private void HideWindow()
    {
        gameObject.SetActive(false);
    }
}
