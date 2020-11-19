using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutInstructionsManager : MonoBehaviour
{
    public GameObject window;

    void Start()
    {
        OpenWindow();
    }

    public void OpenWindow()
    {
        window.SetActive(true);
        PlayerMovement.GetInstance().FreezePlayer();
        MouseLook.instance.UnlockMouse();
    }

    public void CloseWindow()
    {
        AudioManager.instance.Play("ButtonClick");
        window.SetActive(false);
        PlayerMovement.GetInstance().UnfreezePlayer();
        MouseLook.instance.LockMouse();
    }
}
