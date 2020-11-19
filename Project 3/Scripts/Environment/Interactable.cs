using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour
{
    private GameObject droneButton;
    private Animation droneButtonAnim;

    public Transform player;
    private Renderer rend;
    private string currentScene;

    [Range(0f, 10f)]
    public float radius = 5f;

    public bool hasInteracted = false;
    public bool hasBeenMarked = false;

    private void Awake()
    {
        player = GameObject.Find("Interactor").GetComponent<Transform>();
        rend = GetComponent<Renderer>();
    }

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;

        droneButton = GameObject.Find("DroneDisableButton");
        droneButtonAnim = droneButton.GetComponent<Animation>();
    }

    private void Update()
    {

    }

    public void Interact()
    {
        if (!hasInteracted)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance <= radius)
            {
                AudioManager.instance.Play("InteractWithSphere");
                Debug.Log("Interact by Player!");
                hasInteracted = true;
                GridEntry.Value value = GridData.GetInstance().RevealSelectedSprite(gameObject.name, false);

                if (value == GridEntry.Value.Mine)
                {
                    AudioManager.instance.Play("MineExplosion");
                    rend.material.SetColor("_EmissionColor", Color.red);
                    PlayerState.Instance.TriggerDead();
                }
            }
        }
    }

    public void Mark()
    {
        if (!hasInteracted && !hasBeenMarked)
        {
            Debug.Log("Marked with flag!");
            hasBeenMarked = true;
            GridData.GetInstance().MarkSelectedSprite(gameObject.name);
        }
        if (hasBeenMarked)
        {
            Debug.Log("Unmarked!");
            hasBeenMarked = true;
            GridData.GetInstance().MarkSelectedSprite(gameObject.name);
        }
    }

    public void PressDroneDisableButton()
    {
        AudioManager.instance.Play("InteractWithSphere");

        droneButtonAnim["DroneDisableButtonPress"].wrapMode = WrapMode.Once;
        droneButtonAnim.Play("DroneDisableButtonPress");

        DroneController.instance.DroneDisable(false);
    }

    public void InteractWithPortal()
    {
        AudioManager.instance.Play("InteractWithPortal");

        PlayerMovement.GetInstance().FreezePlayer();

        Debug.Log("Loading: " + currentScene);
        if (currentScene == "Tutorial")
        {
            // LOAD LEVEL ONE
            Loader.Load(Loader.Scene.LevelOne);
            PlayerState.Instance.StartTimer();
        }
        else if (currentScene == "LevelOne")
        {
            // LOAD LEVEL TWO
            Loader.Load(Loader.Scene.LevelTwo);
        }
        else if (currentScene == "LevelTwo")
        {
            // LOAD LEVEL THREE
            Loader.Load(Loader.Scene.LevelThree);
        }
        else if (currentScene == "LevelThree")
        {
            // // GAME COMPLETE
            PlayerState.Instance.GameComplete();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        AudioManager.instance.Play("InteractWithSphere");

        Debug.Log("Collision detected!");
        if (collider.gameObject.tag == "Drone" && DroneController.instance.interactReady)
        {
            Debug.Log("Interact by Drone!");
            hasInteracted = true;
            GridEntry.Value value = GridData.GetInstance().RevealSelectedSprite(gameObject.name, true);
            rend.material.SetColor("_EmissionColor", Color.blue);
            DroneController.instance.interactReady = false;
            DroneController.instance.interactCooldown = DroneController.instance.cooldown;
        }
    }

    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
