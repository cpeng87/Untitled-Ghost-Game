using UnityEngine;
using System.Collections.Generic;

public class GhostManager : MonoBehaviour
{
    public List<GameObject> ghosts;  //list of all ghosts 
    public Dictionary<string, GameObject> ghostNameToGameObjDict = new Dictionary<string, GameObject>();
    public Dictionary<string, Ghost> ghostNameToScriptableDict = new Dictionary<string, Ghost>();
    public Dictionary<string, int> ghostNameToStoryIndex = new Dictionary<string, int>();
    public Dictionary<Recipe, List<Ghost>> recipeToGhostsDict = new Dictionary<Recipe, List<Ghost>>();
    public Ghost[] activeGhosts; //array of ghosts that have been spawned. Limited by maxghosts in GameManager

    //setup dictionaries and activeghosts
    public void Setup()
    {
        // settting up dictionaries
        foreach (GameObject ghost in ghosts)
        {
            Ghost currGhost = ghost.GetComponent<GhostObj>().GetScriptable();
            ghostNameToScriptableDict.Add(currGhost.ghostName, currGhost);
            ghostNameToGameObjDict.Add(currGhost.ghostName, ghost);
            ghostNameToStoryIndex.Add(currGhost.ghostName, 0);
            foreach (Recipe recipe in currGhost.recipesOrdered)
            {
                if (recipeToGhostsDict.ContainsKey(recipe))
                {
                    recipeToGhostsDict[recipe].Add(currGhost);
                }
                else
                {
                    recipeToGhostsDict.Add(recipe, new List<Ghost>());
                    recipeToGhostsDict[recipe].Add(currGhost);
                }
            }
        }
        activeGhosts = new Ghost[GameManager.Instance.maxGhosts];
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

    public void IncrementStoryIndex(string name)
    {
        if (ghostNameToStoryIndex.ContainsKey(name))
        {
            Ghost ghost = GetGhostScriptableFromName(name);
            if (ghost != null)
                if (ghost.numStory <= ghostNameToStoryIndex[name])
                {
                    Debug.Log("Reached end of dialogue, will not increment");
                }
                else
                {
                    ghostNameToStoryIndex[name] = ghostNameToStoryIndex[name] + 1;
                }
            return;
        }
        Debug.Log("Ghost with name: " + name + " does not exist in the dictionary.");
    }

    public int GetStoryIndex(string name)
    {
        if (ghostNameToStoryIndex.ContainsKey(name))
        {
            return ghostNameToStoryIndex[name];
        }
        Debug.Log("Ghost with name: " + name + " does not exist in the dictionary.");
        return -1;
    }

    public List<Ghost> GetGhostsFromRecipe(Recipe recipe)
    {
        if (recipeToGhostsDict.ContainsKey(recipe))
        {
            return recipeToGhostsDict[recipe];
        }
        else
        {
            return new List<Ghost>();
        }
    }

    public bool CheckGhostIsActive(Ghost ghost)
    {
        foreach (Ghost otherGhost in activeGhosts)
        {
            if (ghost == otherGhost)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsActiveFull()
    {
        int activeCount = 0;

        foreach (Ghost ghost in activeGhosts)
        {
            if (ghost != null)
            {
                activeCount++;
            }
        }

        if (activeCount == GameManager.Instance.maxGhosts)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public bool HasActive()
    {
        foreach (Ghost ghost in activeGhosts)
        {
            if (ghost != null)
            {
                return true;
            }
        }
        return false;
    }


    public bool AddActiveGhost(Ghost newGhost)
    {
        if (!IsActiveFull())
        {
            for (int i = 0; i < activeGhosts.Length; i++)
            {
                if (activeGhosts[i] == null)
                {
                    activeGhosts[i] = newGhost;
                    return true;
                }
            }
        }
        
        Debug.Log("Max ghosts reached! No null spot found.");
        return false;
    }

    public void RemoveActiveGhost(Ghost ghost)
    {
        for (int i = 0; i < activeGhosts.Length; i++)
        {
            if (activeGhosts[i] == ghost)
            {
                activeGhosts[i] = null;
            }
        }
    }

    public int GetSeatNum(Ghost ghost)
    {
        for (int i = 0; i < activeGhosts.Length; i++)
        {
            if (activeGhosts[i] == ghost)
            {
                return i;
            }
        }
        return -1;
    }

}
