using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCreditsWindow : MonoBehaviour
{
    public void Back()
    {
        SoundsManager.PlaySound(SoundsManager.Sound.ButtonClick);
        Loader.Load(Loader.Scene.MainMenu);
    }
}
