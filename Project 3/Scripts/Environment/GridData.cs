using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GridData : MonoBehaviour
{
    private static GridData instance;
    public bool spritesHidden = false;
    public bool isComplete = false;
    public int totalMines;
    private string currentScene;

    int countRevealed;

    public GridEntry[] gridEntryArray;

    private GameObject levelCompletePortal;
    private Animation levelCompletePortalAnim;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        SetValues();
        ForceHideAllSprites();

        levelCompletePortal = GameObject.Find("LevelCompletePortal");
        levelCompletePortal.SetActive(false);

        levelCompletePortalAnim = levelCompletePortal.GetComponent<Animation>();
    }

    public static GridData GetInstance()
    {
        return instance;
    }

    public void SetValues()
    {
        PlayerMovement.GetInstance().ResetPositionToSpawn();

        int dimensions = (int)Mathf.Sqrt(gridEntryArray.Length);
        totalMines = (int)Mathf.Ceil(gridEntryArray.Length * .2f);
        int minesPlaced = 0;
        int xPos = 0;
        int yPos = 0;
        countRevealed = 0;



        foreach (GridEntry e in gridEntryArray)
        {
            e.position = new Vector2(xPos, yPos);
            xPos++;
            if (xPos == dimensions)
            {
                xPos = 0;
                yPos++;
            }

            e.isRevealed = false;
            e.isMarked = false;
            e.value = GridEntry.Value.Empty;
            e.go.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.magenta);
        }

        if (currentScene == "Tutorial")
        {
            gridEntryArray[0].value = GridEntry.Value.Empty;
            gridEntryArray[1].value = GridEntry.Value.Num_1;
            gridEntryArray[2].value = GridEntry.Value.Mine;
        }

        else
        {
            while (minesPlaced < totalMines)
            {
                int index = Random.Range(0, gridEntryArray.Length);
                GridEntry e = gridEntryArray[index];

                // if there is not already a mine placed here
                if (e.value != GridEntry.Value.Mine)
                {
                    // place mine and increment mine count
                    e.value = GridEntry.Value.Mine;
                    minesPlaced++;
                }
            }

            for (int i = 0; i < gridEntryArray.Length; i++)
            {
                GridEntry e = gridEntryArray[i];

                if (e.value == GridEntry.Value.Empty)
                {
                    e.neighborList = GetNeighbors(dimensions, i);

                    int mineCount = 0;

                    foreach (int index in e.neighborList)
                    {
                        Debug.Log(i + ": " + index);
                        GridEntry ge = gridEntryArray[index];
                        if (ge.value == GridEntry.Value.Mine)
                        {
                            mineCount++;
                        }
                    }

                    switch (mineCount)
                    {
                        case 1:
                            e.value = GridEntry.Value.Num_1;
                            break;
                        case 2:
                            e.value = GridEntry.Value.Num_2;
                            break;
                        case 3:
                            e.value = GridEntry.Value.Num_3;
                            break;
                        case 4:
                            e.value = GridEntry.Value.Num_4;
                            break;
                        case 5:
                            e.value = GridEntry.Value.Num_5;
                            break;
                        case 6:
                            e.value = GridEntry.Value.Num_6;
                            break;
                        case 7:
                            e.value = GridEntry.Value.Num_7;
                            break;
                        case 8:
                            e.value = GridEntry.Value.Num_8;
                            break;
                    }
                }
            }
        }


        // set all sprites to their underlying values
        foreach (GridEntry e in gridEntryArray)
        {
            e.go.transform.GetChild(0).GetComponent<ValueSpriteHandler>().SetSprite(e.value);
        }
    }

    public List<int> GetNeighbors (int size, int index)
    {
        List<int> neighborList = new List<int>();

        if (index % size != 0)
        {
            // LEFT
            neighborList.Add(index - 1);

            if (index - size > 0)
            {
                // LEFT DOWN
                neighborList.Add(index - (size + 1));
            }
            if (index + size <= gridEntryArray.Length)
            {
                // LEFT UP
                neighborList.Add(index + (size - 1));
            }
        }
        if (index % size != (size - 1))
        {
            // RIGHT
            neighborList.Add(index + 1);

            if (index - size >= 0)
            {
                // RIGHT DOWN
                neighborList.Add(index - (size - 1));
            }
            if (index + size < gridEntryArray.Length)
            {
                // RIGHT UP
                neighborList.Add(index + (size + 1));
            }
        }
        if (index - size >= 0)
        {
            // DOWN
            neighborList.Add(index - size);
        }
        if (index + size < gridEntryArray.Length)
        {
            // UP
            neighborList.Add(index + size);
        }

        return neighborList;
    }

    public GridEntry.Value RevealSelectedSprite(string objectName, bool droneTrigger)
    {
        GridEntry.Value revealedValue = GridEntry.Value.Empty;
        // Triggered by player: show contents and trigger mine (if hit)
        if (!droneTrigger)
        {
            foreach (GridEntry e in gridEntryArray)
            {
                if (e.go.name == objectName)
                {
                    if (!e.isRevealed)
                    {
                        e.go.transform.GetChild(0).gameObject.SetActive(true);
                        e.isRevealed = true;
                        countRevealed++;
                        revealedValue = e.value;

                        // if empty, reveal empty neighbors
                        if (e.value == GridEntry.Value.Empty)
                        {
                            RevealEmptyNeighbors(e);
                        }
                    }
                    break;
                }
            }
        }
        // Triggered by drone: show contents without triggering mine (if hit)
        else
        {
            foreach (GridEntry e in gridEntryArray)
            {
                if (e.go.name == objectName)
                {
                    if (!e.isRevealed)
                    {
                        e.go.transform.GetChild(0).gameObject.SetActive(true);
                        e.isRevealed = true;
                        revealedValue = e.value;

                        if (e.value != GridEntry.Value.Mine)
                        {
                            countRevealed++;
                        }
                    }
                    break;
                }
            }
        }

        if (countRevealed == gridEntryArray.Length - totalMines)
        {
            levelCompletePortal.SetActive(true);
            levelCompletePortalAnim["PortalOpen"].wrapMode = WrapMode.Once;
            levelCompletePortalAnim.Play("PortalOpen");
            PlayerState.Instance.LevelComplete();
        }

        return revealedValue;
    }

    // Recursive function to reveal all empty entries that are neighbors of each other
    public void RevealEmptyNeighbors(GridEntry e)
    {
        foreach (int index in e.neighborList)
        {
            GridEntry neighborEntry = gridEntryArray[index];
            if (neighborEntry.value != GridEntry.Value.Empty && !neighborEntry.isRevealed)
            {
                neighborEntry.go.transform.GetChild(0).gameObject.SetActive(true);
                neighborEntry.isRevealed = true;
                countRevealed++;
                // RevealEmptyNeighbors(neighborEntry);
            }
            if (neighborEntry.value == GridEntry.Value.Empty && !neighborEntry.isRevealed)
            {
                neighborEntry.go.transform.GetChild(0).gameObject.SetActive(true);
                neighborEntry.isRevealed = true;
                countRevealed++;
                RevealEmptyNeighbors(neighborEntry);
            }
        }
    }

    public void MarkSelectedSprite(string objectName)
    {
        foreach (GridEntry e in gridEntryArray)
        {
            if (e.go.name == objectName)
            {
                if (!e.isRevealed && !e.isMarked)
                {
                    // e.go.transform.GetChild(0).gameObject.SetActive(true);
                    e.isMarked = true;
                }
                if (e.isMarked)
                {
                    // e.go.transform.GetChild(0).gameObject.SetActive(true);
                    e.isMarked = false;
                }
                break;
            }
        }
    }

    public void ShowAllSprites()
    {
        // sprites currently hidden, show them
        if (spritesHidden)
        {
            foreach (GridEntry e in gridEntryArray)
            {
                e.go.transform.GetChild(0).gameObject.SetActive(true);
            }
            spritesHidden = false;
        }
    }

    public void HideAllSprites()
    {
        // sprites currently shown, hide them
        if (!spritesHidden)
        {
            foreach (GridEntry e in gridEntryArray)
            {
                if (!e.isRevealed)
                {
                    e.go.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            spritesHidden = true;
        }
    }

    public void ForceHideAllSprites()
    {
        // hide regardless of current status
        foreach (GridEntry e in gridEntryArray)
        {
            e.go.transform.GetChild(0).gameObject.SetActive(false);
        }
        spritesHidden = true;
    }

    public void ResetGrid()
    {
        SetValues();
        ForceHideAllSprites();
    }
}
