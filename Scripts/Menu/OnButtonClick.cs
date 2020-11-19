using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnButtonClick : MonoBehaviour
{

    // Load tutorial level scene
    public void Play()
    {
        AudioManager.instance.Play("ButtonClick");
        Loader.Load(Loader.Scene.Tutorial);
    }

    // Load credits scene
    public void Credits()
    {
        AudioManager.instance.Play("ButtonClick");
        Loader.Load(Loader.Scene.Credits);
    }

    // Load main menu scene
    public void MainMenu()
    {
        AudioManager.instance.Play("ButtonClick");
        Loader.Load(Loader.Scene.MainMenu);
    }

    // Quit game
    public void Quit()
    {
        AudioManager.instance.Play("ButtonClick");

        // if in unity editor, stop playing
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

        // else game is being run in executable
        #else
        Application.Quit();

        #endif
    }
}
