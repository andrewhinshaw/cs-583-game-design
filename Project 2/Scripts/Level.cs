using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    // LEVEL CONSTANTS
    private const float POINT_SPEED = 30f;          // speed of points
    private const float GRID_SPEED = 10f;
    private const float DESTROY_X_VALUE = -200f;    // x pos where points destroyed
    private const float SPAWN_X_VALUE = 150f;       // x pos where points spawned
    private const float POWERUP_X_VALUE = SPAWN_X_VALUE - 30f;
    private const float ROCKET_X_VALUE = 0f;        // x pos of player (rocket)
    public const int POINTS_PER_DAY = 13;           

    // LEVEL INSTANCE
    private static Level instance;                  // instance for Level

    // LINE VARIABLES
    public GameObject lineGeneratorPrefab;          // game obj prefab for line generator
    private GameObject currentLine;                 // game obj for current line

    // POWERUP VARIABLES
    public Transform powerUpPrefab = null;
    public bool powerUpSpawned = false;
    private bool futureSightEnabled = false;
    public Transform futureSightMarker = null;
    private float futureSightMarkerSpawnTime;

    // POINT VARIABLES
    private List<Point> pointList;                  // list of all points
    private int totalPoints = 0;                    // total number of points
    private float pointSpawnTime;                   // current spawn time for points
    private float pointSpawnTimeMax;                // maximum spawn time for points
    private float pointGapMax;                      // max vertical distance between points
    private int consecutiveHit = 0;                 // num points hit in a row
    private bool moreRight = true;                  // number of missed points

    public State state;                             // current state of player

    // GRID VARIABLES
    public List<Transform> gridList;                // list of all current grids spawned
    private float gridSpawnTime;                    // grid spawn timer

    // Returns the current Level instance
    public static Level GetInstance()
    {
        return instance;
    }

    public enum State
    {
        Playing,
        GameOver,
        LevelComplete,
        Waiting,
    }

    private void Awake()
    {
        // initialize instance
        instance = this;

        // initialize spawn times
        pointSpawnTimeMax = 2f;
        futureSightMarkerSpawnTime = 0.1f;
    }

    private void Start()
    {
        state = State.Waiting;
        Rocket.GetInstance().ShowRocket();

        DifficultyManager.GetInstance().SetLevelDifficulty();
        WaitingOverlay.GetInstance().ShowWaitingText();
        WaitingOverlay.GetInstance().HideCountdownText();
        ScoreOverlay.GetInstance().UpdateStrikeOverlay();
        pointList = new List<Point>();
        gridList = new List<Transform>();

        Transform newGrid = Instantiate(GameAssets.GetInstance().gridBackgroundPrefab);
        newGrid.position = new Vector3(0f, 0f, 0f);
        gridList.Add(newGrid);
    }

    private void Update()
    {
        if (state == State.Playing)
        {
            // continue to spawn points until points per day reached
            if (totalPoints < POINTS_PER_DAY)
            {
                SpawnPoints();
            }
            // once points per day reached, check that all points are past Player
            // then trigger end of level
            else
            {
                if (!moreRight)
                {
                    Debug.Log("Level over");
                    Rocket.GetInstance().LevelComplete();
                }
            }

            // if player hits 7 consecutive points out of the first 10 points,
            // spawn a power up that lasts the rest of the day
            if (!powerUpSpawned && consecutiveHit == 5 && totalPoints <= 10)
            {
                SpawnPowerUp();
            }
            if (powerUpSpawned)
            {
                MovePowerUp();
            }

            // if player has future sight, spawn sight markers
            if (futureSightEnabled)
            {
                SpawnFutureSightMarker();
            }

            // move environment
            MovePoints();
            MoveLine();
            SpawnGrid();
            MoveGrid();
        }
        // if level is complete, continue moving environment but do not spawn more points
        if (state == State.LevelComplete)
        {
            MovePoints();
            MoveLine();
            SpawnGrid();
            MoveGrid();
        }
    }

    // Sets the current state of the player to the State enum value passed
    public void SetState(State newState)
    {
        state = newState;
    }

    // Returns the current State
    public State GetState()
    {
        return state;
    }


    /* ========== ENVIRONMENT FUNCTIONS ========== */

    // Creates a new line connecting all of the current points in the level that are left of player
    public void GenerateNewLine()
    {
        // Find and destroy old line before making new one
        GameObject oldLine = GameObject.FindWithTag("LineTag");
        Destroy(currentLine);

        // Find all points to connect
        GameObject[] allPoints = GameObject.FindGameObjectsWithTag("NextPointTag");
        Vector3[] allPointPositions = new Vector3[allPoints.Length];

        if (allPoints.Length >= 2)
        {
            int count = 0;
            for (int i = 0; i < allPoints.Length; i++)
            {
                // only connect points to the left of the player
                if (allPoints[i].transform.position.x <= ROCKET_X_VALUE)
                {
                    allPointPositions[count] = allPoints[i].transform.position;
                    count++;
                }
            }

            SpawnLineGenerator(allPointPositions);
        }
    }

    // Spawns a line generator to create the line
    private void SpawnLineGenerator(Vector3[] linePoints)
    {
        currentLine = Instantiate(lineGeneratorPrefab);
        LineRenderer lRend = currentLine.GetComponent<LineRenderer>();

        lRend.positionCount = linePoints.Length;

        // SetPosition(point_index, (x, y, z))
        lRend.SetPositions(linePoints);
    }

    private void MoveLine()
    {
        // Find current line
        // GameObject currentLine = GameObject.FindWithTag("LineTag");
        if (currentLine != null)
        {
            // move left, matching speed of points
            currentLine.GetComponent<Transform>().position += new Vector3(-1, 0, 0) * POINT_SPEED * Time.deltaTime;
        }
    }

    private void SpawnGrid()
    {
        if (gridList[gridList.Count - 1].position.x <= 0)
        {
            Transform newGrid = Instantiate(GameAssets.GetInstance().gridBackgroundPrefab);
            newGrid.position = new Vector3(200f, 0f, 0f);
            gridList.Add(newGrid);
        }
    }

    // Moves the background grid
    private void MoveGrid()
    {
        for (int i = 0; i < gridList.Count; i++)
        {
            if (gridList[i] != null)
            {
                gridList[i].position += new Vector3(-1, 0, 0) * GRID_SPEED * Time.deltaTime;

                // if grid reaches destroy point, destroy it
                if (gridList[i].position.x <= -200)
                {
                    Destroy(gridList[i].gameObject);
                }
            }
        }
    }


    /* ========== POWERUP FUNCTIONS ========== */

    // picks a random powerup and spawns it in the level
    private void SpawnPowerUp()
    {
        int numOptions;
        // only spawn +1 if player's lives are NOT FULL
        if (Rocket.GetInstance().GetStrikesLeft() < Rocket.GetInstance().GetStrikesMax())
        {
            numOptions = 3;
        }
        else
        {
            numOptions = 2;
        }
        int powerUpSelector = Random.Range(0, numOptions);
        float nextY = pointList[pointList.Count - 1].GetYPosition();
        if (powerUpSelector == 0)
        {
            Transform newPowerUp = Instantiate(GameAssets.GetInstance().futureSightPowerupPrefab);
            newPowerUp.position = new Vector3(POWERUP_X_VALUE, nextY, 0);
            powerUpPrefab = newPowerUp;
            Debug.Log("Spawning future sight powerup!");
        }
        else if (powerUpSelector == 1)
        {
            Transform newPowerUp = Instantiate(GameAssets.GetInstance().gravToolPowerupPrefab);
            newPowerUp.position = new Vector3(POWERUP_X_VALUE, nextY, 0);
            powerUpPrefab = newPowerUp;
            Debug.Log("Spawning grav tool powerup!");
        }
        else
        {
            Transform newPowerUp = Instantiate(GameAssets.GetInstance().plusOnePowerupPrefab);
            newPowerUp.position = new Vector3(POWERUP_X_VALUE, nextY, 0);
            powerUpPrefab = newPowerUp;
            Debug.Log("Spawning plus one powerup!");
        }
        powerUpSpawned = true;
    }

    // Moves the spawned poerup in the level at the same location as the incoming point
    private void MovePowerUp()
    {
        if (powerUpPrefab != null)
        {
            powerUpPrefab.position += new Vector3(-1, 0, 0) * POINT_SPEED * Time.deltaTime;
            if (powerUpPrefab.position.x <= -200)
            {
                Destroy(powerUpPrefab.gameObject);
            }
        }
    }

    public void SetFutureSightOn()
    {
        futureSightEnabled = true;
    }

    public void SetFutureSightOff()
    {
        futureSightEnabled = false;
    }

    // Spawns the marker when the future sight powerup is enabled
    private void SpawnFutureSightMarker()
    {
        // only create one at a time
        if (futureSightMarker == null)
        {
            futureSightMarkerSpawnTime -= Time.deltaTime;
            Debug.Log(futureSightMarkerSpawnTime);
            if (futureSightMarkerSpawnTime < 0)
            {
                // finds the y value of the incoming point and spawns there
                float nextY = pointList[pointList.Count - 1].GetYPosition();
                Transform newFutureSightMarker = Instantiate(GameAssets.GetInstance().futureSightMarkerPrefab);
                newFutureSightMarker.position = new Vector3(82, nextY, 0);
                futureSightMarker = newFutureSightMarker;
                Debug.Log("Spawning future sight marker!");
            }
        }
    }

    // when point reaches marker, destroy the marker so a new one can be created
    public void DestroyFutureSightMarker()
    {
        // Find and destroy all markers
        GameObject currentMarker = GameObject.FindWithTag("FutureSightMarker");
        Destroy(currentMarker);
        // Set this marker to null
        futureSightMarker = null;
    }


    /* ========== POINT FUNCTIONS ========== */

    // Moves all current points
    private void MovePoints()
    {
        int countRight = 0;
        for (int i = 0; i < pointList.Count; i++)
        {
            Point point = pointList[i];
            if(!point.CheckPastRocket())
            {
                countRight++;
            }
            bool isRightOfRocket = point.GetXPosition() > ROCKET_X_VALUE;
            point.Move();

            // Point was to the right of rocket and crossed over rocket's x value
            if (isRightOfRocket && point.GetXPosition() <= ROCKET_X_VALUE)
            {
                // increment consecutive hit
                consecutiveHit++;
                GenerateNewLine();

                // point is missed
                if (!point.GetIsHit())
                {
                    consecutiveHit = 0;
                    Rocket.GetInstance().IncrementTotalMissed();
                    SoundsManager.PlaySound(SoundsManager.Sound.MissedPoint);
                    Rocket.GetInstance().DecrementStrikesLeft();
                    ScoreOverlay.GetInstance().UpdateStrikeOverlay();
                }
                // check previous two points to set gravity
                if (i >= 1)
                {
                    Point prevPoint = pointList[i - 1];
                    if (prevPoint.GetYPosition() < point.GetYPosition())
                    {
                        Rocket.GetInstance().GravityUp();
                    }
                    else if (prevPoint.GetYPosition() > point.GetYPosition())
                    {
                        Rocket.GetInstance().GravityDown();
                    }
                }
            }

            // if point has reached the destroy point, destroy it
            if (point.GetXPosition() < DESTROY_X_VALUE)
            {
                point.DestroyPoint();
                pointList.Remove(point);
                i--;
            }
        }

        // if there are no more points to the right, set moreRight to indicate end of level
        if(countRight == 0)
        {
            moreRight = false;
        }
    }

    // Point spawn handler
    private void SpawnPoints()
    {
        pointSpawnTime -= Time.deltaTime;
        if (pointSpawnTime < 0)
        {
            // gets faster over time
            pointSpawnTime += pointSpawnTimeMax;

            // picks a random new y value within the pointGapMax range
            float newY = Random.Range(-pointGapMax, pointGapMax);
            CreatePoint(SPAWN_X_VALUE, newY);
            totalPoints++;
        }
    }

    private void CreatePoint(float xPosition, float yPosition)
    {
        Transform linePoint = Instantiate(GameAssets.GetInstance().linePointPrefab);
        linePoint.position = new Vector3(xPosition, yPosition);

        Point point = new Point(linePoint);
        pointList.Add(point);
    }

    public List<Point> GetPointList()
    {
        return pointList;
    }

    public int GetTotalPoints()
    {
        return totalPoints;
    }

    public void SetPointGapRange(float gapMax)
    {
        pointGapMax = gapMax;
    }

    // POINT HELPER CLASS
    public class Point
    {
        private Transform pointTransform;
        private bool isPastRocket;

        public Point(Transform pointTransform)
        {
            this.pointTransform = pointTransform;
        }

        public void Move()
        {
            pointTransform.position += new Vector3(-1, 0, 0) * POINT_SPEED * Time.deltaTime;
        }

        public float GetXPosition()
        {
            return pointTransform.position.x;
        }

        public float GetYPosition()
        {
            return pointTransform.position.y;
        }

        public bool CheckPastRocket()
        {
            if (pointTransform.position.x < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GetIsHit()
        {
            return pointTransform.GetComponent<LinePointPrefab>().isHit;
        }

        public void DestroyPoint()
        {
            Destroy(pointTransform.gameObject);
        }
    }

}
