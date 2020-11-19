using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleHandler : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown)
        {
            Loader.Load(Loader.Scene.MainMenu);
        }
    }
}
