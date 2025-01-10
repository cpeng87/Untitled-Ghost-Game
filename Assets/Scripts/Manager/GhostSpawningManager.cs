using UnityEngine;
using System.Collections.Generic;

public class GhostSpawningManager : MonoBehaviour
{
    public static GhostSpawningManager Instance { get; private set; }
    public List<GameObject> seats = new List<GameObject>(); //dependent on maxGhosts in gameManager, should be the same number
    private Vector3[] positions;
    private float ghostSpawnTimer = 0f;
    private List<GameObject> spawnedGhosts = new List<GameObject>();
    [SerializeField] private float ghostSpawnCooldown;

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
        for (int i = 0; i < seats.Count; i++)
        {
            positions[i] = seats[i].transform.position;
        }
        UpdateGhostObjs();
    }

    // Update is called once per frame
    void Update()
    {
        ghostSpawnTimer += Time.deltaTime;
        if (GameManager.Instance.ghostManager.HasActive() == false) //no ghosts in shop
        {
            SpawnGhost();
            ghostSpawnTimer = 0;
        }
        else if (ghostSpawnTimer > ghostSpawnCooldown)
        {
            SpawnGhost();
            ghostSpawnTimer = 0;
        }
    }

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
        Debug.Log("Possible ghosts size: " + possibleGhost.Count);
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

    public void DeleteSpawnedGhost(int seatNum)
    {
        
        Destroy(spawnedGhosts[seatNum]);
        spawnedGhosts.RemoveAt(seatNum);
    }
}
