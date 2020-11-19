using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraMotion : MonoBehaviour
{
    private const float CAMERA_SPEED = 1f;
    private const float X_BOUNDARY = 200f;
    private const float Y_BOUNDARY = 200f;

    Transform cameraTransform;

    private float xDirection = 1;
    private float yDirection = 1;

    void Awake()
    {
        cameraTransform = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    private void Update()
    {
        SetXDirection();
        SetYDirection();

        cameraTransform.position += new Vector3((1 * xDirection), (2 * yDirection), 0) * CAMERA_SPEED * Time.deltaTime;
    }

    private float GetXPosition()
    {
        return cameraTransform.position.x;
    }

    private void SetXDirection()
    {
        if (GetXPosition() >= X_BOUNDARY)
        {
            xDirection = -1;
        }
        if (GetXPosition() <= -1 * X_BOUNDARY)
        {
            xDirection = 1;
        }
    }

    private float GetYPosition()
    {
        return cameraTransform.position.y;
    }

    private void SetYDirection()
    {
        if (GetYPosition() >= Y_BOUNDARY)
        {
            yDirection = -1;
        }
        if (GetYPosition() <= -1 * Y_BOUNDARY)
        {
            yDirection = 1;
        }
    }
}
