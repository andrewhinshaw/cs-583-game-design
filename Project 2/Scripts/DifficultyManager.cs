using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyManager : MonoBehaviour
{
    private float difficultyModifier;

    public Difficulty currentDifficulty;

    private static DifficultyManager Instance;

    public static DifficultyManager GetInstance()
    {
        return Instance;
    }

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Expert,
    }

    private void Awake()
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
        SetPlayerDifficulty(Difficulty.Easy);
    }

    public float GetDifficultyModifier()
    {
        return difficultyModifier;
    }

    public Difficulty GetCurrentDifficulty()
    {
        return currentDifficulty;
    }

    // Sets the player's attributes depending on the difficulty selected
    public void SetPlayerDifficulty(Difficulty difficulty)
    {
        switch(difficulty)
        {
            case Difficulty.Easy:
                Rocket.GetInstance().SetStrikesMax(7);
                difficultyModifier = 10f;
                break;
            case Difficulty.Medium:
                Rocket.GetInstance().SetStrikesMax(5);
                difficultyModifier = 20f;
                break;
            case Difficulty.Hard:
                Rocket.GetInstance().SetStrikesMax(3);
                difficultyModifier = 30f;
                break;
            case Difficulty.Expert:
                Rocket.GetInstance().SetStrikesMax(1);
                difficultyModifier = 40f;
                break;
        }
        currentDifficulty = difficulty;
        Rocket.GetInstance().SetStrikesLeft();
    }

    // Sets the level attributes based on the current player difficulty
    public void SetLevelDifficulty()// Difficulty difficulty)
    {
        float gapMax = (float)((Rocket.GetInstance().GetCurrentDay() * GetDifficultyModifier()) + 0);
        if (gapMax > 45)
        {
            gapMax = 45;
        }
        Level.GetInstance().SetPointGapRange(gapMax);
    }
}
