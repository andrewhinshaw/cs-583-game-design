using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public enum Scene
    {
        SplashScreen,
        Loading,
        Credits,
        MainMenu,
        Tutorial,
        LevelOne,
        LevelTwo,
        LevelThree,
    }

    private static Scene targetScene;

    public static void Load(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

    public static string GetCurrentScene()
    {
        return SceneManager.GetActiveScene().name;
    }
}
