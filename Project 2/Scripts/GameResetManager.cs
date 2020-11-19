using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// If the player restarts the game this script is invoked destroying
// the Rocket singleton instance so a new one can be created
public class GameResetManager : MonoBehaviour
{
    public static void DestroyPlayer()
    {
        Destroy(GameObject.Find("Rocket"));
        Destroy(GameObject.Find("DifficultyManager"));
    }
}
