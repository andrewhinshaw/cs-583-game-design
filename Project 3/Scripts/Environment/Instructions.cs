using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour
{
    private Camera mainCamera;
    private SpriteRenderer sRend;
    public static Instructions instance;
    Quaternion originalRotation;

    public Instructions GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        sRend = GetComponent<SpriteRenderer>();
        originalRotation = transform.rotation;
    }

    void LateUpdate()
    {
        // update the rotation of this game object to point at the camera
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                        mainCamera.transform.rotation * Vector3.up);

    }
}
