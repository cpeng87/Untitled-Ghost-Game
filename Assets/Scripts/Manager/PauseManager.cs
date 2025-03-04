using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

//EC: As a preface, this script is basically yoinked from Slider. It's a free game, you should go play it!
//Last I checked, this code was written by Travis Bittel
public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    private static readonly List<Pause> pauses = new();
    
    /// <summary>
    /// Called when the game is paused on unpaused. 
    /// Passes true if the game is now paused and false if the game is unpaused.
    /// </summary>
    public static Action<bool> PauseStateChanged;
    public static bool IsPaused { get; private set; } = false;

    private void Awake()
    {
        Instance = this;
    }
    /// <summary>
    /// Prevents the player from opening the pause menu. This is setup to support multiple locations
    /// allowing and disallowing pausing independently, so you don't need to worry about overwriting
    /// the existing state by calling this method. This is achieved by keeping a list of the current
    /// restrictions in place. When you want to allow pausing again, call <see cref="RemovePause(Pause)"/>
    /// with the <see cref="Pause"/> returned by this method to remove it.
    /// </summary>
    public static Pause AddPause(GameObject owner)
    {
        Pause restriction = new(owner);
        pauses.Add(restriction);
        return restriction;
    }

    public static void RemovePause(Pause restrictionToRemove)
    {
        Instance.StartCoroutine(IRemovePause(restrictionToRemove));
    }

    public static void RemovePause(GameObject owner)
    {
        List<Pause> restrictionsWithMatchingOwner
            = pauses.Where(restriction => restriction.owner == owner).ToList();
        
        Instance.StartCoroutine(IRemovePause(restrictionsWithMatchingOwner.ToArray()));
    }

    private static IEnumerator IRemovePause(params Pause[] restrictions)
    {
        yield return new WaitForEndOfFrame();
        restrictions.ToList().ForEach(restriction => pauses.Remove(restriction));
    }

    public static void RemoveAllPauseRestrictions()
    {
        pauses.Clear();
    }

    public static void SetPauseState(bool paused)
    {
        bool newPauseState = CanPause() && paused;
        if (newPauseState == IsPaused)
        {
            return;
        }
        Instance.StartCoroutine(IUpdateIsPausedAfterEndOfFrame(paused));
    }

    // We want to wait until the end the frame to unpause so we avoid double behavior
    // from inputs (like Escape unpausing and immediately re-pausing)
    private static IEnumerator IUpdateIsPausedAfterEndOfFrame(bool newValue)
    {
        yield return new WaitForEndOfFrame();

        IsPaused = newValue;
        Time.timeScale = IsPaused ? 0 : 1;
        PauseStateChanged?.Invoke(newValue);
    }

    /// <summary>
    /// The player cannot pause the game in some circumstances, such as if they are
    /// on the main menu or in a cutscene.
    /// </summary>
    public static bool CanPause()
    {
        //return !GameUI.instance.isMenuScene && pauseRestrictions.Count == 0;
        return pauses.Count > 0;
    }
    
    /// <summary>
    /// A flag used to prevent the player from pausing the game. The owner of the 
    /// Pause can be used to easily add and remove them without needing to store
    /// a reference to the Pause returned by <see cref="AddPause"/>.
    /// </summary>
    public struct Pause
    {
        public GameObject owner;

        public Pause(GameObject owner)
        {
            this.owner = owner;
        }
    }
}
