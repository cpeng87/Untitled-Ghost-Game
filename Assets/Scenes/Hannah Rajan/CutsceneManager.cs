using System.Linq;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance { get; private set; }
    public GameObject spawnLocationOne, spawnLocationTwo;
    public Camera cutsceneCamera;

    public GameObject mainCharacterObject;
    private Vector3 mainCharacterInitialPosition;
    private Ghost currentCutsceneGhost;

    //PANNING CAMERA VARIABLES
    public bool hasPanning, cutsceneTriggered;
    private float zoom;
    public float zoomMultiplier = 1.2f;
    public float minZoom = 3.2f;
    public float maxZoom = 5f;
    private float velocity = 0f;
    private float smoothTime = 0.25f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cutsceneTriggered = false;
        zoom = cutsceneCamera.orthographicSize;
        mainCharacterInitialPosition = mainCharacterObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //create "panning" effect only when called and if panning is applied
        if(hasPanning && cutsceneTriggered)
        {
            zoom -= Time.deltaTime * zoomMultiplier;
            zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
            cutsceneCamera.orthographicSize = Mathf.SmoothDamp(cutsceneCamera.orthographicSize, zoom, ref velocity, smoothTime);
        } else if (!hasPanning) //if you don't want the panning effect, that's always an option! :)
        {
            cutsceneCamera.orthographicSize = minZoom;
        }
    }

    public void TriggerCutscene()
    {
        if (TeleportNPCGhostToCutscene())
        {
            Debug.Log("Cutscene Triggered");
            cutsceneTriggered = true;
            GhostSpawningManagerWithCutscenes.Instance.isCutscene = true;

            mainCharacterObject.transform.position = spawnLocationTwo.transform.position;

            cutsceneCamera.orthographicSize = maxZoom;
            CameraManagerCutscene.Instance.SwapToMainCutsceneCamera();
        }   
    }

    public void LeaveCutscene()
    {
        cutsceneTriggered = false;
        GhostSpawningManagerWithCutscenes.Instance.isCutscene = false;

        mainCharacterObject.transform.position = mainCharacterInitialPosition;
        currentCutsceneGhost = null;

        cutsceneCamera.orthographicSize = maxZoom;
        CameraManagerCutscene.Instance.SwapToMainCamera();
    }

    public bool TeleportNPCGhostToCutscene(Ghost ghost)
    {
        if (!GameManager.Instance.ghostManager.HasActive()) //there are active ghosts to choose from
        {
            Debug.Log("Can't trigger cutscene - no active ghosts!");
        } else if (ghost == null) //non-null ghost
        {
            Debug.Log("Can't trigger cutscene - ghost has no value!");
        } else if (!GameManager.Instance.ghostManager.activeGhosts.Contains(ghost)) //valid ghost
        {
            Debug.Log("Can't trigger cutscene - invalid ghost!");
        } else
        {

            GameObject ghostGameObject = GameManager.Instance.ghostManager.GetGhostObjFromName(ghost.ghostName);
            ghostGameObject = GhostSpawningManagerWithCutscenes.Instance.getSpawnedGhostFromGameObject(ghostGameObject);

            Debug.Log(ghost.name);
            Debug.Log(ghostGameObject.transform.position);
            ghostGameObject.transform.position = spawnLocationOne.transform.position;
            Debug.Log(ghost);
            currentCutsceneGhost = ghost;
            return true;
        }
        Debug.Log(GameManager.Instance.ghostManager.activeGhosts);
        return false;
    }

    public bool TeleportNPCGhostToCutscene()
    {
        Ghost[] activeGhosts = GameManager.Instance.ghostManager.activeGhosts;
        return TeleportNPCGhostToCutscene(activeGhosts[(int)Random.Range(0, activeGhosts.Length)]);
        //for now, we choose a ghost at random from the active list.
        //later on, we can implement a trigger based on a minigame score but that's just a bit above my paygrade right now lol
    }
}
