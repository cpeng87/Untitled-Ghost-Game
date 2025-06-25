using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using JetBrains.Annotations;
using System.Drawing.Text;
using Manager.RecipeShop;
using static UnityEngine.GraphicsBuffer;

public class GhostSpawningManager : MonoBehaviour
{
    public static GhostSpawningManager Instance { get; private set; }
    public List<GameObject> seats = new List<GameObject>(); //dependent on maxGhosts in gameManager, should be the same number
    private Vector3[] positions;  // extracted positions of the seats from the seats List, size = maxGhosts from gamemanager
    private float ghostSpawnTimer = 0f;  //keeps track of time passed 
    public (GameObject, bool)[] spawnedGhosts;  //keeps track of ghost gameobject spawned in the scene, and whether it is moving or not
    [SerializeField] private float ghostSpawnCooldown;  //time for new ghost spawn
    [SerializeField] private Vector3 door = new Vector3(-6f, 0.5f,-7f);
    [SerializeField] private float ghostSpeed = 5f;
    [SerializeField] private float rotationSpeed = 135f;
    private float minRotationSpeed = 45f;
    private Quaternion RotationGoal1 = Quaternion.Euler(0f, -90f, 0f);
    private Quaternion RotationGoal2 = Quaternion.Euler(0f, 0f, 0f);
    private bool isReaperSpawn = false;

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
        if (GameManager.Instance == null)
        {
            return;
        }
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
                /* DIRECTIONAL LOGIC (VECTORIZED, NO ROTATION)
                Vector3 direction = (positions[i] - spawnedGhosts[i].Item1.transform.position).normalized;
                spawnedGhosts[i].Item1.transform.position += ghostSpeed * direction * Time.deltaTime;
                if (Vector3.Distance(spawnedGhosts[i].Item1.transform.position, positions[i]) <= 0.1f)
                {
                    spawnedGhosts[i].Item2 = false;
                    spawnedGhosts[i].Item1.transform.position = positions[i];
                }
                */

                // DIRECTIONAL LOGIC (MANHATTAN DISTANCE w/ SMOOTH ROTATION)
                Vector3 distanceDiff = positions[i] - spawnedGhosts[i].Item1.transform.position;

                if (distanceDiff.z > 0f)
                {
                    if (Math.Abs(Quaternion.Angle(spawnedGhosts[i].Item1.transform.rotation, RotationGoal1)) > 5f)
                    {
                        Quaternion currRotation = spawnedGhosts[i].Item1.transform.rotation;
                        float smoothAdjustment = (Math.Abs(Quaternion.Angle(currRotation, RotationGoal1)) + minRotationSpeed) / 90; //formula which modifies the rotation speed to have smooth deceleration
                        spawnedGhosts[i].Item1.transform.rotation = Quaternion.RotateTowards(currRotation, RotationGoal1, smoothAdjustment * rotationSpeed * Time.deltaTime);
                    }
                    if (Math.Abs(Quaternion.Angle(spawnedGhosts[i].Item1.transform.rotation, RotationGoal1)) < 15f)
                    {
                        float relativeDistZ = Math.Abs(positions[i].z - spawnedGhosts[i].Item1.transform.position.z);
                        float totalDistZ = Math.Abs(door.z - positions[i].z);
                        float smoothAdjustment = (float) (relativeDistZ + totalDistZ * 0.67) / totalDistZ; //formula which modifies the movement speed to have smooth deceleration
                        spawnedGhosts[i].Item1.transform.position += ghostSpeed * smoothAdjustment * Time.deltaTime * Vector3.forward;
                    }
                }
                else
                {
                    if (Math.Abs(Quaternion.Angle(spawnedGhosts[i].Item1.transform.rotation, RotationGoal2)) > 5f)
                    {
                        Quaternion currRotation = spawnedGhosts[i].Item1.transform.rotation;
                        float smoothAdjustment = (Math.Abs(Quaternion.Angle(currRotation, RotationGoal2)) + minRotationSpeed) / 90; //formula which modifies the rotation speed to have smooth deceleration
                        spawnedGhosts[i].Item1.transform.rotation = Quaternion.RotateTowards(currRotation, RotationGoal2, smoothAdjustment * rotationSpeed * Time.deltaTime);
                    }
                    if (Math.Abs(Quaternion.Angle(spawnedGhosts[i].Item1.transform.rotation, RotationGoal2)) < 15f)
                    {
                        float relativeDistZ = Math.Abs(positions[i].x - spawnedGhosts[i].Item1.transform.position.x);
                        float totalDistX = Math.Abs(door.x - positions[i].x);
                        float smoothAdjustment = (float) (relativeDistZ + totalDistX * 0.67) / totalDistX; //formula which modifies the movement speed to have smooth deceleration
                        spawnedGhosts[i].Item1.transform.position += ghostSpeed * smoothAdjustment * Time.deltaTime * Vector3.right;
                    }
                }

                if (Vector3.Distance(spawnedGhosts[i].Item1.transform.position, positions[i]) <= 0.1f || distanceDiff.x < 0f) //0.1f positional tolerance, extra check to make sure ghost doesnt walk into the counter erm
                {
                    spawnedGhosts[i].Item2 = false;
                    spawnedGhosts[i].Item1.transform.position = positions[i]; //lock ghost into position
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
                
                GhostObj currGhostObj = newGhost.GetComponent<GhostObj>();
                currGhostObj.SetSeatNum(i);
                if (GameManager.Instance.orderManager.HasActiveOrder(activeGhosts[i].ghostName))
                {
                    currGhostObj.SetHasTakenOrder(true);
                }
                spawnedGhosts[i] = (newGhost,false);
            }
        }

    }

    public void SpawnGhost()
    {
        if (GameManager.Instance.ghostManager.IsActiveFull() == true)
        {
            return;
        }

        int reaperIndex = GameManager.Instance.ghostManager.GetStoryIndex("Reaper");

        //reaper spawns hardcoded >.> erm
        if (reaperIndex == 1)
        {  
            Ghost reaperScriptable = GameManager.Instance.ghostManager.GetGhostScriptableFromName("Reaper");
            if (GameManager.Instance.ghostManager.CheckGhostIsActive(reaperScriptable) == false)
            {
                GameManager.Instance.ghostManager.AddActiveGhost(reaperScriptable);
                int reaperSeatNum = GameManager.Instance.ghostManager.GetSeatNum(reaperScriptable);
                GameObject reaperObj = Instantiate(GameManager.Instance.ghostManager.GetGhostObjFromName("Reaper"), door, Quaternion.identity);
                reaperObj.GetComponent<GhostObj>().SetSeatNum(reaperSeatNum);
                spawnedGhosts[reaperSeatNum] = (reaperObj, true);
                AudioManager.Instance.PlaySound("DoorChime");
            }
            return;
        }

        if (isReaperSpawn)
        {
            Ghost reaperScriptable = GameManager.Instance.ghostManager.GetGhostScriptableFromName("Reaper");
            if (GameManager.Instance.ghostManager.CheckGhostIsActive(reaperScriptable) == false)
            {
                GameManager.Instance.ghostManager.AddActiveGhost(reaperScriptable);
                int reaperSeatNum = GameManager.Instance.ghostManager.GetSeatNum(reaperScriptable);
                GameObject reaperObj = Instantiate(GameManager.Instance.ghostManager.GetGhostObjFromName("Reaper"), door, Quaternion.identity);
                reaperObj.GetComponent<GhostObj>().SetSeatNum(reaperSeatNum);
                spawnedGhosts[reaperSeatNum] = (reaperObj, true);
                AudioManager.Instance.PlaySound("DoorChime");
            }
            isReaperSpawn = false;
            return;
        }

        //if finish arc ghosts
        
        List<Ghost> possibleGhost = new List<Ghost>();

        foreach (Recipe recipe in GameManager.Instance.unlockedRecipes)
        {
            List<Ghost> ghostRangePerRecipe = GameManager.Instance.ghostManager.GetGhostsFromRecipe(recipe);
            for (int i = ghostRangePerRecipe.Count - 1; i >= 0; i--) {
                Ghost g = ghostRangePerRecipe[i];
                if (GameManager.Instance.ghostManager.IsComplete(g))
                {
                    ghostRangePerRecipe.RemoveAt(i);
                }
            }
            possibleGhost.AddRange(ghostRangePerRecipe);
        }

        List<Ghost> arcGhost = new List<Ghost>();
        List<Ghost> sideGhost = new List<Ghost>();

        foreach (Ghost ghost in possibleGhost)
        {
            if (ghost.arc == GameManager.Instance.arc)
            {
                arcGhost.Add(ghost);
            }
            else
            {
                sideGhost.Add(ghost);
            }
        }

        if (arcGhost.Count != 0 && sideGhost.Count != 0)
        {
            float rand = UnityEngine.Random.value;
            if (rand < 0.65f) // main story ghost
            {
                possibleGhost = arcGhost;
            }
            else
            {
                possibleGhost = sideGhost;
            }
        }
        else if (arcGhost.Count == 0)
        {
            possibleGhost = sideGhost;
        }
        else
        {
            possibleGhost = arcGhost;
        }

        int index = (int) (UnityEngine.Random.value * possibleGhost.Count);
        index = Math.Abs(index);
        int count = 0;
        
        if (possibleGhost.Count == 0) {
            Debug.Log("No customers are left. All of them have been completed!");
            RecipeShopManager.Instance.SetRecipeShopNotif(true);
            return;
        }
        while (GameManager.Instance.ghostManager.CheckGhostIsActive(possibleGhost[index]) == true || possibleGhost[index].ghostName == "Reaper")
        {
            if (count > 100)
            {
                Debug.Log("Cannot spawn any ghost, maxed rolls");
                return;
            }
            index = (int) (UnityEngine.Random.value * possibleGhost.Count);
            count++;
        }
        GameManager.Instance.ghostManager.AddActiveGhost(possibleGhost[index]);
        int seatNum = GameManager.Instance.ghostManager.GetSeatNum(possibleGhost[index]);
        GameObject newGhost = Instantiate(GameManager.Instance.ghostManager.GetGhostObjFromName(possibleGhost[index].ghostName), door, Quaternion.identity);
        newGhost.GetComponent<GhostObj>().SetSeatNum(seatNum);
        spawnedGhosts[seatNum] = (newGhost, true);
        AudioManager.Instance.PlaySound("DoorChime");
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

    public void SetIsReaperSpawn(bool val)
    {
        isReaperSpawn = val;
    }

}

