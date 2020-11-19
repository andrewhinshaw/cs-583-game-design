using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DroneController : MonoBehaviour
{
    public Transform target;
    public Camera droneCamera;
    public static DroneController instance;
    private string currentScene;

    [Range(1f, 20f)]
    public float speed;

    private Vector3 spawnCoords = new Vector3(-71.2f, 32.26f, -47.8f);
    private Vector3 tutorialSpawnCoords = new Vector3(-64f, 10f, -35f);

    [Range(1f, 30f)]
    public float cooldown = 30f;
    public float interactCooldown;
    public bool interactReady = false;
    public bool isDisabled = false;
    public float disableCooldown;
    private int directionModifier = 1;

    const float BUFFER = 1f;

    int layerMask = ~(1 << 10);
    int platformMask = ~(1 << 11);

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;

        interactCooldown = cooldown;
        disableCooldown = cooldown;
        interactReady = true;
        isDisabled = false;

        target = GameObject.Find("Player").GetComponent<Transform>();

        if (currentScene == "Tutorial")
        {
            transform.position = tutorialSpawnCoords;
        }

        speed = GridData.GetInstance().gridEntryArray.Length * .20f;
    }

    void Update()
    {
        if (!interactReady)
        {
            interactCooldown -= 1 * Time.deltaTime;
        }
        if (interactCooldown <= 0)
        {
            interactReady = true;
        }

        if (isDisabled)
        {
            disableCooldown -= 1 * Time.deltaTime;
        }
        if (disableCooldown <= 0)
        {
            isDisabled = false;
        }

        if (currentScene == "Tutorial")
        {
            if (transform.position.x > -44.0f)
            {
                directionModifier = -1;
                transform.Rotate(directionModifier * 180.0f, 0.0f, 0.0f, Space.Self);
            }
            if (transform.position.x < -64.0f)
            {
                directionModifier = 1;
                transform.Rotate(directionModifier * 180.0f, 0.0f, 0.0f, Space.Self);
            }

            transform.position += new Vector3(directionModifier * speed * Time.deltaTime, 0.0f, 0.0f);

        }
        else
        {
            if (!isDisabled)
            {
                transform.LookAt(target.position);

                Ray ray = droneCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100f, layerMask) ||
                    Physics.Raycast(ray, out hit, 100f, platformMask))
                {
                    // Debug.Log(hit.collider.gameObject.ToString());
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        if ((transform.position - target.position).magnitude > BUFFER)
                        {
                            transform.Translate(0.0f, 0.0f, speed * Time.deltaTime);
                        }
                    }
                }
            }
        }
    }

    public void DroneDisable(bool levelComplete)
    {
        if (!isDisabled && !levelComplete)
        {
            isDisabled = true;
            disableCooldown = cooldown;
        }

        // disable the drone indefinitely (end of level)
        else if (levelComplete)
        {
            isDisabled = true;
            disableCooldown = 999999;
        }
    }

    public void ResetPositionToSpawn()
    {
        transform.position = spawnCoords;
    }
}
