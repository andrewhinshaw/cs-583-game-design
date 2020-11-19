using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        ScoreOverlay.GetInstance().ShowPowerUpMessage(gameObject.name);
        Rocket.GetInstance().EnablePowerUp(gameObject.name);
        Destroy(gameObject);
    }
}
