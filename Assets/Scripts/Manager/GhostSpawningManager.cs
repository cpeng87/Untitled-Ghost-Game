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
    [SerializeField] private Vector3 door = new Vector3(-5.5f, 0.5f,7.5f);

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

    //A dictionary to contain a GameObject (key) and its assigned seat (value)
    IDictionary<GameObject, Vector3> ghost_with_seat;
    //A dictionary to contain a GameObject (key)and its change in position toward its seat (value)
    IDictionary<GameObject, Vector3> ghost_with_speed;
    //Sets up positions array and loads in ghost gameobjects based on ghostManager's active ghosts

    void Start()
    {
        positions = new Vector3[seats.Count];
        for (int i = 0; i < seats.Count; i++)
        {
            positions[i] = seats[i].transform.position;
        }
        ghostSpawnTimer = 0f;
        ghost_with_seat = new Dictionary<GameObject, Vector3>();
        ghost_with_speed = new Dictionary<GameObject, Vector3>();
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

        //For every GameObject that has been spawned (key) and calculated speed for said GameObject (value)
        foreach(KeyValuePair<GameObject, Vector3> entry in ghost_with_speed)
        {
            //If the GameObject's position is not at its seat, move it by its speed
            if (ghost_with_seat[entry.Key].x > entry.Key.transform.position.x)
            {
                entry.Key.transform.position += entry.Value;
            }
            //If it has moved to far, place it in its seat
            else
            {
                entry.Key.transform.position = ghost_with_seat[entry.Key];
            }
        }
    }
    
    //Spawning point for newly active ghosts, before they move to their seat
    // Vector3 door = new Vector3(-5.5f, 0.5f,7.5f);

    //Variable to store the amount of game updates between a ghost spawning at the door and getting to their seat
    float speed = 850;


    //reloads ghost objects based on ghostmanager's activeghosts
    public void UpdateGhostObjs()
    {
        //Commented out this code so that moving ghost wouldn't be deleted/respawned
        //delete currently spawned ghosts
        /*for (int i = 0; i < spawnedGhosts.Count; i++)
        {
            Destroy(spawnedGhosts[i]);
        }*/
        //spawnedGhosts = new List<GameObject>();

        Ghost[] activeGhosts = GameManager.Instance.ghostManager.activeGhosts;
        for (int i = 0; i < activeGhosts.Length; i++)
        {
            //Declare a boolean to represent if a seat has a GameObject already assigned to it
            bool seat_taken = false;

            //For every spawned ghost, check if their seat is the same as the ghost in activeGhosts
            foreach(KeyValuePair<GameObject, Vector3> keyValuePair in ghost_with_seat)
            {
                if (positions[i] == keyValuePair.Value) { seat_taken = true; break; }
            }

            //Only do this if the active ghost isn't null and the seat they're assigned to is not taken
            if (activeGhosts[i] != null && !seat_taken)
            {
                
                GameObject newGhost = Instantiate(GameManager.Instance.ghostManager.GetGhostObjFromName(activeGhosts[i].ghostName), door, Quaternion.identity);
                GhostObj ghostObj = newGhost.GetComponent<GhostObj>();
                ghostObj.SetSeatNum(i);
                if (GameManager.Instance.orderManager.HasActiveOrder(ghostObj.GetScriptable().ghostName) == true)
                {
                    ghostObj.SetHasTakenOrder(true);
                }

                //Add the GameObject to the two dictionaries, calculating the needed speed for the ghost_with_speed dictionary
                ghost_with_speed.Add(newGhost, new Vector3 ((positions[i].x - door.x)/speed, 
                    (positions[i].y - door.y)/speed, 
                    (positions[i].z - door.z)/speed));
                ghost_with_seat.Add(newGhost, positions[i]);

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
        ghost_with_seat.Remove(spawnedGhosts[seatNum]);
        ghost_with_speed.Remove(spawnedGhosts[seatNum]);
        Destroy(spawnedGhosts[seatNum]);
        spawnedGhosts.RemoveAt(seatNum);
    }

    public GameObject GetSpawnedGhost(int seatNum)
    {
        return spawnedGhosts[seatNum];
    }

    public GameObject GetSpawnedGhost(string name) {
        foreach(GameObject ghost in spawnedGhosts) {
            if (ghost.GetComponent<GhostObj>().GetScriptable().ghostName.Equals(name)) {
                return ghost;
            }
        }
        return null;
    }
}
