using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using JetBrains.Annotations;
using System.Drawing.Text;
using Manager.RecipeShop;
using static UnityEngine.GraphicsBuffer;
using System.Text.RegularExpressions;

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
    // [SerializeField] private float minRotationSpeed = 90f;
    [SerializeField] private float spinSpeed = 125f;
    private double sulkSpeed = 1f;
    private Quaternion RotationGoal1 = Quaternion.Euler(0f, -90f, 0f);
    private Quaternion RotationGoal2 = Quaternion.Euler(0f, 0f, 0f);
    private bool isReaperSpawn = false;
    [SerializeField] private float mainGhostChance;


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
        // spawn faster when nobody in cafe
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

                Vector3 distanceDiff = positions[i] - spawnedGhosts[i].Item1.transform.position;
                switch (GameManager.Instance.ghostManager.GetGhostScriptableFromName(spawnedGhosts[i].Item1.GetComponent<GhostObj>().GetScriptable().ghostName).walking)
                {
                    case Walking.Silly:
                        spawnedGhosts[i].Item1.transform.Rotate(0, -spinSpeed * Time.deltaTime, 0);
                        // DIRECTIONAL LOGIC (MANHATTAN DISTANCE w/ SMOOTH ROTATION) - move forward, turn left, move forward, turn right, move forward, snap to seat position
                        if (distanceDiff.x > 2)
                        {
                            spawnedGhosts[i].Item1.transform.position += ghostSpeed * Time.deltaTime * Vector3.right;
                        }
                        else if (distanceDiff.z > 0f)
                        {
                            spawnedGhosts[i].Item1.transform.position += ghostSpeed * Time.deltaTime * Vector3.forward;
                        }
                        else if (distanceDiff.x > 0f)
                        {
                            spawnedGhosts[i].Item1.transform.position += ghostSpeed * Time.deltaTime * Vector3.right;
                        }

                        if (Vector3.Distance(spawnedGhosts[i].Item1.transform.position, positions[i]) <= 0.1f || distanceDiff.x < 0f) //0.1f positional tolerance, extra check to make sure ghost doesnt walk into the counter erm
                        {
                            if (Math.Abs(Quaternion.Angle(spawnedGhosts[i].Item1.transform.rotation, RotationGoal2)) < 1f)
                            {
                                spawnedGhosts[i].Item2 = false;
                                spawnedGhosts[i].Item1.GetComponent<GhostObj>().SetSeated();
                            }
                        }
                        break;
                    case Walking.Sulking:
                        // DIRECTIONAL LOGIC (MANHATTAN DISTANCE w/ SMOOTH ROTATION) - move forward, turn left, move forward, turn right, move forward, snap to seat position
                        if (distanceDiff.x > 2)
                        {
                            spawnedGhosts[i].Item1.transform.position += ghostSpeed * Time.deltaTime * Vector3.right * (float)sulkSpeed;
                        }
                        else if (distanceDiff.z > 0f)
                        {
                            if (Math.Abs(Quaternion.Angle(spawnedGhosts[i].Item1.transform.rotation, RotationGoal1)) > 1f)
                            {
                                spawnedGhosts[i].Item1.transform.rotation = Quaternion.RotateTowards(spawnedGhosts[i].Item1.transform.rotation, RotationGoal1, rotationSpeed * Time.deltaTime);
                            } else
                            {
                                spawnedGhosts[i].Item1.transform.position += ghostSpeed * Time.deltaTime * Vector3.forward * (float)sulkSpeed;
                            }
                        }
                        else if (distanceDiff.x > 0f)
                        {
                            if (Math.Abs(Quaternion.Angle(spawnedGhosts[i].Item1.transform.rotation, RotationGoal2)) > 1f)
                            {
                                spawnedGhosts[i].Item1.transform.rotation = Quaternion.RotateTowards(spawnedGhosts[i].Item1.transform.rotation, RotationGoal2, rotationSpeed * Time.deltaTime);
                            } else
                            {
                                spawnedGhosts[i].Item1.transform.position += ghostSpeed * Time.deltaTime * Vector3.right * (float)sulkSpeed;
                            }
                        }

                        if (Vector3.Distance(spawnedGhosts[i].Item1.transform.position, positions[i]) <= 0.1f || distanceDiff.x < 0f) //0.1f positional tolerance, extra check to make sure ghost doesnt walk into the counter erm
                        {
                            spawnedGhosts[i].Item2 = false;
                            spawnedGhosts[i].Item1.GetComponent<GhostObj>().SetSeated();
                            //GameObject ghost = GameManager.Instance.ghostManager.GetGameObjFromName(spawnedGhosts[i].Item1.GetComponent<GhostObj>().GetScriptable().ghostName);
                            //ghost.GetComponent<GhostObj>().SetSeated(true);
                            //spawnedGhosts[i].Item1.transform.position = positions[i]; //lock ghost into position
                            spawnedGhosts[i].Item1.transform.rotation = RotationGoal2;
                        } else
                        {
                            sulkSpeed -= 0.65f * Time.deltaTime;
                            if (sulkSpeed < 0)
                            {
                                sulkSpeed = 1f;
                            }
                        }
                        break;
                    default:
                        // DIRECTIONAL LOGIC (MANHATTAN DISTANCE w/ SMOOTH ROTATION) - move forward, turn left, move forward, turn right, move forward, snap to seat position
                        if (distanceDiff.x > 2)
                        {
                            spawnedGhosts[i].Item1.transform.position += ghostSpeed * Time.deltaTime * Vector3.right;
                        }
                        else if (distanceDiff.z > 0f)
                        {
                            if (Math.Abs(Quaternion.Angle(spawnedGhosts[i].Item1.transform.rotation, RotationGoal1)) > 1f)
                            {
                                spawnedGhosts[i].Item1.transform.rotation = Quaternion.RotateTowards(spawnedGhosts[i].Item1.transform.rotation, RotationGoal1, rotationSpeed * Time.deltaTime);
                            } else
                            {
                                spawnedGhosts[i].Item1.transform.position += ghostSpeed * Time.deltaTime * Vector3.forward;
                            }
                        }
                        else if (distanceDiff.x > 0f)
                        {
                            if (Math.Abs(Quaternion.Angle(spawnedGhosts[i].Item1.transform.rotation, RotationGoal2)) > 1f)
                            {
                                spawnedGhosts[i].Item1.transform.rotation = Quaternion.RotateTowards(spawnedGhosts[i].Item1.transform.rotation, RotationGoal2, rotationSpeed * Time.deltaTime);
                            } else
                            {
                                spawnedGhosts[i].Item1.transform.position += ghostSpeed * Time.deltaTime * Vector3.right;
                            }
                        }

                        if (Vector3.Distance(spawnedGhosts[i].Item1.transform.position, positions[i]) <= 0.1f || distanceDiff.x < 0f) //0.1f positional tolerance, extra check to make sure ghost doesnt walk into the counter erm
                        {
                            spawnedGhosts[i].Item2 = false;
                            spawnedGhosts[i].Item1.GetComponent<GhostObj>().SetSeated();
                            //GameObject ghost = GameManager.Instance.ghostManager.GetGameObjFromName(spawnedGhosts[i].Item1.GetComponent<GhostObj>().GetScriptable().ghostName);
                            //ghost.GetComponent<GhostObj>().SetSeated(true);
                            //spawnedGhosts[i].Item1.transform.position = positions[i]; //lock ghost into position
                            spawnedGhosts[i].Item1.transform.rotation = RotationGoal2;
                        }
                        break;
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
            spawnedGhosts[i] = (null, false);
        }

        for (int i = 0; i < activeGhosts.Length; i++)
        {
            if (activeGhosts[i] != null)
            {
                GameObject newGhost = Instantiate(GameManager.Instance.ghostManager.GetGameObjFromName(activeGhosts[i].ghostName), positions[i], Quaternion.identity);

                GhostObj currGhostObj = newGhost.GetComponent<GhostObj>();
                currGhostObj.SetSeatNum(i);
                if (GameManager.Instance.orderManager.HasActiveOrder(activeGhosts[i].ghostName))
                {
                    currGhostObj.SetHasTakenOrder();
                }
                else
                {
                    currGhostObj.SetCanTakeOrder();
                }
                spawnedGhosts[i] = (newGhost, false);
            }
        }
    }

    public void SpawnReaper()
    {
        Ghost reaperScriptable = GameManager.Instance.ghostManager.GetGhostScriptableFromName("Reaper");
        if (GameManager.Instance.ghostManager.CheckGhostIsActive(reaperScriptable) == false)
        {
            GameManager.Instance.ghostManager.AddActiveGhost(reaperScriptable);
            int reaperSeatNum = GameManager.Instance.ghostManager.GetSeatNum(reaperScriptable);
            GameObject reaperObj = Instantiate(GameManager.Instance.ghostManager.GetGameObjFromName("Reaper"), door, Quaternion.identity);
            reaperObj.GetComponent<GhostObj>().SetSeatNum(reaperSeatNum);
            spawnedGhosts[reaperSeatNum] = (reaperObj, true);
            AudioManager.Instance.PlaySound("DoorChime");
        }
        isReaperSpawn = false;
    }
    
    public void SpawnGhost()
    {
        if (GameManager.Instance.ghostManager.IsActiveFull() == true)
        {
            return;
        }

        int reaperIndex = GameManager.Instance.ghostManager.GetStoryIndex("Reaper");

        // spawns reaper if first or is 
        if (reaperIndex == 1 || isReaperSpawn)
        {  
            SpawnReaper();
            return;
        }

        //if finish arc ghosts
        
        List<Ghost> possibleGhost = new List<Ghost>();

        foreach (Ghost ghost in GameManager.Instance.ghostManager.GetGhostScriptables())
        {
            if ((int)ghost.arc <= (int)GameManager.Instance.arc)
            {
                if (GameManager.Instance.ghostManager.IsComplete(ghost) == false)
                {
                    possibleGhost.Add(ghost);
                }
            }
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

        // this will probably make is so that reaper is the only thing spawned when all arc ghosts are complete
        if (arcGhost.Count == 0)
        {
            SpawnReaper();
            return;
        }

        if (arcGhost.Count != 0 && sideGhost.Count != 0)
        {
            float rand = UnityEngine.Random.value;
            if (rand < mainGhostChance) // main story ghost
            {
                possibleGhost = arcGhost;
            }
            else
            {
                possibleGhost = sideGhost;
            }
        }

        // else if (arcGhost.Count == 0)
        // {
        //     possibleGhost = sideGhost;
        // }
        // else
        // {
        //     possibleGhost = arcGhost;
        // }

        int index = (int) (UnityEngine.Random.value * possibleGhost.Count);
        index = Math.Abs(index);
        int count = 0;
        
        if (possibleGhost.Count == 0) {
            Debug.Log("No customers are left. All of them have been completed! Something went wrong since reaper is supposed to spawn.");
        }
        if (index >= possibleGhost.Count || index < 0)
        {
            return;
        }
        while (GameManager.Instance.ghostManager.CheckGhostIsActive(possibleGhost[index]) == true || possibleGhost[index].ghostName == "Reaper")
        {
            possibleGhost.RemoveAt(index);
            if (possibleGhost.Count == 0)
            {
                return;
            }
            if (count > 100)
            {
                Debug.Log("Maxed rolls. Should not have happened.");
                return;
            }
            index = (int) (UnityEngine.Random.value * possibleGhost.Count);
            // possibleGhost.RemoveAt(index);
            // if (possibleGhost.Count == 0)
            // {
            //     return;
            // }
            count++;
        }
        GameManager.Instance.ghostManager.AddActiveGhost(possibleGhost[index]);
        int seatNum = GameManager.Instance.ghostManager.GetSeatNum(possibleGhost[index]);
        GameObject newGhost = Instantiate(GameManager.Instance.ghostManager.GetGameObjFromName(possibleGhost[index].ghostName), door, Quaternion.identity);
        newGhost.GetComponent<GhostObj>().SetSeatNum(seatNum);
        spawnedGhosts[seatNum] = (newGhost, true);
        AudioManager.Instance.PlaySound("DoorChime");
    }

    //deletes gameobject and removes from spawned ghosts list
    public void DeleteSpawnedGhost(int seatNum)
    {
        Destroy(spawnedGhosts[seatNum].Item1);
        spawnedGhosts[seatNum] = (null, false);
    }

    public GameObject GetSpawnedGhost(int seatNum)
    {
        return spawnedGhosts[seatNum].Item1;
    }

    public List<GameObject> GetAllSpawnedGhost()
    {
        List<GameObject> rtn = new List<GameObject>();
        foreach ((GameObject, bool) spawnedGhost in spawnedGhosts)
        {
            if (spawnedGhost.Item1 != null)
            {
                rtn.Add(spawnedGhost.Item1);
            }
        }
        return rtn;
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

