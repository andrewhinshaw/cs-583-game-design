using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueSpriteHandler : MonoBehaviour
{
    private Camera mainCamera;
    private SpriteRenderer sRend;
    public static ValueSpriteHandler instance;
    Quaternion originalRotation;

    public ValueSpriteHandler GetInstance()
    {
        return instance;
    }

    public Sprite[] spriteArray;

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

    public void SetSprite(GridEntry.Value val)
    {
        switch (val)
        {
            case GridEntry.Value.Empty:
                sRend.sprite = spriteArray[0];
                break;
            case GridEntry.Value.Mine:
                sRend.sprite = spriteArray[10];
                break;
            case GridEntry.Value.Num_1:
                sRend.sprite = spriteArray[1];
                break;
            case GridEntry.Value.Num_2:
                sRend.sprite = spriteArray[2];
                break;
            case GridEntry.Value.Num_3:
                sRend.sprite = spriteArray[3];
                break;
            case GridEntry.Value.Num_4:
                sRend.sprite = spriteArray[4];
                break;
            case GridEntry.Value.Num_5:
                sRend.sprite = spriteArray[5];
                break;
            case GridEntry.Value.Num_6:
                sRend.sprite = spriteArray[6];
                break;
            case GridEntry.Value.Num_7:
                sRend.sprite = spriteArray[7];
                break;
            case GridEntry.Value.Num_8:
                sRend.sprite = spriteArray[8];
                break;
        }
    }
}
