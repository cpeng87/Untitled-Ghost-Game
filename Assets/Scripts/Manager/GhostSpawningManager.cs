using UnityEngine;
using System.Collections.Generic;

public class GhostSpawningManager : MonoBehaviour
{
    public static GhostSpawningManager Instance { get; private set; }
    public List<GameObject> seats = new List<GameObject>(); //dependent on maxGhosts in gameManager, should be the same number
    private Vector3[] positions;  // extracted positions of the seats from the seats List, size = maxGhosts from gamemanager
    private float ghostSpawnTimer = 0f;  //keeps track of time passed 
    public (GameObject, bool)[] spawnedGhosts;  //keeps track of ghost gameobject spawned in the scene, and whether it is moving or not
    [SerializeField] private float ghostSpawnCooldown;  //time for new ghost spawn
    [SerializeField] private Vector3 door = new Vector3(-6f, 0.5f,-7f);
    [SerializeField] private float ghostSpeed = 3f;

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

    void Start()
    {
        positions = new Vector3[seats.Count];
        spawnedGhosts = new (GameObject, bool)[seats.Count];
        for (int i = 0; i < seats.Count; i++)
        {
            positions[i] = seats[i].transform.position;
        }
        ghostSpawnTimer = 0f;
        UpdateGhostObjs();
    }

    void Update()
    {
        Debug.Log(GameManager.Instance.state);
        if (GameManager.Instance.state == State.Main)
        {
            ghostSpawnTimer += Time.deltaTime;
        }
        if (GameManager.Instance.ghostManager.HasActive() == false && ghostSpawnTimer > 1.5f)
        {
            SpawnGhost();
            ghostSpawnTimer = 0;
        }
        if (ghostSpawnTimer > ghostSpawnCooldown)
        {
            SpawnGhost();
            ghostSpawnTimer = 0;
        }

        for (int i = 0; i < spawnedGhosts.Length; i++)
        {
            // if it is moving we move it
            if (spawnedGhosts[i].Item2)
            {
                Vector3 direction = (positions[i] - spawnedGhosts[i].Item1.transform.position).normalized;
                spawnedGhosts[i].Item1.transform.position += ghostSpeed * direction * Time.deltaTime;
                if (Vector3.Distance(spawnedGhosts[i].Item1.transform.position, positions[i]) <= 0.005f)
                {
                    spawnedGhosts[i].Item2 = false;
                    spawnedGhosts[i].Item1.transform.position = positions[i];
                }
            }
        }
    }

    //reloads ghost objects based on ghostmanager's activeghosts
    public void UpdateGhostObjs()
    {
        Ghost[] activeGhosts = GameManager.Instance.ghostManager.activeGhosts;
        //delete currently spawned ghosts
        for (int i = 0; i < spawnedGhosts.Length; i++)
        {
            spawnedGhosts[i] = (null,false);
        }

        for (int i = 0; i < activeGhosts.Length; i++)
        {
            if (activeGhosts[i] != null)
            {
                GameObject newGhost = Instantiate(GameManager.Instance.ghostManager.GetGhostObjFromName(activeGhosts[i].ghostName), positions[i], Quaternion.identity);
                newGhost.GetComponent<GhostObj>().SetSeatNum(i);
                spawnedGhosts[i] = (newGhost,false);
            }
        }

    }

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
        int seatNum = GameManager.Instance.ghostManager.GetSeatNum(possibleGhost[index]);
        GameObject newGhost = Instantiate(GameManager.Instance.ghostManager.GetGhostObjFromName(possibleGhost[index].ghostName), door, Quaternion.identity);
        newGhost.GetComponent<GhostObj>().SetSeatNum(seatNum);
        spawnedGhosts[seatNum] = (newGhost, true);
    }

    //deletes gameobject and removes from spawned ghosts list
    public void DeleteSpawnedGhost(int seatNum)
    {
        Destroy(spawnedGhosts[seatNum].Item1);
        spawnedGhosts[seatNum] = (null,false);
    }

    public GameObject GetSpawnedGhost(int seatNum)
    {
        return spawnedGhosts[seatNum].Item1;
    }

    public GameObject GetSpawnedGhost(string name) {
        foreach((GameObject,bool) ghost in spawnedGhosts) {
            if (ghost.Item1.GetComponent<GhostObj>().GetScriptable().ghostName.Equals(name)) {
                return ghost.Item1;
            }
        }
        return null;
    }
}
