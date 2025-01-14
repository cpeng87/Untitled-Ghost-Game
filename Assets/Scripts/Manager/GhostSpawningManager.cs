using UnityEngine;
using System.Collections.Generic;

public class GhostSpawningManager : MonoBehaviour
{
    public static GhostSpawningManager Instance { get; private set; }
    public List<GameObject> seats = new List<GameObject>(); //dependent on maxGhosts in gameManager, should be the same number
    private Vector3[] positions;  // extracted positions of the seats from the seats List, size = maxGhosts from gamemanager
    private float ghostSpawnTimer = 0f;  //keeps track of time passed 
    private List<GameObject> spawnedGhosts = new List<GameObject>();  //keeps track of ghost gameobject spawned in the scene
    [SerializeField] private float ghostSpawnCooldown;  //time for new ghost spawn

    //singleton
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Sets up positions array and loads in ghost gameobjects based on ghostManager's active ghosts
    void Start()
    {
        positions = new Vector3[seats.Count];
        for (int i = 0; i < seats.Count; i++)
        {
            positions[i] = seats[i].transform.position;
        }
        ghostSpawnTimer = 0f;
        UpdateGhostObjs();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.state == State.Main)
        {
            ghostSpawnTimer += Time.deltaTime;
        }
        if (GameManager.Instance.ghostManager.HasActive() == false && ghostSpawnTimer > 1.5f) //no ghosts in shop
        {
            SpawnGhost();
            ghostSpawnTimer = 0;
        }
        if (ghostSpawnTimer > ghostSpawnCooldown)
        {
            SpawnGhost();
            ghostSpawnTimer = 0;
        }
    }

    //reloads ghost objects based on ghostmanager's activeghosts
    public void UpdateGhostObjs()
    {
        //delete currently spawned ghosts
        for (int i = 0; i < spawnedGhosts.Count; i++)
        {
            Destroy(spawnedGhosts[i]);
        }
        spawnedGhosts = new List<GameObject>();
        Ghost[] activeGhosts = GameManager.Instance.ghostManager.activeGhosts;
        for (int i = 0; i < activeGhosts.Length; i++)
        {
            if (activeGhosts[i] != null)
            {
                GameObject newGhost = Instantiate(GameManager.Instance.ghostManager.GetGhostObjFromName(activeGhosts[i].ghostName), positions[i], Quaternion.identity);
                GhostObj ghostObj = newGhost.GetComponent<GhostObj>();
                ghostObj.SetSeatNum(i);
                if (GameManager.Instance.orderManager.HasActiveOrder(ghostObj.GetScriptable().ghostName) == true)
                {
                    ghostObj.SetHasTakenOrder(true);
                }
                spawnedGhosts.Add(newGhost);
            }
        }

    }

    //spawns a new ghost, randomly selects ghost based on possible (recipes unlocked), adds to active ghosts and reloads ghost gameobjects
    public void SpawnGhost()
    {
        Debug.Log("Spawning Ghost");
        if (GameManager.Instance.ghostManager.IsActiveFull() == true)
        {
            Debug.Log("Full seats!");
            return;
        }
        List<Ghost> possibleGhost = new List<Ghost>();
        foreach (Recipe recipe in GameManager.Instance.unlockedRecipes)
        {
            possibleGhost.AddRange(GameManager.Instance.ghostManager.GetGhostsFromRecipe(recipe));
        }
        //randomize a index to check spawn, if alr has active order, keep reroll, until no active order or 100 rerolls
        int index = (int) (Random.value * possibleGhost.Count);
        int count = 0;
        while (GameManager.Instance.ghostManager.CheckGhostIsActive(possibleGhost[index]) == true)
        {
            if (count > 100)
            {
                Debug.Log("Cannot spawn any ghost, maxed rolls");
                return;
            }
            index = (int) (Random.value * possibleGhost.Count);
            count++;
        }

        GameManager.Instance.ghostManager.AddActiveGhost(possibleGhost[index]);
        UpdateGhostObjs();
    }

    //deletes gameobject and removes from spawned ghosts list
    public void DeleteSpawnedGhost(int seatNum)
    {
        Debug.Log("seat num to destroy: " + seatNum);
        Destroy(spawnedGhosts[seatNum]);
        spawnedGhosts.RemoveAt(seatNum);
    }
}
