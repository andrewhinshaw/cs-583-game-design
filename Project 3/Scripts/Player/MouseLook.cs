using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // MOUSE SETTINGS
    [Range(10f, 1000f)]
    public float mouseSensitivity = 500f;
    public bool isLocked;

    public Transform playerBody;

    private float xRotation = 0f;

    public static MouseLook instance;

    void Awake()
    {
        // LockMouse();
        instance = this;
    }

    void Start()
    {
        if (Loader.GetCurrentScene() != "Tutorial")
        {
            LockMouse();
        }
    }

    void Update()
    {
        if (isLocked)
        {
            // FIRST PERSON
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -45f, 60f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        isLocked = true;
    }

    public void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        isLocked = false;
    }
}
