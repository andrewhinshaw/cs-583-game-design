using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Score
{
    public static void Start()
    {
        // Rocket.GetInstance().OnDied += Rocket_OnDied;
    }

    // private static void Rocket_OnDied(object sender, System.EventArgs e)
    // {
    //     SetNewHighScore(Rocket.GetInstance().GetCurrentScore());
    // }

    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt("highscore");
    }

    public static bool SetNewHighScore(int score)
    {
        int curentHighsScore = GetHighScore();
        if (score > curentHighsScore)
        {
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void ResetHighScore()
    {
        PlayerPrefs.SetInt("highscore", 0);
        PlayerPrefs.Save();
    }
}
