using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // PHYSICS VARIABLES
    private Rigidbody2D rocketRigidbody2D;
    private Quaternion downRotation;
    private Quaternion upRotation;
    private Quaternion noRotation;
    private Quaternion gravityRotation;
    public float tiltSmooth = 100;
    public Animator animator;
    private const float upAmount = 300f;
    private const float downAmount = 300f;
    private int gravityModifier = 1;

    // SCORE AND LOGISTICS VARIABLES
    private int currentScore;
    private int totalScore;
    private int currentDayCount;
    public int strikesLeft;
    public int strikesLeftMax;
    private int totalMissed;
    private bool inMenu;

    // ROCKET INSTANCE
    private static Rocket Instance;

    public static Rocket GetInstance()
    {
        return Instance;
    }

    private void Awake()
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

        rocketRigidbody2D = GetComponent<Rigidbody2D>();
        currentDayCount = 1;
    }

    private void Start()
    {
        inMenu = false;
        rocketRigidbody2D.bodyType = RigidbodyType2D.Static;
        strikesLeft = strikesLeftMax;
        noRotation = Quaternion.Euler(0, 0, 0);
        upRotation = Quaternion.Euler(0, 0, 45);
        downRotation = Quaternion.Euler(0, 0, -45);
    }

    private void Update()
    {
        if (!inMenu)
        {
            if (Level.GetInstance().state == Level.State.Waiting)
            {
                currentScore = 0;
                rocketRigidbody2D.bodyType = RigidbodyType2D.Static;
                if (Input.anyKeyDown)
                {
                    WaitingOverlay.GetInstance().HideWaitingText();
                    WaitingOverlay.GetInstance().ShowCountdownText();
                }
            }
            else if (Level.GetInstance().state == Level.State.Playing)
            {
                rocketRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                if (Input.GetKey(KeyCode.UpArrow) | Input.GetKey(KeyCode.W))
                {
                    Up();
                }
                else if (Input.GetKey(KeyCode.DownArrow) | Input.GetKey(KeyCode.S))
                {
                    Down();
                }
                else
                {
                    Stabilize();
                }

                if (strikesLeft == 0)
                {
                    GameOver();
                }
            }
        }
    }


    /* ========== MOVEMENT FUNCTIONS ========== */

    // Move the player upwards and play moving animation
    private void Up()
    {
        rocketRigidbody2D.AddForce(Vector2.up * upAmount);
        transform.rotation = Quaternion.Lerp(transform.rotation, upRotation, tiltSmooth * Time.deltaTime);
        animator.SetFloat("Speed", 1f);
        animator.SetBool("isMoving", true);
    }

    // Move the player downwards and play moving animation
    private void Down()
    {
        rocketRigidbody2D.AddForce(Vector2.down * downAmount);
        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
        animator.SetFloat("Speed", 1f);
        animator.SetBool("isMoving", true);
    }

    // Default to stable state when not moving
    private void Stabilize()
    {
        gravityRotation = Quaternion.Euler(0, 0, -1 * rocketRigidbody2D.gravityScale);
        transform.rotation = Quaternion.Lerp(transform.rotation, gravityRotation, tiltSmooth * Time.deltaTime);
        animator.SetFloat("Speed", 0f);
        animator.SetBool("isMoving", false);
    }

    // Change the gravity to pull player upwards
    public void GravityUp()
    {
        if (rocketRigidbody2D.gravityScale >= 0)
        {
            rocketRigidbody2D.gravityScale = -20 * gravityModifier;
        }
    }

    // Change the gravity to pull the player downwards
    public void GravityDown()
    {
        if (rocketRigidbody2D.gravityScale <= 0)
        {
            rocketRigidbody2D.gravityScale = 20 * gravityModifier;
        }
    }


    /* ========== ROCKET HELPER FUNCTIONS ========== */

    public void IncrementScore()
    {
        currentScore++;
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public int GetTotalScore()
    {
        return totalScore;
    }

    public void IncrementTotalMissed()
    {
        totalMissed++;
    }

    public int GetTotalMissed()
    {
        return totalMissed;
    }

    public int GetStrikesLeft()
    {
        return strikesLeft;
    }

    public void SetStrikesLeft()
    {
        strikesLeft = strikesLeftMax;
    }

    public int GetStrikesMax()
    {
        return strikesLeftMax;
    }

    public void SetStrikesMax(int strikes)
    {
        strikesLeftMax = strikes;
    }

    public int GetCurrentDay()
    {
        return currentDayCount;
    }

    public void IncrementCurrentDay()
    {
        currentDayCount++;
    }

    public Rigidbody2D GetRididBody()
    {
        return rocketRigidbody2D;
    }

    public void DecrementStrikesLeft()
    {
        strikesLeft--;
    }

    public void ShowRocket()
    {
        gameObject.SetActive(true);
    }

    public void HideRocket()
    {
        gameObject.SetActive(false);
    }

    public void ResetPosition()
    {
        transform.position = new Vector3(0, 0, 0);
        transform.rotation = noRotation;
    }

    public void EnablePowerUp(string powerUpName)
    {
        Debug.Log("Entering switch for name " + powerUpName + "!");
        switch (powerUpName)
        {
            case "FutureSightPowerUpPrefab(Clone)":
                Debug.Log("Collected future sight power up!");
                Level.GetInstance().SetFutureSightOn();
                break;
            case "GravHelperPowerUpPrefab(Clone)":
                Debug.Log("Collected grav helper power up!");
                gravityModifier = 0;
                break;
            case "PlusOnePowerUpPrefab(Clone)":
                Debug.Log("Collected plus one power up!");
                strikesLeft++;
                break;
        }
    }

    public void DisableAllPowerUps()
    {
        // destroy future sight helper
        Level.GetInstance().DestroyFutureSightMarker();
        Level.GetInstance().SetFutureSightOff();
        // reset gravity to normal
        gravityModifier = 1;
    }


    /* ========== EVENT FUNCTIONS ========== */

    // Triggers on game over
    public void GameOver()
    {
        // Make player static
        rocketRigidbody2D = GetComponent<Rigidbody2D>();
        rocketRigidbody2D.bodyType = RigidbodyType2D.Static;

        // Show game over window
        GameOverWindow.GetInstance().RocketDied();

        // Set the level state to game over
        Level.GetInstance().SetState(Level.State.GameOver);

        // Set new high score
        Score.SetNewHighScore(totalScore);
    }

    // Triggers on level complete
    public void LevelComplete()
    {
        // disable power ups
        DisableAllPowerUps();

        // make player static
        rocketRigidbody2D = GetComponent<Rigidbody2D>();
        rocketRigidbody2D.bodyType = RigidbodyType2D.Static;

        // add to total score
        totalScore += currentScore;

        // every third day complete, add one left - only if lives left < max
        if (currentDayCount % 3 == 0 && strikesLeft < strikesLeftMax)
        {
            strikesLeft++;
        }

        // set level state to level complete and show level complete window
        LevelCompleteWindow.GetInstance().LevelComplete();
        Level.GetInstance().SetState(Level.State.LevelComplete);
    }
}
