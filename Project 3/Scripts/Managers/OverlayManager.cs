using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayManager : MonoBehaviour
{
    public static OverlayManager Instance;

    public static bool isPaused = false;
    public GameObject pauseMenuUI;

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        MouseLook.instance.LockMouse();
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        MouseLook.instance.UnlockMouse();
    }

    public void LoadMenu()
    {
        AudioManager.instance.Play("ButtonClick");
        Loader.Load(Loader.Scene.MainMenu);
        gameObject.SetActive(false);
    }

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
