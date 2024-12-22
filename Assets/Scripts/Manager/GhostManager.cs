using UnityEngine;
using System.Collections.Generic;

public class GhostManager : MonoBehaviour
{
    public List<GameObject> ghosts;
    public Dictionary<string, GameObject> ghostNameToGameObjDict = new Dictionary<string, GameObject>();
    public Dictionary<string, Ghost> ghostNameToScriptableDict = new Dictionary<string, Ghost>();

    public void Setup()
    {
        // settting up both dictionaries
        foreach (GameObject ghost in ghosts)
        {
            Ghost currGhost = ghost.GetComponent<GhostObj>().GetScriptable();
            ghostNameToScriptableDict.Add(currGhost.ghostName, currGhost);
            ghostNameToGameObjDict.Add(currGhost.ghostName, ghost);
        }
    }

    public GameObject GetGhostObjFromName(string name)
    {
        if (ghostNameToGameObjDict.ContainsKey(name))
        {
            return ghostNameToGameObjDict[name];
        }
        Debug.Log("Could not find ghost object with name: " + name);
        return null;
    }

    public List<Ghost> GetGhostScriptables()
    {
        return new List<Ghost>(ghostNameToScriptableDict.Values);
    }

    public Ghost GetGhostScriptableFromName(string name)
    {
        if (ghostNameToScriptableDict.ContainsKey(name))
        {
            return ghostNameToScriptableDict[name];
        }
        Debug.Log("Could not find ghost scriptable with name: " + name);
        return null;
    }
}
