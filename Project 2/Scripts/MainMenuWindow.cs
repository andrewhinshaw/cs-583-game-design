using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuWindow : MonoBehaviour
{
    private Animation mainMenuAnim;
    private Animation helpMenuAnim;

    private GameObject mainMenuFrame;
    private GameObject helpWindowFrame;
    private GameObject mainInnerFrame;
    private GameObject helpInnerFrame;
    private GameObject pointImageHolder;
    private GameObject powerUpImageHolder;
    private GameObject plusSymbol;
    private GameObject minusSymbol;

    private GameObject[] buttonList;
    private GameObject[] panelContentsList;
    // private GameObject[] middlePanelImageList;

    private Text middlePanelText;

    private bool mainMenuIsOpen;
    private bool helpWindowIsOpen;
    private bool moreIsOpen = false;

    public void Start()
    {
        mainMenuFrame = GameObject.Find("MainMenuFrame");
        helpWindowFrame = GameObject.Find("HelpWindowFrame");
        mainInnerFrame = GameObject.Find("MainInnerFrame");
        helpInnerFrame = GameObject.Find("HelpInnerFrame");
        buttonList = GameObject.FindGameObjectsWithTag("MainButtons");
        panelContentsList = GameObject.FindGameObjectsWithTag("PanelContents");
        pointImageHolder = GameObject.Find("PointsHolder");
        powerUpImageHolder = GameObject.Find("PowerUpsHolder");
        plusSymbol = GameObject.Find("Plus");
        minusSymbol = GameObject.Find("Minus");
        middlePanelText = GameObject.Find("MiddlePanelText").GetComponent<Text>()   ;

        mainMenuAnim = mainInnerFrame.GetComponent<Animation>();
        helpMenuAnim = helpInnerFrame.GetComponent<Animation>();

        minusSymbol.SetActive(false);
        powerUpImageHolder.SetActive(false);
        middlePanelText.text = pointsText;

        Rocket.GetInstance().HideRocket();
    }

    private void Update()
    {

    }

    public void Play()
    {
        SoundsManager.PlaySound(SoundsManager.Sound.ButtonClick);
        Loader.Load(Loader.Scene.GameScene);
    }

    public void Quit()
    {
        SoundsManager.PlaySound(SoundsManager.Sound.ButtonClick);
        // if viewing game in unity editor, exit playing
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        // else game is executable, quit game
        #else
        Application.Quit();
        #endif
    }

    public void Help()
    {
        SoundsManager.PlaySound(SoundsManager.Sound.ButtonClick);
        mainMenuAnim["MainOut"].wrapMode = WrapMode.Once;
        helpMenuAnim["HelpIn"].wrapMode = WrapMode.Once;
        mainMenuAnim.Play("MainOut");
        helpMenuAnim.Play("HelpIn");
    }

    public void Back()
    {
        SoundsManager.PlaySound(SoundsManager.Sound.ButtonClick);
        mainMenuAnim["MainIn"].wrapMode = WrapMode.Once;
        helpMenuAnim["HelpOut"].wrapMode = WrapMode.Once;
        mainMenuAnim.Play("MainIn");
        helpMenuAnim.Play("HelpOut");
    }

    public void Credits()
    {
        SoundsManager.PlaySound(SoundsManager.Sound.ButtonClick);
        Loader.Load(Loader.Scene.CreditsScene);
    }

    public void ShowHideMore()
    {
        if (!moreIsOpen)
        {
            plusSymbol.SetActive(false);
            pointImageHolder.SetActive(false);
            minusSymbol.SetActive(true);
            powerUpImageHolder.SetActive(true);
            middlePanelText.text = powerUpsText;
            moreIsOpen = true;
        }
        else if (moreIsOpen)
        {
            middlePanelText.text = pointsText;
            minusSymbol.SetActive(false);
            powerUpImageHolder.SetActive(false);
            plusSymbol.SetActive(true);
            pointImageHolder.SetActive(true);
            moreIsOpen = false;
        }
    }

    private string pointsText = "Future stock prices appear as white points.\n\n" +
"Points hit will turn green, indicating a correct prediction.\n\n" +
"Points missed will turn red, indicating an incorrect prediction.\n\n" +
"Too many missed predictions will result in your funding being pulled, game over.\n\n" +
"Hearts indicate how many more strikes your investors have given you.\n\n";

    private string powerUpsText = "Your investors love to reward good performance " +
"and will give you temporary upgrades if you earn them.\n\n" +
"Hit a certain amount of correct predictions in the first half of the day to " +
"unlock temporary upgrades for the remainder of the day.\n\n" +
"Future Sight: The investors have allocated more hardware resources to you, " +
"giving you a greater prediction advantage.\n\n" +
"The Un-Inluencer: The investors have removed all restrictions from your code, " +
"freeing you from any outside influence.\n\n" +
"Plus One Strike: The investors are willing to forgive a past mistake, giving you a strike back.\n\n";
}
