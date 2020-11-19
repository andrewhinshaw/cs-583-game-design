using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelector : MonoBehaviour
{
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;
    public Button expertButton;

    private void Start()
    {
        easyButton = GameObject.Find("EasyButton").GetComponent<Button>();
        mediumButton = GameObject.Find("MediumButton").GetComponent<Button>();
        hardButton = GameObject.Find("HardButton").GetComponent<Button>();
        expertButton = GameObject.Find("ExpertButton").GetComponent<Button>();

        ButtonSwitcher(0);
    }
    public void SetEasy()
    {
        DifficultyManager.GetInstance().SetPlayerDifficulty(DifficultyManager.Difficulty.Easy);
        ButtonSwitcher(0);
    }

    public void SetMedium()
    {
        DifficultyManager.GetInstance().SetPlayerDifficulty(DifficultyManager.Difficulty.Medium);
        ButtonSwitcher(1);
    }

    public void SetHard()
    {
        DifficultyManager.GetInstance().SetPlayerDifficulty(DifficultyManager.Difficulty.Hard);
        ButtonSwitcher(2);
    }

    public void SetExpert()
    {
        DifficultyManager.GetInstance().SetPlayerDifficulty(DifficultyManager.Difficulty.Expert);
        ButtonSwitcher(3);
    }

    public void ButtonSwitcher(int selector)
    {
        easyButton.interactable = true;
        mediumButton.interactable = true;
        hardButton.interactable = true;
        expertButton.interactable = true;

        switch(selector)
        {
            case 0:
                easyButton.interactable = false;
                break;
            case 1:
                mediumButton.interactable = false;
                break;
            case 2:
                hardButton.interactable = false;
                break;
            case 3:
                expertButton.interactable = false;
                break;
        }
    }
}
