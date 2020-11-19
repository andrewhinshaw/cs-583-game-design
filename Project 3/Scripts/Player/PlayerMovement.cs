using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private GameObject playerArm;
    private Animation playerArmAnim;

    public CharacterController controller;
    public Camera mainCamera;
    public static PlayerMovement instance;

    public static PlayerMovement GetInstance()
    {
        return instance;
    }

    // PLAYER PARAMETERS
    [Range(5f, 25f)]
    public float speed = 12f;
    [Range(-30f, 0f)]
    public float gravity = -10f;
    [Range(0f, 30f)]
    public float jumpHeight = 3f;
    [Range(1, 5)]
    public int maxJumps = 3;
    public int jumpsLeft;

    private Vector3 spawnCoords = new Vector3(44.5f, 2.88f, -50.0f);

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public LayerMask platformMask;

    Vector3 velocity;
    bool isGrounded;
    bool isFrozen;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        playerArm = GameObject.Find("PlayerArm");
        playerArmAnim = playerArm.GetComponent<Animation>();
    }

    void Update()
    {
        if (isFrozen)
        {
            controller.enabled = false;
        }

        else
        {
            /* ===== PLAYER MOVEMENT ===== */
            // Check if the player is grounded
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask) ||
                         Physics.CheckSphere(groundCheck.position, groundDistance, platformMask);
            // if grounded, reset vertical velocity from gravity
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
                jumpsLeft = maxJumps;
            }

            float x = Input.GetAxis("Horizontal");  // A/D or Left/Right Arrow Keys
            float z = Input.GetAxis("Vertical");    // W/s or Up/Down Arrow Keys

            Vector3 move;

            if (Input.GetButton("Fire3"))           // Left Shift
            {
                move = 1.3f * (transform.right * x + transform.forward * z);
            }
            else
            {
                move = transform.right * x + transform.forward * z;
            }

            controller.Move(move * speed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && jumpsLeft > 0)
            {
                AudioManager.instance.Play("Jump");
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                jumpsLeft--;
            }

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);


            /* ===== PLAYER ACTIONS ===== */

            // Left Mouse Button - interact with sphere
            if (Input.GetMouseButton(0))
            {
                playerArmAnim["PlayerArm"].wrapMode = WrapMode.Once;
                playerArmAnim.Play("PlayerArm");

                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100f))
                {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
                    if (interactable != null)
                    {
                        interactable.Interact();
                    }
                }
            }

            // Right Mouse Button - mark target sphere with flag
            if (Input.GetMouseButton(1))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1000f))
                {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
                    if (interactable != null)
                    {
                        interactable.Mark();
                    }
                }
            }

            // Q (ONLY FOR TESTING) - reveals/hides all values
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (GridData.GetInstance().spritesHidden)
                {
                    GridData.GetInstance().ShowAllSprites();
                }
                else if (!GridData.GetInstance().spritesHidden)
                {
                    GridData.GetInstance().HideAllSprites();
                }
            }

            // L (ONLY FOR TESTING) - triggers level complete
            if (Input.GetKeyDown(KeyCode.L))
            {
                PlayerState.Instance.LevelComplete();
            }

            // E - use drone disable button and level complete portal
            if (Input.GetKeyDown(KeyCode.E))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1000f))
                {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
                    if (interactable != null && hit.collider.gameObject.tag == "DroneButton")
                    {
                        interactable.PressDroneDisableButton();
                    }

                    if (interactable != null && hit.collider.gameObject.tag == "Portal")
                    {
                        interactable.InteractWithPortal();
                    }
                }
            }

            if (transform.position.y < -75)
            {
                ResetPositionToSpawn();
            }
        }

    }

    public void ResetPositionToSpawn()
    {
        transform.position = spawnCoords;
        isFrozen = false;
    }

    public void FreezePlayer()
    {
        isFrozen = true;
    }

    public void UnfreezePlayer()
    {
        isFrozen = false;
    }
}
